
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private UIInfo _infoText;
    [SerializeField]
    private UIInfo _helmetOfFarsightCount;
    [SerializeField]
    private UIInfo _shieldOfGravityCount;
    [SerializeField]
    private UIInfo _bootsOfFrictionCount;
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
    [SerializeField]
    private float _movingPlatformRatio;
    private bool startGameTextShowing = true;

    private Dictionary<LevelItem, int> _playerItems = new Dictionary<LevelItem, int>
    {
        { LevelItem.HELMET_OF_FARSIGHT, 0 },
        { LevelItem.SHIELD_OF_GRAVITY, 0 },
        { LevelItem.BOOTS_OF_FRICTION, 0 }
    };
    private float _difficultyStep = 0.1f;

    private void Start()
    {
        SetPlayerDefaults();
        _levelGenerator.GenerateLevel(false, RandomLevelItem(), _totalPlatforms, _widePlatformRatio, _spikedPlatformRatio, _icePlatformRatio, _movingPlatformRatio);
        _infoText.SetText("Get to the top of the level\n\n"
            + "Drag over the warrior to throw him");
    }

    private void SetPlayerDefaults()
    {
        _player.UpdatePlayerFriction(0.4f);
    }

    private LevelItem RandomLevelItem()
    {
        return (LevelItem)UnityEngine.Random.Range(0, 3);
    }

    public void CompleteLevel(LevelItem item)
    {
        StartCoroutine(CompleteLevelCoroutine(item));
    }

    public void Update()
    {
        if (startGameTextShowing && Input.GetMouseButtonDown(0))
        {
            _infoText.SetText("");
        }
    }

    public IEnumerator CompleteLevelCoroutine(LevelItem item)
    {
        _finishSound.Play();
        AddPlayerItem(item);
        _player.CompleteLevel();
        _infoText.SetText(LevelItemTitles.Get(item) + " Acquired!\n"
            + LevelItemDescriptions.Get(item));
        yield return new WaitForSeconds(4f);
        _infoText.SetText("");
        _timeText.ResetTime();
        _stageText.IncrementStage();
        _widePlatformRatio = Math.Max(0, _widePlatformRatio - _difficultyStep);
        _spikedPlatformRatio = Math.Min(1, _spikedPlatformRatio + _difficultyStep);
        _icePlatformRatio = Math.Min(1, _icePlatformRatio + _difficultyStep);
        _movingPlatformRatio = Math.Min(1, _icePlatformRatio + _difficultyStep);
        _levelGenerator.GenerateLevel(true, RandomLevelItem(), _totalPlatforms, _widePlatformRatio, _spikedPlatformRatio, _icePlatformRatio, _movingPlatformRatio);
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
        UpdateItemCounts();
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

    private void UpdateItemCounts()
    {
        _helmetOfFarsightCount.SetText("x" + _playerItems[LevelItem.HELMET_OF_FARSIGHT]);
        _shieldOfGravityCount.SetText("x" + _playerItems[LevelItem.SHIELD_OF_GRAVITY]);
        _bootsOfFrictionCount.SetText("x" + _playerItems[LevelItem.BOOTS_OF_FRICTION]);
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
            int posCount = 12 + (1 * _playerItems[LevelItem.HELMET_OF_FARSIGHT]);
            _player.UpdateTrajectoryLine(posCount);
        }

        // HELMET OF FARSIGHT
        if (_playerItems.ContainsKey(LevelItem.SHIELD_OF_GRAVITY))
        {
            _player.UpdateGravity((float) (1 - (0.1 * _playerItems[LevelItem.SHIELD_OF_GRAVITY])));
        }
    }
}
