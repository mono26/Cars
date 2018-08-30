using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject objectType;
    [SerializeField]
    private Transform[] spawnPoints;
    [SerializeField]
    private Transform[] spawnPositions;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private int objectsAmount;

	void Start () {

        StartCoroutine("SpawnObject");
	}

    private void PrepareSpawnPoints()
    {
        objectsAmount = Random.Range(2, 5);
        spawnPositions = new Transform[objectsAmount];
        int[] prevPoints = new int[objectsAmount];
        for(int i = 0; i < spawnPositions.Length; i++)
        {
            int spawnPoint = RandomBetweenRange(spawnPoints.Length);
            prevPoints[i] = spawnPoint;
            //Check prevPoints to compare, if is different, instantiate.
            for (int e = 0; e < prevPoints.Length; e++)
            {
                if(spawnPoint != prevPoints[e])
                {
                    spawnPositions[i] = spawnPoints[spawnPoint];
                }

            }

        }      
    }

    private int RandomBetweenRange(int range)
    {
        int number = Random.Range(0, range);
        return number;
    }

    private IEnumerator SpawnObject()
    {
        PrepareSpawnPoints();
        for(int i = 0; i < spawnPositions .Length; i++)
        {
            Instantiate(objectType, spawnPositions[i].position, spawnPoints[i].rotation);
        }
        yield return new WaitForSeconds(spawnTime);
    }

}
