
using UnityEngine;

public class Deathbox : MonoBehaviour
{
    private LevelSystem _levelSystem;

    private void Awake()
    {
        _levelSystem = GameObject.FindGameObjectWithTag("LevelSystem").GetComponent<LevelSystem>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            _levelSystem.KillPlayer();
        }
    }
}
