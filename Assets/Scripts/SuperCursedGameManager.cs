using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.EventSystems;

public class SuperCursedGameManager : MonoBehaviour
{
    [SerializeField] private WorldMap WorldMap;
    [SerializeField] private CatCopCar CatCopCar;

    [SerializeField] private List<GameObject> EventPositions;
    [SerializeField] private CallEvent callEvent; //Not actually an event, it's the visual representation of where we need to pick someone up

    [SerializeField] private UIClickedHandler uiClickedHandler;
    [SerializeField] private Manager managerCanvas;

    [Header("Car stuff")]
    [SerializeField] private int amountOfCars = 5;
    [SerializeField] private CatCopCar CurrentlySelectedCopCar; //Only serialized for debugging
    
    [SerializeField] List<Sprite> carSprites;

    //For the car selection menu
    [SerializeField] private Canvas carCanvas;
    [SerializeField] private CarImageAndStatus carImagePrefab;

    [SerializeField] private int score = 0;
    [SerializeField] private int customersDelivered = 0;

    private Camera mainCamera;

    void Start()
    {
        Instantiate(WorldMap, new Vector3(3,0,0), Quaternion.identity);
        CreateCars();

        mainCamera = Camera.main;
        
        //Subscribe to events
        uiClickedHandler.OnCallAnswered += OnCallAnswered;
        uiClickedHandler.OnLostCustomer += OnLostPoints;
    }

    private void CreateCars()
    {
        for (int i = 0; i < amountOfCars; i++)
        {
            var car = Instantiate(CatCopCar, Vector3.zero, Quaternion.identity);
            car.SetCarSprite(carSprites[i]);
            car.OnGainedPoints += OnGainedPoints;
            
            var carImage = Instantiate(carImagePrefab, carCanvas.transform, false);
            carImage.SetCarRef(car);
            carImage.SetCarSprite(carSprites[i]/*car.GetCarSprite()*/);
            carImage.OnCarImageClicked += OnCarImageClicked;
        }
    }

    private void OnGainedPoints()
    {
        score++;
        customersDelivered++;
    }
    
    private void OnLostPoints()
    {
        score--;

        if (score < 0)
        {
            //Game over
            uiClickedHandler.StopRecievingCalls();
            managerCanvas.RestartText("You disappoint meow..!\n" + "You delivered " + customersDelivered + " customers..\n" + "(Press the checkmark if you'd like to try again.)");
        }
    }

    private void OnCarImageClicked(CatCopCar car)
    {
        car.SetIsHighlighted(true);
        
        if (CurrentlySelectedCopCar != null)
        {
            CurrentlySelectedCopCar.SetIsHighlighted(false);
        }
        
        CurrentlySelectedCopCar = car;
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
                    if (CurrentlySelectedCopCar )
                    {
                        CurrentlySelectedCopCar.SetIsHighlighted(false); 
                    }
                    
                    CurrentlySelectedCopCar = copCar;
                    CurrentlySelectedCopCar.SetIsHighlighted(true);
                }
                else if (hit.collider.TryGetComponent(out WorldMap worldMap))
                {
                    if (CurrentlySelectedCopCar != null)
                    {
                        CurrentlySelectedCopCar.MoveToPosition(hit.point);
                        CurrentlySelectedCopCar.SetIsHighlighted(false);
                        CurrentlySelectedCopCar = null;
                    }
                    else
                    {
                        if (CurrentlySelectedCopCar)
                        {
                            CurrentlySelectedCopCar.SetIsHighlighted(false);
                        }
                        CurrentlySelectedCopCar = null;
                    }
                }
            }
        }
    }
    
    private void OnCallAnswered(float time, CallWithTimer callWithTimer, /*Color catColor*/ Sprite catSprite)
    {
        int index = Random.Range(0, EventPositions.Count);
        CallEvent eventInWorld = Instantiate(callEvent, EventPositions[index].transform.position, Quaternion.identity);
        eventInWorld.SetCatImage(/*catColor*/catSprite);
        eventInWorld.CallThatIsConnectedToThis = callWithTimer;
        eventInWorld.DestroyAfterTime(time);
    }
}
