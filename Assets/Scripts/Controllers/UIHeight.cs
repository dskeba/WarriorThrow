
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

    public void UpdateHeight(int currentHeight, int totalHeight)
    {
        heightText.text = currentHeight.ToString() + "m / " + totalHeight.ToString() + "m";
    }
}
