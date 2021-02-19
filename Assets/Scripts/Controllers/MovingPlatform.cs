using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    public bool move = false;
    private float minX;
    private float maxX;
    private float direction;
    private float speed = 0.05f;

    private void Awake()
    {
        minX = transform.position.x - 2;
        maxX = transform.position.x + 2;
        direction = Random.Range(0, 2) * 2 - 1;
    }

    private void Update()
    {
        if (!move)
        {
            return;
        }

        if (transform.position.x < minX)
        {
            direction = 1f;
        } else if (transform.position.x > maxX)
        {
            direction = -1f;
        }

        transform.position = (transform.position + transform.right * direction * speed * Time.fixedDeltaTime);
    }
}
