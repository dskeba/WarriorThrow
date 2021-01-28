
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 beginDragPos;
    private Vector2 endDragPos;
    private LineRenderer lr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private LevelGenerator lg;
    private LineRenderer trajectoryLine;
    [SerializeField]
    private LayerMask platformLayerMask;
    private int numOfTrajectoryPoints = 15;
    private float timeBetweenTrajectoryPoints = 0.05f;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        lg = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        trajectoryLine = transform.Find("Trajectory").GetComponent<LineRenderer>();
        trajectoryLine.positionCount = numOfTrajectoryPoints;
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
        Vector3[] trajectoryPoints = new Vector3[numOfTrajectoryPoints];
        for (int i = 0; i < numOfTrajectoryPoints; i++)
        {
            trajectoryPoints[i] = pointPosition(i * timeBetweenTrajectoryPoints, getForceVector());
        }
        trajectoryLine.enabled = true;
        trajectoryLine.SetPositions(trajectoryPoints);
    }

    private void OnMouseUp()
    {
        if (!isGrounded())
        {
            return;
        }
        rb.velocity = getForceVector();
        lr.enabled = false;
        trajectoryLine.enabled = false;
    }

    private Vector2 getForceVector()
    {
        return (endDragPos - beginDragPos) * -3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Deathbox"))
        {
            ResetGame(false);
        } else if (collision.gameObject.tag.Equals("Star"))
        {
            ResetGame(true);
        }
    }

    private void ResetGame(bool regenerateLevel)
    {
        if (regenerateLevel)
        {
            lg.GenerateLevel(true);
        }
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = new Vector3(0, 1, 0);
    }

    private bool isGrounded()
    {
        float extra = 0.05f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(bc.bounds.center, new Vector2((bc.bounds.extents.x * 2) + extra, bc.bounds.extents.y * 2), 0, Vector2.down, extra, platformLayerMask);
        return raycastHit.collider != null;
    }

    private Vector2 pointPosition(float time, Vector2 force)
    {
        return (Vector2)transform.position + (getForceVector() * time) + (Physics2D.gravity * (time * time) / 2);
    }
}
