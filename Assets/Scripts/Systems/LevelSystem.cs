
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelItem
{
    HELMET_OF_FARSIGHT,
    BOOTS_OF_FRICTION
}

class LevelSystem : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private UIHeight _heightText;
    [SerializeField]
    private UITimer _timeText;
    [SerializeField]
    private UIStage _stageText;
    [SerializeField]
    private AudioSource _finishSound;
    [SerializeField]
    private LevelGenerator _levelGenerator;
    [SerializeField]
    private int _totalPlatforms;
    [SerializeField]
    private float _widePlatformRatio;
    [SerializeField]
    private float _spikedPlatformRatio;
    [SerializeField]
    private float _icePlatformRatio;

    private Dictionary<LevelItem, int> _playerItems = new Dictionary<LevelItem, int>();
    private float _difficultyStep = 0.1f;

    private void Start()
    {
        SetPlayerDefaults();
        _levelGenerator.GenerateLevel(false, RandomLevelItem(), _totalPlatforms, _widePlatformRatio, _spikedPlatformRatio, _icePlatformRatio);
    }

    private void SetPlayerDefaults()
    {
        _player.UpdatePlayerFriction(0.4f);
    }

    private LevelItem RandomLevelItem()
    {
        return (LevelItem)UnityEngine.Random.Range(0, 2);
    }

    public void CompleteLevel(LevelItem item)
    {
        StartCoroutine(CompleteLevelCoroutine(item));
    }

    public IEnumerator CompleteLevelCoroutine(LevelItem item)
    {
        _finishSound.Play();
        AddPlayerItem(item);
        _player.CompleteLevel();
        yield return new WaitForSeconds(1f);
        _timeText.ResetTime();
        _stageText.IncrementStage();
        _widePlatformRatio = Math.Max(0, _widePlatformRatio - _difficultyStep);
        _spikedPlatformRatio = Math.Min(1, _spikedPlatformRatio + _difficultyStep);
        _icePlatformRatio = Math.Min(1, _icePlatformRatio + _difficultyStep);
        _levelGenerator.GenerateLevel(true, RandomLevelItem(), _totalPlatforms, _widePlatformRatio, _spikedPlatformRatio, _icePlatformRatio);
        _player.ResetPlayer();
    }

    public IEnumerator ResetLevelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _player.ResetPlayer();
    }

    public void UpdatePlayerHeight(float height)
    {
        _heightText.UpdateHeight((int)height, (int)_levelGenerator.TotalHeight);
    }

    public void KillPlayer()
    {
        _player.Die();
        StartCoroutine(ResetLevelAfterSeconds(1f));
    }

    public void AddPlayerItem(LevelItem item)
    {
        if (!_playerItems.ContainsKey(item))
        {
            _playerItems.Add(item, 1);
        }
        _playerItems[item] += 1;
        ResolvePlayerItems();
    }

    public void RemovePlayerItem(LevelItem item)
    {
        if (_playerItems[item] > 0)
        {
            _playerItems.Add(item, _playerItems[item] - 1);
        }
        ResolvePlayerItems();
    }

    private void ResolvePlayerItems()
    {
        // BOOTS OF FRICTION
        if (_playerItems.ContainsKey(LevelItem.BOOTS_OF_FRICTION))
        {
            float playerFriction = 0.4f + (0.05f * _playerItems[LevelItem.BOOTS_OF_FRICTION]);
            _player.UpdatePlayerFriction(playerFriction);
        }

        // HELMET OF FARSIGHT
        if (_playerItems.ContainsKey(LevelItem.HELMET_OF_FARSIGHT))
        {
            Color startColor = new Color(1f, 0.9f, 0.1f, 1f);
            Color endColor = new Color(1f, 0.9f, 0.1f, 0f);
            int posCount = 12 + (1 * _playerItems[LevelItem.HELMET_OF_FARSIGHT]);
            _player.UpdateTrajectoryLine(startColor, endColor, posCount);
        }
    }
}
