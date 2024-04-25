using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] Agent agent;
    [SerializeField] int count;
    [SerializeField] float spawnSize;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 ranPos = Random.insideUnitSphere;
            ranPos *= spawnSize;

            Quaternion ranRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(agent, ranPos, ranRot, this.transform);
        }
    }
}
