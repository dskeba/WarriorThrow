
using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float seconds;

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public void ResetTime()
    {
        seconds = 0;
    }

    private void FixedUpdate()
    {
        seconds += Time.deltaTime;
        int intSeconds = (int)seconds;
        timerText.text = intSeconds.ToString() + " secs";
    }

}