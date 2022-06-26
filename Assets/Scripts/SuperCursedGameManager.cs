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
    
    [SerializeField] private CatCopCar CurrentlySelectedCopCar; //Only serialized for debugging

    [SerializeField] private List<GameObject> EventPositions;
    [SerializeField] private CallEvent callEvent;

    [SerializeField] private UIClickedHandler uiClickedHandler;

    [SerializeField] private int amountOfCars = 5;
    
    //For the car selection menu
    [SerializeField] private Canvas carCanvas;
    [SerializeField] private CarImageAndStatus carImagePrefab;

    private Camera mainCamera;

    void Start()
    {
        Instantiate(WorldMap, new Vector3(3,0,0), Quaternion.identity);
        CreateCars();

        mainCamera = Camera.main;
        
        //Subscribe to events
        uiClickedHandler.OnCallAnswered += OnCallAnswered;
    }

    private void CreateCars()
    {
        for (int i = 0; i < amountOfCars; i++)
        {
            var car = Instantiate(CatCopCar, Vector3.zero, Quaternion.identity);
            car.SetCarSprite(Random.ColorHSV());
            
            var carImage = Instantiate(carImagePrefab, carCanvas.transform, false);
            carImage.CarRef = car;
            carImage.SetCarSprite(car.GetCarSprite());
            carImage.OnCarImageClicked += OnCarImageClicked;
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
    
    private void OnCallAnswered(float time, CallWithTimer callWithTimer, Color catColor)
    {
        int index = Random.Range(0, EventPositions.Count);
        CallEvent eventInWorld = Instantiate(callEvent, EventPositions[index].transform.position, Quaternion.identity);
        eventInWorld.SetCatImage(catColor);
        eventInWorld.CallThatIsConnectedToThis = callWithTimer;
        eventInWorld.DestroyAfterTime(time);
    }
}
