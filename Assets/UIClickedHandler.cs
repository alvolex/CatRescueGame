using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class UIClickedHandler : MonoBehaviour
{
    
    [SerializeField] private CallWithTimer callWithTimerPrefab;
    [SerializeField] private Canvas callsCanvas;
    [SerializeField] private Canvas recievingCallCanvas;
    [SerializeField] private Canvas callsQueueCanvas;
    
    Queue<CallWithTimer> CallsQueue = new Queue<CallWithTimer>();

    [SerializeField] private float minTimeBetweenCalls = 2f;
    [SerializeField] private float maxTimeBetweenCalls = 10f;
    

    //Events
    public delegate void UIEvent(float time, CallWithTimer newCall, Color catColor);
    public UIEvent OnCallAnswered;
    
    private void Start()
    {
        recievingCallCanvas.gameObject.SetActive(false);
        StartCoroutine(RecieveCall());
    }

    public void AnswerCall()
    {
        float timeToCompleteMission = Random.Range(10f, 20f);

        //Create UI Element for the call
        CallWithTimer newCall = Instantiate(callWithTimerPrefab, callsCanvas.transform, false);
        
        //Random color, this will later be a random cat image todo fix this
        var catColor = Random.ColorHSV();
        
        newCall.SetCatImage(catColor);
        newCall.HandleTimer(timeToCompleteMission);

        OnCallAnswered.Invoke(timeToCompleteMission, newCall, catColor);
        recievingCallCanvas.gameObject.SetActive(false);
    }


    IEnumerator RecieveCall()
    {
        float timeBetweenCalls = Random.Range(minTimeBetweenCalls, maxTimeBetweenCalls);
        
        yield return new WaitForSeconds(timeBetweenCalls);
        recievingCallCanvas.gameObject.SetActive(true);
        
        StartCoroutine(RecieveCall());
    }
}
