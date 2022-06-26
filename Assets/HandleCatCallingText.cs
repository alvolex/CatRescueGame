using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandleCatCallingText : MonoBehaviour
{
    [SerializeField] private float timeUntilAutoHide = 5f;

    [SerializeField] private Image catImage;
    [SerializeField] private TextMeshProUGUI catName;
    [SerializeField] private TextMeshProUGUI catText;

    [SerializeField] private List<string> Names = new List<string>();
    [SerializeField] private List<string> LowPrioStrings = new List<string>();
    [SerializeField] private List<string> MediumPrioStrings = new List<string>();
    [SerializeField] private List<string> HighPrioStrings = new List<string>();

    [SerializeField] private float missionTimeToshowLowPrio = 30f;
    [SerializeField] private float missionTimeToshowMediumPrio = 15f;

    [SerializeField] private CanvasRenderer managerCanvas;

    public void ShowCatSpeech(Sprite catInfoCatSprite, float catInfoTimeToCompleteMission)
    {
        gameObject.SetActive(true);
        
        catImage.sprite = catInfoCatSprite;
        catName.text = Names[Random.Range(0, Names.Count)];

        if (catInfoTimeToCompleteMission > missionTimeToshowLowPrio)
        {
            catText.text = LowPrioStrings[Random.Range(0, LowPrioStrings.Count)];
            
        }
        else if (catInfoTimeToCompleteMission > missionTimeToshowMediumPrio)
        {
            catText.text = MediumPrioStrings[Random.Range(0, MediumPrioStrings.Count)];
        }
        else
        {
            catText.text = HighPrioStrings[Random.Range(0, HighPrioStrings.Count)];
            managerCanvas.gameObject.SetActive(true);
            StartCoroutine(HideManagerAfterTime());
        }

        StartCoroutine(HideCustomerAfterTime());
    }

    IEnumerator HideCustomerAfterTime()
    {
        yield return new WaitForSeconds(timeUntilAutoHide);
        
        gameObject.SetActive(false);
    }
    
    IEnumerator HideManagerAfterTime()
    {
        yield return new WaitForSeconds(timeUntilAutoHide/3f);
        
        managerCanvas.gameObject.SetActive(false);
    }
}
