
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

    private List<LevelItem> _items = new List<LevelItem>();

    public bool IsDead;

    private void FixedUpdate()
    {
        _levelSystem.UpdatePlayerHeight(transform.position.y);
    }

    public void ResetPlayer()
    {
        IsDead = false;
        _rb.velocity = Vector2.zero;
        _rb.angularVelocity = 0;
        _sr.sprite = _idleSprite;
        _sr.color = Color.white;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(0, 1, 0);
    }

    public void CompleteLevel(LevelItem item)
    {
        _sr.color = new Color(1, 0.8f, 0);
        _items.Add(item);
        if (item.Equals(LevelItem.HELMET_OF_FARSIGHT)) {
            _trajectoryLr.positionCount = 30;
            Color startColor = new Color(1f, 0.9f, 0.1f, 1f);
            Color endColor = new Color(1f, 0.9f, 0.1f, 0f);
            _trajectoryLr.startColor = startColor;
            _trajectoryLr.endColor = endColor;
        }
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
