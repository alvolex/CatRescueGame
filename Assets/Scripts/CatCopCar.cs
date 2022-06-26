using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum CarActions
{
    MovingToTarget,
    IsHelpingSomeone,
    Idle
}


public class CatCopCar : MonoBehaviour
{
    public NavMeshAgent agent;
    private Vector3 targetDest;

    private bool bIsMovingToTarget = false;
    private bool bIsPatrolling = false;
    private bool bDrawPath = false;
    private bool bIsReturningToHospital = false;

    private LineRenderer line;

    private int framCounter = 0;
    [SerializeField] private Vector3 hospitalPosition = Vector3.zero;

    [SerializeField]private List<Vector3> RandomPositionsToPatrolTo;

    [SerializeField] private SpriteRenderer carSprite;
    [SerializeField] private SpriteRenderer highlighSprite;
    

    public CallWithTimer CurrentCall { get; set; }
    public bool bIsHelpingSomeone { get; set; } = false;
    
    //create event without parameters
    public delegate void CarSelectedDelegate(bool bIsHighlighted);
    public CarSelectedDelegate OnCarSelected; 
    
    public delegate void CarActionChanged(CarActions currentAction);
    public CarActionChanged OnActionChanged; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        ShouldStartPatrolling();
        ShouldReturnAfterPatrol();
        ShouldReturnToHospital();

        if (bDrawPath)
        {
            DrawPath();
        }
    }

    private void ShouldReturnToHospital()
    {
        if (bIsReturningToHospital /*|| bIsHelpingSomeone*/)
        {
            bIsMovingToTarget = false;
            bIsPatrolling = false;
            
            targetDest = hospitalPosition;
            agent.SetDestination(targetDest);
            float distance = Vector3.Distance(transform.position, targetDest);
            if (distance < 1)
            {
                bIsMovingToTarget = false;
                bIsPatrolling = false;
                bIsReturningToHospital = false;
                bIsHelpingSomeone = false;
                OnActionChanged?.Invoke(CarActions.Idle);

                if (CurrentCall != null)
                {
                    Destroy(CurrentCall.gameObject);
                }
            }
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
        if (bIsPatrolling/* || bIsHelpingSomeone*/)
        {
            bIsMovingToTarget = false;
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
            if (!bIsHelpingSomeone)
            {
                OnActionChanged?.Invoke(CarActions.Idle);
            }
            
            bIsMovingToTarget = false;
            bIsPatrolling = false;
            
            agent.SetDestination(hospitalPosition);
            targetDest = hospitalPosition;
            bIsReturningToHospital = true;
        }
    }

    private void ShouldStartPatrolling()
    {
        if (bIsMovingToTarget)
        {
            OnActionChanged?.Invoke(CarActions.MovingToTarget);
            
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
        OnActionChanged?.Invoke(CarActions.IsHelpingSomeone);
        bIsMovingToTarget = false;
        bIsPatrolling = false;
        float timeToWait = UnityEngine.Random.Range(0, 1.5f);
        yield return new WaitForSeconds(timeToWait);
        bIsReturningToHospital = true;
        ReturnToHospital();
    }
    
    public void SetCarSprite(Sprite sprite /*Color color*/)
    {
        carSprite.sprite = sprite;
        //GetComponent<SpriteRenderer>().sprite = sprite;
        
        /*GetComponentInChildren<SpriteRenderer>().color = color;*/
    }
    
    public Color GetCarSprite()
    {
        /*GetComponent<SpriteRenderer>().sprite = sprite;*/
        
        return GetComponentInChildren<SpriteRenderer>().color;
    }

    public void SetIsHighlighted(bool bIsHighlighted)
    {
        highlighSprite.gameObject.SetActive(bIsHighlighted);
        OnCarSelected.Invoke(bIsHighlighted);
    }
}