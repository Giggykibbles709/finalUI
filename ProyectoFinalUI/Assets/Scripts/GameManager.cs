using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public PlayerController player;
    public Image healthBarFill;
    public Image staminaBarFill;
    public GameObject pausePanel;
    public GameObject diePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            healthBarFill.fillAmount = player.currentHealth / player.maxHealth;
            staminaBarFill.fillAmount = player.currentStamina / player.maxStamina;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.timeScale == 0f)
            {
                Continue(); // If the game is already paused, resume it
            }
            else
            {
                pausePanel.SetActive(true); // Show the pause panel
                Cursor.lockState = CursorLockMode.None; // Unlock the cursor
                Cursor.visible = true; // Make the cursor visible
                Time.timeScale = 0f; // Pause the game
            }
        }
    }

    public void Continue()
    {
        pausePanel.SetActive(false); // Hide the pause panel
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
        Cursor.visible = false; // Hide the cursor
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
