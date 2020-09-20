﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellGameManager : MonoBehaviour
{
    //The treasure chest prefab object
    public GameObject TreasureChest_PO;

    //The number of treasure chests that spawn
    public int NumTreasureChests = 3;
    //The distance between each chest
    public float TreasureChestSpacing = 5.0f;


    //The list of spawned treasure chests
    protected List<GameObject> treasureChests;

    //Are we shuffling the chests right now?
    protected bool bShuffling;
    //The time it takes to do one chest shuffle
    protected float shuffleTime = 1.0f;
    //The current time in the current shuffle
    protected float currentShuffleTime = 0.0f;


    //The game objects that we are shuffling
    protected GameObject go1;
    protected GameObject go2;
    //The start and end positions for the shuffle
    protected Vector3 go1Start;
    protected Vector3 go1End;
    protected Vector3 go2Start;
    protected Vector3 go2End;




    // Start is called before the first frame update
    void Start()
    {
        treasureChests = new List<GameObject>();

        SpawnTreasureChests();

        StartCoroutine(WaitToStartBeginShuffle());
    }

    IEnumerator WaitToStartBeginShuffle()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);

        BeginShuffle();
    }

    void SpawnTreasureChests()
    {
        for (int i = 0; i < NumTreasureChests; i++)
        {
            GameObject go = Instantiate(TreasureChest_PO);
            treasureChests.Add(go);
        }

        SetTreasureChestPositions();
    }

    // Update is called once per frame
    void Update()
    {
        //If we are shuffling objects, change their positions
        if (bShuffling)
        {
            currentShuffleTime += Time.deltaTime;

            if (currentShuffleTime > shuffleTime)
            {
                go1.transform.position = go1End;
                go2.transform.position = go2End;
                bShuffling = false;
                return; //break out early here if we have finished our shuffle

            }

            float alpha = currentShuffleTime / shuffleTime;

            float go1X = Mathf.Lerp(go1Start.x, go1End.x, alpha);
            float go1Y = Mathf.Lerp(go1Start.y, go1End.y, alpha);
            float go1Z = Mathf.Lerp(go1Start.z, go1End.z, alpha);

            float go2X = Mathf.Lerp(go2Start.x, go2End.x, alpha);
            float go2Y = Mathf.Lerp(go2Start.y, go2End.y, alpha);
            float go2Z = Mathf.Lerp(go2Start.z, go2End.z, alpha);

            go1.transform.position = new Vector3(go1X, go1Y, go1Z);
            go2.transform.position = new Vector3(go2X, go2Y, go2Z);
        }
    }

    void BeginShuffle()
    {
        Random rand = new Random();

        go1 = null;
        go2 = null;

        List<GameObject> treasureChestListCopy = treasureChests;

        int randomChestIndex1 = Random.Range(0, treasureChestListCopy.Count - 1);
        go1 = treasureChestListCopy[randomChestIndex1];
        treasureChestListCopy.RemoveAt(randomChestIndex1); //remove so we don't get it again for the second box

        int randomChestIndex2 = Random.Range(0, treasureChestListCopy.Count - 1);
        go2 = treasureChestListCopy[randomChestIndex2];

        //Set the game objects' start and end positions
        go1Start = go1.transform.position;
        go1End = go2.transform.position;

        go2Start = go2.transform.position;
        go2End = go1.transform.position;

        bShuffling = true; //begin shuffle with the bool
    }

    void SetTreasureChestPositions()
    {
        float startingDifference = ((TreasureChestSpacing * (treasureChests.Count - 1)) / 2.0f) * -1.0f;
        for (int i = 0; i < treasureChests.Count; i++)
        {
            float newXPosition = startingDifference + (i * TreasureChestSpacing);
            treasureChests[i].transform.position = new Vector3(newXPosition, 0.5f, 0.0f);
        }
    }
}
