using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject pausePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        pausePanel.SetActive(false); // Hide the pause panel
        Time.timeScale = 1f; // Resume the game
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturntoMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
