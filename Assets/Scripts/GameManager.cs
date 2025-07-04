using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlayerHealthHandler _playerHealthHandler; //TODO: can't be interface, is thi sthe right way to do it?

    private bool _isPaused;
    public bool IsPaused => _isPaused;

    //private enum GameState { Playing, Paused, GameOver }
    //private GameState currentGameState; 

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 GameManager instance");
        }
        else
        {
            Instance = this;
        }
    }

    public void TogglePause()
    {
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOverScreen();
        _isPaused = true;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        CheckpointManager.Instance.RespawnParasite();
        _playerHealthHandler.HandleRespawn();
        _isPaused = false;
    }

}
