using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInfo : MonoBehaviour
{
    private TextMeshProUGUI infoText;

    void Awake()
    {
        infoText = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string msg)
    {
        infoText.text = msg;
    }
}
