
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
    private LevelGenerator _levelGenerator;

    public void GenerateNextLevel()
    {
        _timeText.ResetTime();
        _stageText.IncrementStage();
        _levelGenerator.GenerateLevel(true);
        _player.ResetPlayer();
    }

    public void ResetCurrentLevel()
    {
        _player.ResetPlayer();
    }

    public void UpdatePlayerHeight(float height)
    {
        _heightText.UpdateHeight((int)height);
    }

}
