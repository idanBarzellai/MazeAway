using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildLevel : MonoBehaviour
{
    private GameManager gm;
	private int nVertices;
	private BlockContorller[] blocks;
    private HashSet<GameObject> itemsOnMap = new HashSet<GameObject>();
    private HashSet<BlockContorller> occupiedBlocks = new HashSet<BlockContorller>();
    public GameObject magnifingGlass;
    public GameObject clockItem;
    public GameObject keyItem;
    public GameObject coinItem;
    public Transform[] starts;
    
    private void Start()
    {
        gm = GameManager.Instance;
    }

    public void StartBuildLevel()
    {
		this.nVertices = this.transform.childCount;
        blocks = new BlockContorller[nVertices];
		for (int i = 0; i < nVertices; i++)
        {
			blocks[i] = this.transform.GetChild(i).GetComponent<BlockContorller>();
        }
        BuildLevelFunc();
    }

    private bool DFS()
    {
        // Standart DFS algorithm
        // Used for checking if all blocks are reachable
        var visited = new HashSet<BlockContorller>();

        var stack = new Stack<BlockContorller>();
        stack.Push(blocks[0]);

        while (stack.Count > 0)
        {
            var vertex = stack.Pop();

            if (visited.Contains(vertex))
                continue;

            visited.Add(vertex);

            HashSet<BlockContorller> currNeighbors = new HashSet<BlockContorller>();
            foreach (Bridge bridge in vertex.bridges)
            {
                if (bridge.GetActive() && bridge.GetTheOtherBlock(vertex) != null)
                    currNeighbors.Add(bridge.GetTheOtherBlock(vertex));
            }
            foreach (BlockContorller b in currNeighbors)
            {
                if (!visited.Contains(b))
                    stack.Push(b);
            }
        }
        return visited.Count == nVertices;
    }

    public void BuildLevelFunc()
    {
        // Build the level by setting the blocks and checking with dfs is the "Graph" is connected
        do { SwitchAlgorithm(); } while (!DFS());
        SpawnStuff();
    }

    private void SwitchAlgorithm()
    {
        // Build the blocks and bridges
        ResetBlocks();
        for (int i = 0; i < blocks.Length; i += 2)
        {
            if (i != 0 && i % 6 == 0 && i<blocks.Length-1) i++;
            int falseChild = Random.Range(0, blocks[i].bridges.Length); 
            blocks[i].bridges[falseChild].SwitchActive();
        }
    }

    public void ResetAllLevel()
    {  
        ResetBlocks();
        CancelInvoke();
        BuildLevelFunc();
    }

    public void ResetBlocks()
    {
        // Reset blocks to normal state
        for (int i = 0; i < blocks.Length; i++)
        {
            for (int j = 0; j < blocks[i].bridges.Length; j++)
            {
                if(!blocks[i].bridges[j].GetActive())
                    blocks[i].bridges[j].SwitchActive();
            }
        }  
    }

    private void PickSpawnForPlayer(Transform[] transforms, GameObject go)
    {
        // Set the player and finish line by given gameobjects and random
        ResetSpawns(transforms);
        Transform t = transforms[Random.Range(0, transforms.Length - 1)];
        go.transform.position = t.position;
        occupiedBlocks.Add(t.GetComponent<WhereAmI>().myPlace);
        if (t.childCount > 0)
        {
            GameObject temp = t.GetChild(Random.Range(0, t.childCount - 1)).gameObject;
            temp.SetActive(true);
            occupiedBlocks.Add(temp.GetComponent<WhereAmI>().myPlace);
        }
    }

    private void SpawnStuff()
    {
        // Spawn items by current round, gets harder as the round gets bigger
        PickSpawnForPlayer(starts, gm.player.gameObject);
        PickSpawnForItems(keyItem);
        PickSpawnForItems(coinItem);
        if (gm.round < 5)
        {
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(clockItem);
            PickSpawnForItems(clockItem);
            PickSpawnForItems(clockItem);
            gm.SetTimeToFinish(90);
        }
        else if (gm.round >= 5 && gm.round < 10)
        {
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(clockItem);
            PickSpawnForItems(clockItem);
            gm.SetTimeToFinish(60);
        }
        else 
        {
            PickSpawnForItems(magnifingGlass);
            PickSpawnForItems(clockItem);
            PickSpawnForItems(clockItem);
            gm.SetTimeToFinish(40);
        }
        
    }

    private void PickSpawnForItems(GameObject go)
    {
        // Picks an object transform as t randomly and position the object there
        BlockContorller b;
        while (true)
        {
            b = blocks[Random.Range(0, blocks.Length - 1)];
            if (!occupiedBlocks.Contains(b))
                break;
        }
        GameObject temp = Instantiate(go);
        itemsOnMap.Add(temp);
        temp.transform.position = b.transform.position;
        occupiedBlocks.Add(b);

    }

    public void ResetSpawns(Transform[] transforms)
    {
        // Reset all the spawned objects for new level, so won't overload
        foreach (Transform t in transforms)
        {
            t.GetComponent<SpriteRenderer>().enabled = false;
            for (int i = 0; i < t.childCount; i++)
            {
                t.GetChild(i).gameObject.SetActive(false);
            }
        }
        occupiedBlocks.Clear();
        foreach (GameObject g in itemsOnMap)
        {
            Destroy(g);
        }
        itemsOnMap.Clear();
    }
}
