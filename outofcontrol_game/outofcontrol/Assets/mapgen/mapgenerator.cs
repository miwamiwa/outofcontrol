using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapgenerator : MonoBehaviour
{
    public GameObject testcube;

    int gridDefinition = 20;

    private List<List<int>> grid;
    //  [System.NonSerialized]
    private List<List<List<int>>> blobs;
  //  int[][][] blobs;

    int nodeSpacing = 6;
    int maxTreeCount = 30; // maxtree per node
    float nodeMargin = 0.8f; // minimum space between growing nodes 
    int treeCounter = 0;
    int blobDefinition = 0;
    int nodeCount = 0;

    float gridRatio = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        Renderer rend;
        rend = GetComponent<Renderer>();
        gridRatio = rend.bounds.extents.x / gridDefinition;
        blobDefinition = gridDefinition / nodeSpacing + 1;
        nodeCount = blobDefinition * blobDefinition;

        grid= new List<List<int>>(20);
        blobs = new List<List<List<int>>>(16);


        // setup grid array
        // syntax grid[x][y]=blobIndex;
        //grid = new int[gridDefinition][];
        for (int i=0; i<gridDefinition; i++)
        {
            grid.Add(new List<int>());
           // grid[i] = ;
            for (int j=0; j<gridDefinition; j++)
            {
                grid[i].Add(-1);
            }
        }

        // setup blob array

        // syntax blob[nodeIndex][tilelist][x,y]
        
     //   blobs = new int[nodeCount][][];
        for(int i=0; i< nodeCount; i++)
        {
            blobs.Add(new List<List<int>>());

            for (int j = 0; j < maxTreeCount; j++)
            {
              

                blobs[i].Add(new List<int> ());
                blobs[i][j].Add(-1);
                blobs[i][j].Add(-1);
            }
        }

        

        

        // setup blob nodes 
        int blobCounter = 0;
        for(int x=0; x<gridDefinition; x++)
        {
            for(int y=0; y < gridDefinition; y++)
            {
                if (x % nodeSpacing == 0 && y % nodeSpacing == 0)
                {
                    int offsetX = Random.Range(-2, 2);
                    int offsetY = Random.Range(-2, 2);
                  //  Debug.Log(blobCounter);
                    int blobX = Mathf.Clamp(x + offsetX, 0, gridDefinition);
                    int blobY = Mathf.Clamp(y + offsetY, 0, gridDefinition);
                    //Debug.Log(blobX + " " + blobY);
                    blobs[blobCounter][0][0] = blobX;
                    blobs[blobCounter][0][1] = blobY;

                   // Debug.Log(blobs[blobCounter][0][0]+" "+ blobs[blobCounter][0][1]);
                    grid[blobX][blobY] = blobCounter;
                    blobCounter++;
                }
            }
        }

       // Debug.Log("blob counter: " + blobCounter + ", node count: " + nodeCount);
      //  Debug.Log(blobs[3][0][1]);
        // grow the nodes 
        // setup candidates array 
        // syntax candidates[index]=[x,y];
        int[][] candidates = new int[100][];
        for (int i = 0; i < candidates.Length; i++)
        {
            candidates[i] = new int[2];
        }
        int candidateCount = 0;


        for (int t=0; t<60; t++)
        {
           // Debug.Log(blobs[3][0][1]);
            if (treeCounter >= 0)
            {

                

                // for each blob node
                for (int j = 0; j < nodeCount; j++)
                {

                    // for each element in this blob node's tile list so far 
                    for (int k = 0; k < 3; k++)
                    {
                       // Debug.Log("i: " + treeCounter + ", j: " + j + ", k: " + k);
                      //  Debug.Log(blobs[j][k][0] + " " + blobs[j][k][1]);
                        if (candidateCount < 100 )
                        {
                           // Debug.Log(blobs[3][0][1]);
                            if (checkCandidate(blobs[j][k][0] + 1, blobs[j][k][1], j))
                            {
                               
                                candidates[candidateCount][0] = blobs[j][k][0] + 1;
                                candidates[candidateCount][1] = blobs[j][k][1];
                                candidateCount++;
                            }

                            if (checkCandidate(blobs[j][k][0], blobs[j][k][1] + 1, j))
                            {
                                candidates[candidateCount][0] = blobs[j][k][0];
                                candidates[candidateCount][1] = blobs[j][k][1] + 1;
                                candidateCount++;
                            }

                            if (checkCandidate(blobs[j][k][0] - 1, blobs[j][k][1], j))
                            {
                                candidates[candidateCount][0] = blobs[j][k][0] - 1;
                                candidates[candidateCount][1] = blobs[j][k][1];
                                candidateCount++;
                            }

                            if (checkCandidate(blobs[j][k][0], blobs[j][k][1] - 1, j))
                            {
                                candidates[candidateCount][0] = blobs[j][k][0];
                                candidates[candidateCount][1] = blobs[j][k][1] - 1;
                                candidateCount++;
                            }
                        }



                        // x: blobs[j][k][0]
                        // y: blobs[j][k][1]


                    }

                    if (candidateCount > 0)
                    {
                        int pick = Random.Range(0, candidateCount);
                    //    Debug.Log(candidateCount);

                        grid[candidates[pick][0]][candidates[pick][1]] = j;
                        blobs[j][t][0] = candidates[pick][0];
                        blobs[j][t][1] = candidates[pick][1];
                        Vector2 randomOffset =0.4f * Random.insideUnitCircle;
                        Instantiate(
                                testcube,
                                rend.bounds.center + new Vector3(
                                    1+randomOffset.x + 0.9f*rend.bounds.extents.x * (2f * candidates[pick][0] / gridDefinition - 1f),
                                    2.0f,
                                    1+randomOffset.y + 0.9f * rend.bounds.extents.z * (2f * candidates[pick][1] / gridDefinition - 1f)),
                                Quaternion.identity
                                );
                        //   Debug.Log("candidate count: "+candidateCount+", pick " + pick + ", x: " + blobs[j][i][0] + ", y: " + blobs[j][i][1]);
                        candidateCount = 0;
                    }
                    
                }
            }

        }




            if (treeCounter >= maxTreeCount)
            {

                // get ground tile bounds 
                
                // test placing something at the end
                Instantiate(testcube, rend.bounds.center + new Vector3(rend.bounds.extents.x, 0.5f, 0f), Quaternion.identity);

                for (int i = 0; i < blobs.Count; i++)
                {
                    for (int j = 0; j < blobs[i].Count; j++)
                    {
                    Vector2 randomOffset = 3f*Random.insideUnitCircle;
                       // Debug.Log("make tree");
                        GameObject newcube = Instantiate(
                            testcube,
                            rend.bounds.center + new Vector3(
                                randomOffset.x + rend.bounds.extents.x * (2 * blobs[i][j][0] / gridDefinition - 1),
                                0.5f,
                                randomOffset.y + rend.bounds.extents.z * (2 * blobs[i][j][1] / gridDefinition - 1)),
                            Quaternion.identity
                            );

                    

                }
            }

                treeCounter = -1;
            }
        


    }

    bool checkCandidate(int x, int y, int blob)
    {
        bool rejected = false;


        // if candidate out of bounds
        if (x < 0 || x >= gridDefinition || y < 0 || y >= gridDefinition) rejected = true;
      //  Debug.Log(rejected);
        // if grid tile is occupied 
        if (!rejected)
        {
            if (grid[x][y] != -1) rejected = true;
        }
        // Debug.Log(rejected);
        // if any of the other blobs are in range 
        //Debug.Log(blobs.Count);
        for (int i=0; i<blobs.Count; i++)
        {
           // Debug.Log(blobs[i].Count);
            for(int j=0; j<blobs[i].Count; j++)
            {
              //  Debug.Log(blobs[i][j][0]);
                Vector2 distance = new Vector2(blobs[i][j][0], blobs[i][j][1]) - new Vector2(x, y);
               // Debug.Log("d: " + distance.magnitude);
                if (distance.magnitude*gridRatio < nodeMargin && i!=blob) rejected = true;
            }
        }
      //  Debug.Log(rejected);
       // Debug.Log(x+", "+ y + ", " + blob);
       // Debug.Log(rejected);
        return !rejected;
    }

    // Update is called once per frame
    void Update()
    {
       

        
    }
}
