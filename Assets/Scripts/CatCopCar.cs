using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CatCopCar : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 targetDest;

    private bool bIsMovingToTarget = false;
    private bool bIsPatrolling = false;
    private bool bDrawPath = false;

    private LineRenderer line;

    private int framCounter = 0;
    [SerializeField] private Vector3 hospitalPosition = Vector3.zero;

    [SerializeField]private List<Vector3> RandomPositionsToPatrolTo;
    

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ShouldStartPatrolling();
        ShouldReturnAfterPatrol();

        if (bDrawPath)
        {
            DrawPath();
        }
    }

    private void DrawPath()
    {
        if (agent.path != null)
        {
            line.SetPosition(0, agent.path.corners[0]);
            line.positionCount = agent.path.corners.Length;
            
            for (int i = 1; i < agent.path.corners.Length; i++)
            {
                line.SetPosition(i, agent.path.corners[i]);
            }
        }
    }

    private void ShouldReturnAfterPatrol()
    {
        if (bIsPatrolling)
        {
            framCounter++;
            if (framCounter % 5 == 0)
            {
                framCounter = 0;
                ReturnToHospital();
            }
        }
    }

    private void ReturnToHospital()
    {
        float distance = Vector3.Distance(transform.position, targetDest);
        if (distance < 1)
        {
            bIsMovingToTarget = false;
            bIsPatrolling = false;

            agent.SetDestination(hospitalPosition);
        }
    }

    private void ShouldStartPatrolling()
    {
        if (bIsMovingToTarget)
        {
            framCounter++;

            if (framCounter % 5 == 0)
            {
                framCounter = 0;
                float distance = Vector3.Distance(transform.position, targetDest);
                if (distance < 1)
                {
                    bIsMovingToTarget = false;

                    int randomIndex = UnityEngine.Random.Range(0, RandomPositionsToPatrolTo.Count - 1);
                    agent.SetDestination(RandomPositionsToPatrolTo[randomIndex]);
                    targetDest = RandomPositionsToPatrolTo[randomIndex];
                    bIsPatrolling = true;
                    bDrawPath = true;
                }
            }
        }
    }

    public void MoveToPosition(Vector3 target)
    {
        if (!agent) { return; }
        
        agent.SetDestination(target);
        targetDest = target;

        bIsMovingToTarget = true;
        bDrawPath = true;
    }

    public void IsHelping()
    {
        StartCoroutine(HelpInjured());
    }

    IEnumerator HelpInjured()
    {
        bIsMovingToTarget = false;
        bIsPatrolling = false;
        float timeToWait = UnityEngine.Random.Range(0, 5f);
        yield return new WaitForSeconds(timeToWait);
        ReturnToHospital();
    }

}
