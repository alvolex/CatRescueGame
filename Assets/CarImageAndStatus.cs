using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarImageAndStatus : MonoBehaviour
{
    private CatCopCar CarRef;

    [SerializeField] private Image carImage;
    
    [SerializeField] private Image highlightImage;
    [SerializeField] private Image carAction;

    [SerializeField] private Sprite movingToTargetSprite;
    [SerializeField] private Sprite isHelpingSomeoneSprite;
    
    
    public delegate void CarImageClickedDelegate(CatCopCar car);
    public CarImageClickedDelegate OnCarImageClicked; 

    public void SetCarSprite(Sprite sprite /*Color color*/)
    {
        carImage.sprite = sprite;
        /*carImage.color = color;*/
    }

    public void CarImageClicked()
    {
        OnCarImageClicked.Invoke(CarRef);
    }

    public void SetCarRef(CatCopCar car)
    {
        CarRef = car;
        car.OnCarSelected += HighlightImage;
        car.OnActionChanged += UpdateActionStatus;
    }

    private void UpdateActionStatus(CarActions currentaction)
    {
        switch (currentaction)
        {
            case CarActions.MovingToTarget:
                /*carAction.color = Color.green;*/
                carAction.enabled = true;
                carAction.sprite = movingToTargetSprite;
                break;
            case CarActions.IsHelpingSomeone:
                /*carAction.color = Color.red;*/
                carAction.enabled = true;
                carAction.sprite = isHelpingSomeoneSprite;
                break;
            case CarActions.Idle:
                /*carAction.color = Color.white;*/
                carAction.sprite = null;
                carAction.enabled = false;
                break;
            default:
                break;
        }
    }

    private void HighlightImage(bool bIsHighlighted)
    {
        highlightImage.gameObject.SetActive(bIsHighlighted);
    }
}
