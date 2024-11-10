using System.Collections.Generic;
using UnityEngine;

public class MobSpawnController : MonoBehaviour
{
    public int mobLimit;
    public GameObject mobPrefab;
    public Transform[] spawnPoints;
    public Sprite[] npcSprites;

    List<GameObject> npcList = new List<GameObject>();


    private void Start()
    {
        // Initial spawns
        for (int i = 0;i < Random.Range(5, mobLimit);i++)
        {
            var origin = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var destination = Random.value > .5 ? spawnPoints[Random.Range(0, spawnPoints.Length)].transform : null;
            var go = Instantiate(mobPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, Quaternion.identity);
            if (destination == origin && destination != null)
                destination = null;
            go.GetComponent<AIController>().Initialize(this, destination, origin, npcSprites[Random.Range(0,npcSprites.Length)]);
            npcList.Add(go);
        }

        Invoke(nameof(TrySpawnNPC), Random.Range(2, 3));
    }

    private void TrySpawnNPC()
    {
        if(npcList.Count < mobLimit)
        {
            var origin = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var destination = Random.value > .5 ? spawnPoints[Random.Range(0, spawnPoints.Length)].transform : null;
            var go = Instantiate(mobPrefab, spawnPoints[Random.Range(0,spawnPoints.Length)].transform.position, Quaternion.identity);
            if (destination == origin && destination != null)
                destination = null;
            go.GetComponent<AIController>().Initialize(this, destination, origin, npcSprites[Random.Range(0, npcSprites.Length)]);
            npcList.Add(go);
        }

        Invoke(nameof(TrySpawnNPC), Random.Range(2, 3));
    }

    public void PopNPC(GameObject npc)
    {
        npcList.Remove(npc);
    }
}
