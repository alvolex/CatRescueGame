using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarImageAndStatus : MonoBehaviour
{
    public CatCopCar CarRef { get; set; }

    [SerializeField] private Image carImage;
    
    public delegate void CarImageClickedDelegate(CatCopCar car);

    public CarImageClickedDelegate OnCarImageClicked; 

    public void SetCarSprite(/*Sprite sprite*/ Color color)
    {
        /*carImage.sprite = sprite;*/
        carImage.color = color;
    }

    public void CarImageClicked()
    {
        OnCarImageClicked.Invoke(CarRef);
    }
}
