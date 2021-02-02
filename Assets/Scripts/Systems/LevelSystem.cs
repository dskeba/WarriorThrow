
using System.Collections;
using UnityEngine;

public enum LevelItem
{
    STAR,
    FARSIGHT_HELMET
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
        _levelGenerator.GenerateLevel(true, LevelItem.STAR);
        _player.ResetPlayer();
    }

    public IEnumerator ResetLevelAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _player.ResetPlayer();
    }

    public void UpdatePlayerHeight(float height)
    {
        _heightText.UpdateHeight((int)height);
    }

    public void KillPlayer()
    {
        _player.Die();
        StartCoroutine(ResetLevelAfterSeconds(1f));
    }

}
