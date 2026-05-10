using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // Unlock the cursor
        Cursor.visible = true; // Make the cursor visible
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
