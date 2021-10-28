using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToSurvive;
    [SerializeField] TextMeshProUGUI timerText;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        timerText.text = "Time left: " + timeToSurvive;

        yield return new WaitForSeconds(1f);

        timeToSurvive -= 1;

        if (timeToSurvive < 0)
        {
            FindObjectOfType<ManagementSystem>().WinGame();
        }
        else
        {
            StartCoroutine(Countdown());
        }
    }
}
