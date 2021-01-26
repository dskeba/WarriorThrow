using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 beginDragPos;
    private Vector2 endDragPos;
    private LineRenderer lr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    [SerializeField]
    private LayerMask platformLayerMask;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        isGrounded();
    }

    private void OnMouseDown()
    {
        if (!isGrounded())
        {
            return;
        }
        beginDragPos = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isGrounded())
        {
            return;
        }
        endDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(beginDragPos.x, beginDragPos.y, -1);
        positions[1] = new Vector3(endDragPos.x, endDragPos.y, -1);
        lr.enabled = true;
        lr.SetPositions(positions);
    }

    private void OnMouseUp()
    {
        if (!isGrounded())
        {
            return;
        }
        Vector2 forceVector = endDragPos - beginDragPos;
        rb.AddForce(-forceVector * 3, ForceMode2D.Impulse);
        lr.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Deathbox"))
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = new Vector3(0, 1, 0);
    }

    private bool isGrounded()
    {
        float extraHeight = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, extraHeight, platformLayerMask);
        return raycastHit.collider != null;
    }
}
