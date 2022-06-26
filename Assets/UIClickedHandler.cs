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
    [SerializeField] private HandleCatCallingText callTextCanvas;

    Queue<CatCall> CallsQueue = new Queue<CatCall>();

    [SerializeField] private float minTimeBetweenCalls = 2f;
    [SerializeField] private float maxTimeBetweenCalls = 10f;
    
    [SerializeField] private float minTimeToFinishMission = 10f;
    [SerializeField] private float maxTimeToFinishMission = 35f;

    [SerializeField] private List<Sprite> catSprites;
    
    public delegate void LostPointsDelegate();
    public LostPointsDelegate OnLostCustomer;

    struct CatCall
    {
        /*public Color Color;*/
        public Sprite CatSprite;
        public float TimeToCompleteMission;
        public GameObject objectContainingImage;
    }

    //Events
    public delegate void UIEvent(float time, CallWithTimer newCall, /*Color catColor*/ Sprite catSprite);
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

        /*var catColor = Random.ColorHSV();
        float timeToCompleteMission = Random.Range(10f, 20f);
        
        newCall.SetCatImage(catColor);
        newCall.HandleTimer(timeToCompleteMission);

        OnCallAnswered.Invoke(timeToCompleteMission, newCall, catColor);*/
        
        /*newCall.SetCatImage(catInfo.Color);*/
        newCall.SetCatImage(catInfo.CatSprite);
        newCall.HandleTimer(catInfo.TimeToCompleteMission, OnLostCustomer);

        OnCallAnswered.Invoke(catInfo.TimeToCompleteMission, newCall, catInfo.CatSprite);
        recievingCallCanvas.gameObject.SetActive(false);
        
        //Show text for the call
        callTextCanvas.ShowCatSpeech(catInfo.CatSprite, catInfo.TimeToCompleteMission);

        Destroy(catInfo.objectContainingImage);

        //Get next call straight away if there is one
        if (CallsQueue.Count > 0)
        {
            recievingCallCanvas.gameObject.SetActive(true);
        }
    }

    public void StopRecievingCalls()
    {
        StopAllCoroutines();
    }

    IEnumerator RecieveCall()
    {
        float timeBetweenCalls = Random.Range(minTimeBetweenCalls, maxTimeBetweenCalls);
        
        yield return new WaitForSeconds(timeBetweenCalls);
        recievingCallCanvas.gameObject.SetActive(true);

        //Create the cat
        GameObject incomingCatCallImage = Instantiate(incomingCallPrefab, callsQueueCanvas.transform, false);
        
        CatCall catCaller = new CatCall();
        /*catCaller.Color = Random.ColorHSV();*/
        catCaller.CatSprite = catSprites[Random.Range(0, catSprites.Count)];
        catCaller.TimeToCompleteMission = Random.Range(minTimeToFinishMission, maxTimeToFinishMission);
        catCaller.objectContainingImage = incomingCatCallImage;

        /*incomingCatCallImage.GetComponent<Image>().color = catCaller.Color;*/
        incomingCatCallImage.GetComponent<Image>().sprite = catCaller.CatSprite;

        CallsQueue.Enqueue(catCaller);

        if (CallsQueue.Count > 3)
        {
            var cat = CallsQueue.Dequeue();
            Destroy(cat.objectContainingImage);
            OnLostCustomer?.Invoke();
        }

        StartCoroutine(RecieveCall());
    }
}
