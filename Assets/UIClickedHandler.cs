using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIClickedHandler : MonoBehaviour
{
    [SerializeField] private CallWithTimer callWithTimer;
    [SerializeField] private Canvas callsCanvas;

    //Events
    public delegate void UIEvent(float time, CallWithTimer newCall);
    public UIEvent OnCallAnswered;

    public void AnswerCall()
    {
        float timeToCompleteMission = Random.Range(0f, 20f);

        //Create UI Element for the call
        CallWithTimer newCall = Instantiate(callWithTimer, callsCanvas.transform, false);
        newCall.HandleTimer(timeToCompleteMission);
        
        OnCallAnswered.Invoke(timeToCompleteMission, newCall);
    }
}
