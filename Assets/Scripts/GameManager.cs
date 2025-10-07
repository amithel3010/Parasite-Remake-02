using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PlayerRespawnHandler _playerRespawnHandler;
    private InputHandler _playerInput;

    private bool _isPaused;
    
    private bool _lastDebugPressed;
    private bool _lastPausePressed;
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
        
        if (_playerInput == null)
        {
            _playerInput = FindAnyObjectByType<InputHandler>();
        }

        _playerRespawnHandler = FindAnyObjectByType<PlayerRespawnHandler>();
        
    }

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_playerInput.DebugPressed && !_lastDebugPressed)
        {
            UIManager.Instance.ToggleDebugMenuCanvas();
        }

        if (_playerInput.PausePressed && !_lastPausePressed)
        {
            PauseGame();
        }

        _lastDebugPressed = _playerInput.DebugPressed;
        _lastPausePressed = _playerInput.PausePressed;
    }

    private void PauseGame()
    {
        UIManager.Instance.ShowPauseMenu();
        Time.timeScale = 0f;
        _isPaused = true;
    }

    public void ResumeGame()
    {
        UIManager.Instance.HidePauseMenu();
        Time.timeScale = 1f;
        _isPaused = false;
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOverScreen();
        _isPaused = true;
    }

    public void RestartFromCheckpoint()
    {
        Time.timeScale = 1f;
        CheckpointManager.Instance.RespawnParasite();
        _playerRespawnHandler.OnRespawn();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isPaused = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
