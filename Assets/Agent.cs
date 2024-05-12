using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float FOVAngle = 120;
    float checkSize = 10;
    float additionalSpeed = 0;
    float speed;
    float enemyDistance = 10;

    Spawn spawn;
    Vector3 nextPos;
    Vector3 egoVector;
    [SerializeField] List<Agent> neighborAgent = new();

    public void Init(Spawn _spawn, float _speed){
        spawn = _spawn;
        speed = _speed;

        StartCoroutine(RandomValue());
    }

    void Update()
    {
        if (additionalSpeed > 0)
            additionalSpeed -= Time.deltaTime;

        FindAgent();

        Vector3 center = Cohesion() * spawn.cohesionWeight;
        Vector3 alignment = Alignment() * spawn.alignmentWeight;
        Vector3 separation = Separation() * spawn.separationWeight;
        Vector3 bounds = Bounds() * spawn.boundsWeight;
        Vector3 obstacle = Obstacle() * spawn.obstacleWeight;
        Vector3 egoVec = egoVector * spawn.egoWeight;


        nextPos = center + alignment + separation + bounds + egoVec + obstacle;

        nextPos = Vector3.Lerp(transform.forward, nextPos, Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(nextPos);
        transform.position += (speed + additionalSpeed) * Time.deltaTime * nextPos;
    }

    void FindAgent()
    {
        neighborAgent.Clear();

        Collider[] neighbor = Physics.OverlapSphere(transform.position, checkSize, unitLayer);
        for (int i = 0; i < neighbor.Length; i++)
        {
            if (Vector3.Angle(transform.forward, neighbor[i].transform.position - transform.position) <= FOVAngle)
                neighborAgent.Add(neighbor[i].GetComponent<Agent>());
            if(i > spawn.maxNeighbourCount) 
                break;
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
        center.Normalize();
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
        alignment.Normalize();
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
        separation.Normalize();
        return separation;
    }

    Vector3 Bounds()
    {
        Vector3 offsetToCenter = spawn.mid.transform.position - transform.position;
        return offsetToCenter.magnitude >= spawn.spawnSize ? offsetToCenter.normalized : Vector3.zero;
    }

    private Vector3 Obstacle()
    {
        Vector3 obstacleVec = Vector3.zero;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit, enemyDistance, enemyLayer))
        {
            Debug.DrawLine(transform.position, hit.point, Color.black);
            obstacleVec = hit.normal;
            additionalSpeed = 10;
        }
        return obstacleVec;
    }

    IEnumerator RandomValue()
    {
        speed = Random.Range(spawn.speed.x, spawn.speed.y);
        egoVector = Random.insideUnitSphere;
        yield return new WaitForSeconds(Random.Range(1, 3f));
        StartCoroutine(RandomValue());
    }
}
