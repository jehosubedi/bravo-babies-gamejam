using System.Collections.Generic;
using UnityEngine;

public class MobSpawnController : MonoBehaviour
{
    public int mobLimit;
    public GameObject mobPrefab;
    public Transform[] spawnPoints;
    
    List<GameObject> npcList = new List<GameObject>();

    private void Start()
    {
        // Initial spawns
        for (int i = 0;i < Random.Range(5, mobLimit);i++)
        {
            // SPAWN NPC HERE
            // ADD NPC TO NPC COUNT
        }

        Invoke(nameof(TrySpawnNPC), Random.Range(3, 5));
    }

    private void TrySpawnNPC()
    {
        // Spawn NPC at random based on the spawnpoints array
        // Do recursive call to try and respawn a new NPC
    }
}
