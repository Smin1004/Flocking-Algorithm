using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float cohesionSize;
    [SerializeField] float separationSize;
    [SerializeField] LayerMask layer;
    [SerializeField] float speed;

    List<Agent> neighborAgent = new();
    Vector3 nextPos;

    void Start()
    {

    }

    void Update()
    {
        FindAgent();

        Vector3 center = Cohesion();
        Vector3 alignment = Alignment();
        Vector3 separation = Separation();


        nextPos = center + alignment + separation;

        nextPos = Vector3.Lerp(transform.forward, center, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(nextPos);
        transform.position += speed * Time.deltaTime * nextPos;
    }

    void FindAgent()
    {
        neighborAgent.Clear();

        Collider[] neighbor = Physics.OverlapSphere(transform.position, cohesionSize, layer);
        for (int i = 0; i < neighbor.Length; i++)
        {
            neighborAgent.Add(neighbor[i].GetComponent<Agent>());
        }
    }

    Vector3 Cohesion()
    {
        Vector3 center = Vector3.zero;
        if(neighborAgent.Count < 1) return center;

        for (int i = 0; i < neighborAgent.Count; i++)
        {
            center += neighborAgent[i].transform.position;
        }

        center /= neighborAgent.Count;
        center -= transform.position;
        return center;
    }

    Vector3 Alignment()
    {
        Vector3 alignment = Vector3.zero;
        if(neighborAgent.Count < 1) return alignment;

        for (int i = 0; i < neighborAgent.Count; i++)
        {
            alignment += neighborAgent[i].transform.forward;
        }

        alignment /= neighborAgent.Count;
        return alignment;
    }

    Vector3 Separation()
    {
        Vector3 separation = Vector3.zero;
        if(neighborAgent.Count < 1) return separation;

        for (int i = 0; i < neighborAgent.Count; i++)
        {
            separation += transform.position - neighborAgent[i].transform.position;
        }

        separation /= neighborAgent.Count;
        return separation;
    }
}
