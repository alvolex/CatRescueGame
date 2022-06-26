using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperCursedGameManager : MonoBehaviour
{
    [SerializeField] private WorldMap WorldMap;
    [SerializeField] private CatCopCar CatCopCar;
    [SerializeField] private CatCopCar CurrentlySelectedCopCar;

    [SerializeField] private List<GameObject> EventPositions;
    [SerializeField] private CallEvent callEvent;

    [SerializeField] private UIClickedHandler uiClickedHandler;
    

    private Camera mainCamera;

    void Start()
    {
        Instantiate(WorldMap, Vector3.zero, Quaternion.identity);
        Instantiate(CatCopCar, Vector3.zero, Quaternion.identity);

        mainCamera = Camera.main;
        
        //Subscribe to events
        uiClickedHandler.OnCallAnswered += OnCallAnswered;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out CatCopCar copCar))
                {
                    CurrentlySelectedCopCar = copCar;
                }
                else if (hit.collider.TryGetComponent(out WorldMap worldMap))
                {
                    if (CurrentlySelectedCopCar != null)
                    {
                        CurrentlySelectedCopCar.MoveToPosition(hit.point);
                        CurrentlySelectedCopCar = null;
                    }
                    else
                    {
                        CurrentlySelectedCopCar = null;
                    }
                }
            }
        }
    }
    
    private void OnCallAnswered(float time, CallWithTimer callWithTimer, Color catColor)
    {
        int index = Random.Range(0, EventPositions.Count);
        CallEvent eventInWorld = Instantiate(callEvent, EventPositions[index].transform.position, Quaternion.identity);
        eventInWorld.SetCatImage(catColor);
        eventInWorld.CallThatIsConnectedToThis = callWithTimer;
        eventInWorld.DestroyAfterTime(time);
    }
}
