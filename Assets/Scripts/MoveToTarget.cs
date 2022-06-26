using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    [SerializeField] private GameObject CubeToSpawn;

    
    void OnMouseDown()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit))
        {
            NavMeshAgent NVA = CubeToSpawn.GetComponent<NavMeshAgent>();

            if (NVA)
            {
                NVA.SetDestination(hit.point);
            }

        }
    }
}
