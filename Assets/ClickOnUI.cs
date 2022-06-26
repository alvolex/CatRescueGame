using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickOnUI : MonoBehaviour, IPointerDownHandler
{
    UIClickedHandler uiClickedHandler;
    private void Start()
    {
        uiClickedHandler = GetComponent<UIClickedHandler>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click" + eventData.pointerCurrentRaycast.gameObject.name);
            HandleClick(eventData.pointerCurrentRaycast.gameObject);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
        }
    }

    private void HandleClick(GameObject clickedObj)
    {
        switch (clickedObj.tag)
        {
            case "AnswerButton":
                uiClickedHandler.AnswerCall();
                break;
            default:
                break;
        }
    }
}
