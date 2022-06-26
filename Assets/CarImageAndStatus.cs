using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarImageAndStatus : MonoBehaviour
{
    private CatCopCar CarRef;

    [SerializeField] private Image carImage;
    
    [SerializeField] private Image highlightImage;
    
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

    public void SetCarRef(CatCopCar car)
    {
        CarRef = car;
        car.OnCarSelected += HighlightImage;
    }

    private void HighlightImage()
    {
        highlightImage.gameObject.SetActive(!highlightImage.gameObject.activeSelf);
    }
}
