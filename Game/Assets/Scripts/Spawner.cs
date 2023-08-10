using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> SpawnPointList;

    private bool HasSpawned;

    public Collider m_collider;

    private void Awake()
    {
        var SpawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        SpawnPointList = new List<SpawnPoint>(SpawnPointArray);
    }

    public void SpawnCharacter()
    {
        if (HasSpawned)
            return;

        HasSpawned = true;

        foreach(SpawnPoint point in SpawnPointList)
        {
            if(point.EnemyToSpawn != null)
            {
                GameObject SpawnedObject = Instantiate(point.EnemyToSpawn,point.transform.position,Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SpawnCharacter();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, m_collider.bounds.size);
    }
}
