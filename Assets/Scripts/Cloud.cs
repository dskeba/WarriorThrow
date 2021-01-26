
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    private Vector3 originalPos;

    public void Awake()
    {
        if (transform.position.x < 0)
        {
            speed = 2f;
        }
        else
        {
            speed = -2f;
        }
        originalPos = transform.position;
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x < -20 || transform.position.x > 20)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        transform.position = originalPos;
    }
}
