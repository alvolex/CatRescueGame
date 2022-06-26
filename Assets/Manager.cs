using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private Button button;
    
    
    public void SetText(string text)
    {
        gameObject.SetActive(true);
        textBox.text = text;
    }
    
    public void RestartText(string text)
    {
        var rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.SetPositionAndRotation(new Vector3(500,200,0), Quaternion.identity);

        gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        button.onClick.AddListener(RestartGame);
        textBox.text = text;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }

}
