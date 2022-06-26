using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CallWithTimer : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image timerImage;

    public void StopTimer()
    {
        StopAllCoroutines();
    }
    
    public void HandleTimer(float timeToCompleteMission)
    {
        StartCoroutine(UpdateTimerSlider(timeToCompleteMission));
    }

    IEnumerator UpdateTimerSlider(float timeToCompleteMission)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToCompleteMission)
        {
            timeElapsed += Time.deltaTime;
            
            timerImage.fillAmount = Mathf.Lerp(1, 0, timeElapsed/timeToCompleteMission);
            timerImage.color = Color.Lerp(new Color(0f, 1f, 0.03f), Color.red, timeElapsed/timeToCompleteMission);

            yield return null;
        }
        Destroy(gameObject);
    }
}
