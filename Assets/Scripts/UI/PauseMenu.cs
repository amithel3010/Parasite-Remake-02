using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;


    public void Pause()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(true);
            Debug.Log("test");
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













}
