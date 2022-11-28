using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : Monobehaviour
{
    public GameObject cubePrefab;

    void update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 randomSpawnPosition = new Vector3(Random.Range(-10,11), 5, Random.Range(-10,11));
            Instantiate(cubePrefab, randomSpawnPosition, Quaternion.identity);

        }
    }
}
