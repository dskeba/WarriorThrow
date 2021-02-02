
using TMPro;
using UnityEngine;

public class UIHeight : MonoBehaviour
{
    private TextMeshProUGUI heightText;
    private GameObject finalPlatform;

    void Awake()
    {
        heightText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHeight(int currentHeight)
    {
        heightText.text = currentHeight.ToString() + "m / " + GetFinalPlatformHeight() + "m";
    }

    private int GetFinalPlatformHeight()
    {
        if (finalPlatform == null)
        {
            finalPlatform = GameObject.FindGameObjectWithTag("FinalPlatform");
        }
        return (int)finalPlatform.transform.position.y;
    }

}
