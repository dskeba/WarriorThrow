
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private LineRenderer _slingLr;
    [SerializeField]
    private LineRenderer _trajectoryLr;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private BoxCollider2D _bc;
    [SerializeField]
    private LayerMask _platrformLayerMask;
    [SerializeField]
    private AudioSource _throwSound;

    private int _startingNumOfTrajectoryPoints = 12;
    private float _timeBetweenTrajectoryPoints = 0.05f;
    private Vector2 _beginDragPos;
    private Vector2 _endDragPos;

    private void Awake()
    {
        _trajectoryLr.positionCount = _startingNumOfTrajectoryPoints;
    }

    private void OnMouseDown()
    {
        if (!isGrounded() || _player.IsDead)
        {
            return;
        }
    }

    private void OnMouseDrag()
    {
        if (!isGrounded() || _player.IsDead)
        {
            return;
        }
        _beginDragPos = transform.position;
        _endDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(_beginDragPos.x, _beginDragPos.y, -1);
        positions[1] = new Vector3(_endDragPos.x, _endDragPos.y, -1);
        _slingLr.enabled = true;
        _slingLr.SetPositions(positions);
        Vector3[] trajectoryPoints = new Vector3[_trajectoryLr.positionCount];
        for (int i = 0; i < _trajectoryLr.positionCount; i++)
        {
            trajectoryPoints[i] = pointPosition(i * _timeBetweenTrajectoryPoints, getForceVector());
        }
        _trajectoryLr.enabled = true;
        _trajectoryLr.SetPositions(trajectoryPoints);
    }

    private Vector2 pointPosition(float time, Vector2 forceVector)
    {
        return (Vector2)transform.position + (forceVector * time) + (Physics2D.gravity * (time * time) / 2);
    }

    private Vector2 getForceVector()
    {
        return (_endDragPos - _beginDragPos) * -3;
    }

    private void OnMouseUp()
    {
        _slingLr.enabled = false;
        _trajectoryLr.enabled = false;
        if (!isGrounded() || _player.IsDead)
        {
            return;
        }
        _throwSound.Play();
        _rb.velocity = getForceVector();
    }

    private bool isGrounded()
    {
        float threshhold = 0.01f;

        // Downward boxcast at bottom of players feet
        Vector2 boxcastCenter = new Vector2(_bc.bounds.center.x, _bc.bounds.center.y - _bc.bounds.extents.y);
        Vector2 boxcastSize = new Vector2(_bc.bounds.extents.x * 2, threshhold);
        RaycastHit2D boxcastHit = Physics2D.BoxCast(boxcastCenter, boxcastSize, 0, Vector2.down, 0, _platrformLayerMask);

        // Left and right raycast to check if touching a platform immediately to left or right sides
        Vector3 bottomLeft = new Vector2(_bc.bounds.center.x - _bc.bounds.extents.x, _bc.bounds.center.y - _bc.bounds.extents.y + 0.01f);
        RaycastHit2D linecastHitLeft = Physics2D.Raycast(bottomLeft, Vector2.left, threshhold, _platrformLayerMask);
        Vector3 bottomRight = new Vector2(_bc.bounds.center.x + _bc.bounds.extents.x, _bc.bounds.center.y - _bc.bounds.extents.y + 0.01f);
        RaycastHit2D linecastHitRight = Physics2D.Raycast(bottomRight, Vector2.right, threshhold, _platrformLayerMask);

        // Only grounded if boxcast downward is touching and sides are not touching
        return (boxcastHit.collider && !(linecastHitLeft.collider || linecastHitRight.collider));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            transform.parent = null;
        }
    }
}
