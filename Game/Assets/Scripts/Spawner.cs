using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    private List<SpawnPoint> SpawnPointList;
    private List<Character> SpawnedCharacters;

    private bool HasSpawned;

    public Collider m_collider;
    public UnityEvent OnAllSpawnedCharacterEliminated;

    private void Awake()
    {
        var SpawnPointArray = transform.parent.GetComponentsInChildren<SpawnPoint>();
        SpawnPointList = new List<SpawnPoint>(SpawnPointArray);
        SpawnedCharacters = new List<Character>();
    }

    private void Update()
    {
        if (!HasSpawned || SpawnedCharacters.Count == 0)
            return;

        bool AllSpawnedAreDead = true;

        foreach(Character c in SpawnedCharacters)
        {
            if(c.CurrentState != Character.CharacterState.Dead)
            {
                AllSpawnedAreDead = false;
                break;
            }
        }

        if(AllSpawnedAreDead)
        {
            if(OnAllSpawnedCharacterEliminated != null)
                OnAllSpawnedCharacterEliminated.Invoke();
            SpawnedCharacters.Clear();
        }
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
                GameObject SpawnedObject = Instantiate(point.EnemyToSpawn,point.transform.position,point.transform.rotation);
                SpawnedCharacters.Add(SpawnedObject.GetComponent<Character>());
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
