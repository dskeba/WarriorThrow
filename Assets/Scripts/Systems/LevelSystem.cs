
using System.Collections;
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
    private AudioSource _finishSound;
    [SerializeField]
    private LevelGenerator _levelGenerator;

    public void CompleteLevel()
    {
        StartCoroutine(CompleteLevelWithDelay(1f));
    }

    public IEnumerator CompleteLevelWithDelay(float seconds)
    {
        _finishSound.Play();
        _player.CompleteLevel();
        yield return new WaitForSeconds(seconds);
        _timeText.ResetTime();
        _stageText.IncrementStage();
        _levelGenerator.GenerateLevel(true);
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
