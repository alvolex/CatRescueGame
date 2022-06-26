using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIClickedHandler : MonoBehaviour
{
    
    [SerializeField] private CallWithTimer callWithTimerPrefab;
    [SerializeField] private GameObject incomingCallPrefab;

    [SerializeField] private Canvas callsCanvas;
    [SerializeField] private Canvas recievingCallCanvas;
    [SerializeField] private Canvas callsQueueCanvas;
    
    Queue<CatCall> CallsQueue = new Queue<CatCall>();

    [SerializeField] private float minTimeBetweenCalls = 2f;
    [SerializeField] private float maxTimeBetweenCalls = 10f;

    struct CatCall
    {
        public Color Color;
        public float TimeToCompleteMission;
        public GameObject objectContainingImage;
    }

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
        //Create UI Element for the call
        CallWithTimer newCall = Instantiate(callWithTimerPrefab, callsCanvas.transform, false);
        var catInfo = CallsQueue.Dequeue();
        
        //Random color, this will later be a random cat image todo change this to be a random cat image
        /*var catColor = Random.ColorHSV();
        float timeToCompleteMission = Random.Range(10f, 20f);
        
        newCall.SetCatImage(catColor);
        newCall.HandleTimer(timeToCompleteMission);

        OnCallAnswered.Invoke(timeToCompleteMission, newCall, catColor);*/
        
        newCall.SetCatImage(catInfo.Color);
        newCall.HandleTimer(catInfo.TimeToCompleteMission);

        OnCallAnswered.Invoke(catInfo.TimeToCompleteMission, newCall, catInfo.Color);
        recievingCallCanvas.gameObject.SetActive(false);
        
        Destroy(catInfo.objectContainingImage);

        //Get next call straight away if there is one
        if (CallsQueue.Count > 0)
        {
            recievingCallCanvas.gameObject.SetActive(true);
        }
    }


    IEnumerator RecieveCall()
    {
        float timeBetweenCalls = Random.Range(minTimeBetweenCalls, maxTimeBetweenCalls);
        
        yield return new WaitForSeconds(timeBetweenCalls);
        recievingCallCanvas.gameObject.SetActive(true);

        //Create the cat
        GameObject incomingCatCallImage = Instantiate(incomingCallPrefab, callsQueueCanvas.transform, false);
        
        CatCall catCaller = new CatCall();
        catCaller.Color = Random.ColorHSV();
        catCaller.TimeToCompleteMission = Random.Range(10f, 20f);
        catCaller.objectContainingImage = incomingCatCallImage;

        incomingCatCallImage.GetComponent<Image>().color = catCaller.Color;

        CallsQueue.Enqueue(catCaller);

        if (CallsQueue.Count > 3)
        {
            var cat = CallsQueue.Dequeue();
            Destroy(cat.objectContainingImage);
        }

        StartCoroutine(RecieveCall());
    }
}
