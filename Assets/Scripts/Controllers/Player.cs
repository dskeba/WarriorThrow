
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private BoxCollider2D bc;
    [SerializeField]
    private LevelGenerator lg;
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Sprite idleSprite;
    [SerializeField]
    private Sprite deadSprite;
    [SerializeField]
    private LineRenderer trajectoryLine;
    [SerializeField]
    private LayerMask platformLayerMask;
    [SerializeField]
    private UIHeight heightText;
    [SerializeField]
    private UITimer timeText;
    [SerializeField]
    private UIStage stageText;
    [SerializeField]
    private AudioSource throwSound;
    [SerializeField]
    private AudioSource deathSound;
    [SerializeField]
    private AudioSource finishSound;

    private int numOfTrajectoryPoints = 15;
    private float timeBetweenTrajectoryPoints = 0.05f;
    private Vector2 beginDragPos;
    private Vector2 endDragPos;
    private bool isDead = false;

    private void Awake()
    {
        trajectoryLine = transform.Find("Trajectory").GetComponent<LineRenderer>();
        trajectoryLine.positionCount = numOfTrajectoryPoints;
    }

    private void FixedUpdate()
    {
        heightText.UpdateHeight((int)transform.position.y);
    }

    private void OnMouseDown()
    {
        if (!isGrounded() || isDead)
        {
            return;
        }
        beginDragPos = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isGrounded() || isDead)
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
        lr.enabled = false;
        trajectoryLine.enabled = false;
        if (!isGrounded() || isDead)
        {
            return;
        }
        throwSound.Play();
        rb.velocity = getForceVector();
    }

    private Vector2 getForceVector()
    {
        return (endDragPos - beginDragPos) * -3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Deathbox"))
        {
            StartCoroutine(Death());
        } else if (collision.gameObject.tag.Equals("Star"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(Finish());
        }
    }

    private void ResetGame(bool regenerateLevel)
    {
        if (regenerateLevel)
        {
            timeText.ResetTime();
            lg.GenerateLevel(true);
        }
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        transform.position = new Vector3(0, 1, 0);
    }

    private IEnumerator Finish()
    {
        finishSound.Play();
        sr.color = new Color(1, 0.8f, 0);
        yield return new WaitForSeconds(1f);
        sr.color = Color.white;
        sr.sprite = idleSprite;
        isDead = false;
        stageText.IncrementStage();
        ResetGame(true);
    }

    private IEnumerator Death()
    {
        if (isDead)
        {
            yield break;
        }
        deathSound.Play();
        isDead = true;
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        sr.color = new Color(1, 0.5f, 0.5f);
        sr.sprite = deadSprite;
        yield return new WaitForSeconds(1f);
        sr.color = Color.white;
        sr.sprite = idleSprite;
        isDead = false;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        ResetGame(false);

    }

    private bool isGrounded()
    {
        float threshhold = 0.01f;

        // Downward boxcast at bottom of players feet
        Vector2 boxcastCenter = new Vector2(bc.bounds.center.x, bc.bounds.center.y - bc.bounds.extents.y);
        Vector2 boxcastSize = new Vector2(bc.bounds.extents.x * 2, threshhold);
        RaycastHit2D boxcastHit = Physics2D.BoxCast(boxcastCenter, boxcastSize, 0, Vector2.down, 0, platformLayerMask);

        // Left and right raycast to check if touching a platform immediately to left or right sides
        Vector3 bottomLeft = new Vector2(bc.bounds.center.x - bc.bounds.extents.x, bc.bounds.center.y - bc.bounds.extents.y + 0.01f);
        RaycastHit2D linecastHitLeft = Physics2D.Raycast(bottomLeft, Vector2.left, threshhold, platformLayerMask);
        Vector3 bottomRight = new Vector2(bc.bounds.center.x + bc.bounds.extents.x, bc.bounds.center.y - bc.bounds.extents.y + 0.01f);
        RaycastHit2D linecastHitRight = Physics2D.Raycast(bottomRight, Vector2.right, threshhold, platformLayerMask);

        // Only grounded if boxcast downward is touching and sides are not touching
        return (boxcastHit.collider && !(linecastHitLeft.collider || linecastHitRight.collider));
    }

    private Vector2 pointPosition(float time, Vector2 force)
    {
        return (Vector2)transform.position + (getForceVector() * time) + (Physics2D.gravity * (time * time) / 2);
    }
}
