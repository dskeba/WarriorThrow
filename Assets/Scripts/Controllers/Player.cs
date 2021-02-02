
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LineRenderer _lr;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private BoxCollider2D _bc;
    [SerializeField]
    private SpriteRenderer _sr;
    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _deadSprite;
    [SerializeField]
    private LineRenderer _trajectoryLr;
    [SerializeField]
    private LayerMask _platrformLayerMask;
    [SerializeField]
    private AudioSource _throwSound;
    [SerializeField]
    private AudioSource _deathSound;
    [SerializeField]
    private LevelSystem _levelSystem;

    private int _numOfTrajectoryPoints = 15;
    private float _timeBetweenTrajectoryPoints = 0.05f;
    private Vector2 _beginDragPos;
    private Vector2 _endDragPos;
    private bool _isDead = false;

    private void Awake()
    {
        _trajectoryLr.positionCount = _numOfTrajectoryPoints;
    }

    private void FixedUpdate()
    {
        _levelSystem.UpdatePlayerHeight(transform.position.y);
    }

    private void OnMouseDown()
    {
        if (!isGrounded() || _isDead)
        {
            return;
        }
        _beginDragPos = transform.position;
    }

    private void OnMouseDrag()
    {
        if (!isGrounded() || _isDead)
        {
            return;
        }
        _endDragPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(_beginDragPos.x, _beginDragPos.y, -1);
        positions[1] = new Vector3(_endDragPos.x, _endDragPos.y, -1);
        _lr.enabled = true;
        _lr.SetPositions(positions);
        Vector3[] trajectoryPoints = new Vector3[_numOfTrajectoryPoints];
        for (int i = 0; i < _numOfTrajectoryPoints; i++)
        {
            trajectoryPoints[i] = pointPosition(i * _timeBetweenTrajectoryPoints, getForceVector());
        }
        _trajectoryLr.enabled = true;
        _trajectoryLr.SetPositions(trajectoryPoints);
    }

    private void OnMouseUp()
    {
        _lr.enabled = false;
        _trajectoryLr.enabled = false;
        if (!isGrounded() || _isDead)
        {
            return;
        }
        _throwSound.Play();
        _rb.velocity = getForceVector();
    }

    private Vector2 getForceVector()
    {
        return (_endDragPos - _beginDragPos) * -3;
    }

    public void ResetPlayer()
    {
        _isDead = false;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _sr.sprite = _idleSprite;
        _sr.color = Color.white;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(0, 1, 0);
    }

    public void CompleteLevel()
    {
        _sr.color = new Color(1, 0.8f, 0);
    }

    public void Die()
    {
        if (_isDead)
        {
            return;
        }
        _isDead = true;
        _deathSound.Play();
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        _sr.color = new Color(1, 0.5f, 0.5f);
        _sr.sprite = _deadSprite;
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

    private Vector2 pointPosition(float time, Vector2 force)
    {
        return (Vector2)transform.position + (getForceVector() * time) + (Physics2D.gravity * (time * time) / 2);
    }
}
