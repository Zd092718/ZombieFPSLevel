using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] zombiePrefabs;
    [SerializeField] private int spawnCount;
    [SerializeField] private float spawnRadius;
    [SerializeField] private bool spawnOnStart = true;

    // Start is called before the first frame update
    void Start()
    {
        if (spawnOnStart)
        {
            SpawnAll();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spawnOnStart && other.gameObject.CompareTag("Player"))
        {
            SpawnAll();
        }
    }

    private void SpawnAll()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * spawnRadius;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 10.0f, NavMesh.AllAreas))
            {
                int randomZombie = Random.Range(0, zombiePrefabs.Length);
                Instantiate(zombiePrefabs[randomZombie], hit.position, Quaternion.identity);
            }
            else
            {
                i--;
            }

        }
    }
}
