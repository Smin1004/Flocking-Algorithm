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

    void Start()
    {

    }

    void Update()
    {
        Cohesion();

        Vector3 center = CheckCenterPos();

        center = Vector3.Lerp(transform.forward, center, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(center);
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    void Cohesion()
    {
        neighborAgent.Clear();

        Collider[] neighbor = Physics.OverlapSphere(transform.position, separationSize, layer);
        for (int i = 0; i < neighbor.Length; i++)
        {
            neighborAgent.Add(neighbor[i].GetComponent<Agent>());
        }
    }

    Vector3 CheckCenterPos()
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

    void Alignment()
    {

    }

    void Separation()
    {

    }
}
