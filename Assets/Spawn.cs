using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] Agent agent;
    public Transform mid;
    [SerializeField] int count;
    public float spawnSize;

    [Header("Value")]
    public Vector2 speed;

    public float cohesionWeight;
    public float alignmentWeight;
    public float separationWeight;
    public float boundsWeight;
    public float egoWeight;

    public int maxNeighbourCount;

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 ranPos = Random.insideUnitSphere;
            ranPos *= spawnSize;

            Quaternion ranRot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Agent temp = Instantiate(agent, ranPos, ranRot, this.transform);
            temp.Init(this, Random.Range(speed.x, speed.y));
        }
    }
}
