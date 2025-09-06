using System;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool IsGamePaused = false;
    private bool Audio = true;


    private void Start()
    {
        IsGamePaused = false;
        
    }



    void Update()
    {
        Pause();
    }

    


    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsGamePaused = !IsGamePaused;
            pauseMenu.SetActive(IsGamePaused);
            Debug.Log("click");
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
        Debug.Log("Audio muted");
    }










}
