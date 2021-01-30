
using TMPro;
using UnityEngine;

public class UIStage : MonoBehaviour
{
    private TextMeshProUGUI stageText;
    private int currentStage = 1;

    void Awake()
    {
        stageText = GetComponent<TextMeshProUGUI>();
    }

    public void IncrementStage()
    {
        currentStage++;
        stageText.text = "Stage: " + currentStage.ToString();
    }

}
