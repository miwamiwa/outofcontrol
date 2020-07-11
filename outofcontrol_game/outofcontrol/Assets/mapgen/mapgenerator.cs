using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapgenerator : MonoBehaviour
{
    public GameObject testcube;

    int gridDefinition = 20;

    int[][] grid;
    int[][][] blobs;

    int nodeSpacing = 6;
    int maxTreeCount = 20;
    int nodeMargin = 3; // minimum space between growing nodes 

    // Start is called before the first frame update
    void Start()
    {   
        // setup arrays
        grid = new int[gridDefinition][];
        for (int i=0; i<gridDefinition; i++)
        {
            grid[i] = new int[gridDefinition];
            for(int j=0; j<gridDefinition; j++)
            {
                grid[i][j] = -1;
            }
        }

        int blobDefinition = gridDefinition / nodeSpacing +1;
        int blobCount = blobDefinition * blobDefinition;
        blobs = new int[blobCount][][];
        for(int i=0; i<blobCount; i++)
        {
            blobs[i] = new int[maxTreeCount][];
            for (int j = 0; j < maxTreeCount; j++)
            {
                blobs[i][j] = new int[2];
                blobs[i][j][0] = -1;
                blobs[i][j][1] = -1;
            }
        }
        


        Renderer rend;
        rend = GetComponent<Renderer>();
        Debug.Log(rend.bounds);
        // test placing something at the end
        Instantiate(testcube, rend.bounds.center + new Vector3(rend.bounds.extents.x , 0.5f, 0f), Quaternion.identity);

        // setup blobs
        int blobCounter = 0;
        for(int x=0; x<gridDefinition; x++)
        {
            for(int y=0; y < gridDefinition; y++)
            {
                if (x % nodeSpacing == 0 && y % nodeSpacing == 0)
                {
                    int offsetX = Random.Range(-2, 2);
                    int offsetY = Random.Range(-2, 2);
                    Debug.Log(blobCounter);
                    int blobX = Mathf.Clamp(x + offsetX, 0, gridDefinition);
                    int blobY = Mathf.Clamp(y + offsetY, 0, gridDefinition);

                    blobs[blobCounter][0][0] = blobX;
                    blobs[blobCounter][0][1] = blobY;

                    grid[blobX][blobY] = blobCounter;
                    blobCounter++;
                }
            }
        }

        int[][] candidates = new int[100][];
        for(int i=0; i<candidates.Length; i++)
        {
            candidates[i] = new int[2];
        }
        int candidateCount = 0;

        // grow the nodes
        for(int i=0; i<maxTreeCount; i++)
        {
            for(int j=0; j<blobCount; j++)
            {

                for(int k=0; k<i; k++)
                {
                    Debug.Log(blobs[j][k][0]);
                    if (candidateCount<100&&checkCandidate(blobs[j][k][0] + 1, blobs[j][k][1], j))
                    {
                        candidates[candidateCount][0] = blobs[j][k][0] + 1;
                        candidates[candidateCount][1] = blobs[j][k][1];
                        candidateCount++;
                    }

                    if (candidateCount < 100 && checkCandidate(blobs[j][k][0], blobs[j][k][1] + 1, j))
                    {
                        candidates[candidateCount][0] = blobs[j][k][0] ;
                        candidates[candidateCount][1] = blobs[j][k][1] + 1;
                        candidateCount++;
                    }

                    if (candidateCount < 100 && checkCandidate(blobs[j][k][0] - 1, blobs[j][k][1], j))
                    {
                        candidates[candidateCount][0] = blobs[j][k][0] - 1;
                        candidates[candidateCount][1] = blobs[j][k][1];
                        candidateCount++;
                    }

                    if (candidateCount < 100 && checkCandidate(blobs[j][k][0], blobs[j][k][1] - 1, j))
                    {
                        candidates[candidateCount][0] = blobs[j][k][0] ;
                        candidates[candidateCount][1] = blobs[j][k][1] - 1;
                        candidateCount++;
                    }


                    // x: blobs[j][k][0]
                    // y: blobs[j][k][1]


                }

                int pick = Random.Range(0, candidateCount);
                grid[candidates[pick][0]][candidates[pick][1]] = j;
                blobs[j][i][0] = candidates[pick][0];
                blobs[j][i][1] = candidates[pick][1];
                candidateCount = 0;
            }
        }

        for(int i=0; i<blobs.Length; i++)
        {
            for(int j=0; j<blobs[i].Length; j++)
            {
               // Debug.Log("boom "+ blobs[i][j][0]+" " + blobs[i][j][1]);
                
                Instantiate(
                    testcube, 
                    rend.bounds.center + new Vector3(
                        rend.bounds.extents.x * (2*blobs[i][j][0]/gridDefinition -1),
                        0.5f,
                        rend.bounds.extents.z * (2 * blobs[i][j][1] / gridDefinition - 1)), 
                    Quaternion.identity
                    );
            }
        }
    }

    bool checkCandidate(int x, int y, int blob)
    {
        bool rejected = false;


        // if candidate out of bounds
        if (x < 0 || x >= gridDefinition || y < 0 || y >= gridDefinition) rejected = true;

        // if grid tile is occupied 
        if (!rejected)
        {
            if (grid[x][y] != -1) rejected = true;
        }

        // if any of the other blobs are in range 

        for(int i=0; i<blobs.Length; i++)
        {
            for(int j=0; j<blobs[i].Length; j++)
            {
                Vector2 distance = new Vector2(blobs[i][j][0], blobs[i][j][1]) - new Vector2(x, y);
                if (distance.magnitude <= nodeMargin && i!=blob) rejected = true;
            }
        }

        return !rejected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
