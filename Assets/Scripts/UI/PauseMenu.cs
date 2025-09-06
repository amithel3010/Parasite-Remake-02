using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool IsGamePaused = false;
    private bool Audio = true;

    void Update()
    {
        Pause();
    }

    


    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGamePaused = !IsGamePaused;
            Debug.Log("click");
        }
        if (IsGamePaused == true)
        {
            pauseMenu.SetActive(true);

        }
        else
        {
            pauseMenu.SetActive(false);

        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }
    

    public void IACad()
    {
        Application.OpenURL("https://www.ani-mator.com/");
    }


    public void MuteAllSound()
    {
        AudioListener.volume = 0f;
    }










}
