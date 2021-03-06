using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallWithTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;
    [SerializeField] private TextMeshProUGUI textTimer;

    [SerializeField] private Image catImage;

    
    public void StopTimer()
    {
        StopAllCoroutines();
    }
    
    public void HandleTimer(float timeToCompleteMission, UIClickedHandler.LostPointsDelegate lostPointsDelegate)
    {
        StartCoroutine(UpdateTimerSlider(timeToCompleteMission, lostPointsDelegate));
    }

    IEnumerator UpdateTimerSlider(float timeToCompleteMission, UIClickedHandler.LostPointsDelegate lostPointsDelegate)
    {
        float timeElapsed = 0;

        while (timeElapsed < timeToCompleteMission)
        {
            timeElapsed += Time.deltaTime;

            textTimer.text = /*"00:" + */(timeToCompleteMission - timeElapsed).ToString("00:00.00");
            
            timerImage.fillAmount = Mathf.Lerp(1, 0, timeElapsed/timeToCompleteMission);
            timerImage.color = Color.Lerp(new Color(0f, 1f, 0.03f), Color.red, timeElapsed/timeToCompleteMission);

            yield return null;
        }
        lostPointsDelegate?.Invoke();
        
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
