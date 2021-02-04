
using System.Collections;
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

    private void Start()
    {
        _levelGenerator.GenerateLevel(false, RandomLevelItem());
    }

    private LevelItem RandomLevelItem()
    {
        return (LevelItem)Random.Range(0, 2);
    }

    public void CompleteLevel(LevelItem item)
    {
        StartCoroutine(CompleteLevelCoroutine(item));
    }

    public IEnumerator CompleteLevelCoroutine(LevelItem item)
    {
        _finishSound.Play();
        _player.CompleteLevel(item);
        yield return new WaitForSeconds(1f);
        _timeText.ResetTime();
        _stageText.IncrementStage();
        _levelGenerator.GenerateLevel(true, RandomLevelItem());
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

}
