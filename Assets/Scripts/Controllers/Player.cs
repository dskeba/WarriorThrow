
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private SpriteRenderer _sr;
    [SerializeField]
    private Sprite _idleSprite;
    [SerializeField]
    private Sprite _deadSprite;
    [SerializeField]
    private LineRenderer _trajectoryLr;
    [SerializeField]
    private AudioSource _deathSound;
    [SerializeField]
    private LevelSystem _levelSystem;

    public bool IsDead;

    private void FixedUpdate()
    {
        _levelSystem.UpdatePlayerHeight(transform.position.y);
    }

    public void ResetPlayer()
    {
        UnfreezePlayer();
        IsDead = false;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _sr.sprite = _idleSprite;
        _sr.color = Color.white;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(0, 1, 0);
    }

    public void FreezePlayer()
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
    }

    public void UnfreezePlayer()
    {
        _rb.isKinematic = false;
    }

    public void UpdateGravity(float scale)
    {
        _rb.gravityScale = scale;
    }

    public void CompleteLevel()
    {
        FreezePlayer();
        _sr.color = new Color(1, 0.8f, 0);
    }

    public void UpdateTrajectoryLine(int posCount)
    {
        _trajectoryLr.positionCount = posCount;
    }

    public void UpdatePlayerFriction(float friction)
    {
        _rb.sharedMaterial.friction = friction;
    }

    public void Die()
    {
        if (IsDead)
        {
            return;
        }
        IsDead = true;
        _deathSound.Play();
        transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        _sr.color = new Color(1, 0.5f, 0.5f);
        _sr.sprite = _deadSprite;
    }
}
