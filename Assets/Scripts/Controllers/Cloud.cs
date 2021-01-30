
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;

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
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x < -30 || transform.position.x > 30)
        {
            ResetPosition();
        }
    }

    private void ResetPosition()
    {
        Vector3 currentPos = transform.position;
        currentPos.x = -currentPos.x;
        transform.position = currentPos;
    }
}
