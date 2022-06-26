using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEvent : MonoBehaviour
{
    public CallWithTimer CallThatIsConnectedToThis { get; set; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CatCopCar copCar))
        {
            copCar.IsHelping();
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
}
