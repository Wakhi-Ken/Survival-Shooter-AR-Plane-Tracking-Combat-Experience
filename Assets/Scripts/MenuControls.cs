using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject pauseCanvas;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

  
    // STAGE LOADING
   

    public void LoadStage1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage1");
    }

    public void LoadStage2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage2");
    }

    // PAUSE SYSTEM
   

    public void PauseGame()
    {
        isPaused = true;

        Time.timeScale = 0f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (pauseCanvas != null)
            pauseCanvas.SetActive(false);
    }

    
    // MAIN MENU
    

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start-Menu");
    }

    
    // QUIT GAME
  

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}