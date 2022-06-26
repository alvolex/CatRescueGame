using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer catImage;

    public CallWithTimer CallThatIsConnectedToThis { get; set; }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent(out CatCopCar copCar) && !copCar.bIsHelpingSomeone)
        {
            copCar.IsHelping();
            copCar.CurrentCall = CallThatIsConnectedToThis;
            copCar.bIsHelpingSomeone = true;
            copCar.agent.ResetPath();

            CallThatIsConnectedToThis.StopTimer(); //Make sure that the UI timer on the left side of the screen stops when the car hits the event area
            
            Destroy(gameObject);
        }
    }

    public void DestroyAfterTime(float time)
    {
        StartCoroutine(DestroyAfterSeconds(time));
    }

    IEnumerator DestroyAfterSeconds(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        
        Destroy(gameObject);
    }
    
    public void SetCatImage(/*Color color*/ Sprite sprite)
    {
        if (catImage != null)
        {
            /*catImage.color = color; */
            catImage.sprite = sprite; 
        }
    }
}
