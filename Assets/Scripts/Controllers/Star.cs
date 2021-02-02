using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
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
            gameObject.SetActive(false);
            _levelSystem.CompleteLevel(LevelItem.STAR);
        }
    }
}
