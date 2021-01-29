
using TMPro;
using UnityEngine;

public class UIHeight : MonoBehaviour
{
    private TextMeshProUGUI heightText;
    private GameObject star;

    void Awake()
    {
        heightText = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateHeight(int currentHeight)
    {
        heightText.text = currentHeight.ToString() + "m / " + GetStarHeight() + "m";
    }

    private int GetStarHeight()
    {
        if (star == null)
        {
            star = GameObject.FindGameObjectWithTag("Star");
        }
        return (int)star.transform.position.y;
    }

}
