using System.Collections.Generic;
using UnityEngine;

public class MobSpawnController : MonoBehaviour
{
    public int mobLimit;
    public Transform[] spawnPoints;
    public GameObject[] npcPrefabs;

    List<GameObject> npcList = new List<GameObject>();


    private void Start()
    {
        // Initial spawns
        for (int i = 0;i < Random.Range(5, mobLimit);i++)
        {
            var origin = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var destination = Random.value > .5 ? spawnPoints[Random.Range(0, spawnPoints.Length)].transform : origin;
            GameObject npc = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            var go = Instantiate(npc, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            go.GetComponent<AIController>().Initialize(this, destination, origin);
            npcList.Add(go);
        }

        Invoke(nameof(TrySpawnNPC), Random.Range(4, 8));
    }

    private void TrySpawnNPC()
    {
        if(npcList.Count < mobLimit)
        {
            var origin = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var destination = Random.value > .5 ? spawnPoints[Random.Range(0, spawnPoints.Length)].transform : origin;
            GameObject npc = npcPrefabs[Random.Range(0, npcPrefabs.Length)];
            var go = Instantiate(npc, spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position, Quaternion.identity);
            go.GetComponent<AIController>().Initialize(this, destination, origin);
            npcList.Add(go);
        }

        Invoke(nameof(TrySpawnNPC), Random.Range(2, 3));
    }

    public void PopNPC(GameObject npc)
    {
        npcList.Remove(npc);
    }
    
    public void EndFunction()
    {
        CancelInvoke(nameof(TrySpawnNPC));
        for (int i = 0; i < npcList.Count; i++)
        {
            Destroy(npcList[i]);
        }
    }
}
