using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{

    public PlayerController player;
    public TextMeshProUGUI potionText;
    public CanvasGroup potionIconGroup, weaponIconGroup;

    [Header("Bars")]
    public Image healthBarFill;
    public Image staminaBarFill;
    public Color exhaustedColor = Color.gray;
    public Color normalStaminaColor = Color.yellow;

    [Header("Panels")]
    public GameObject pausePanel;
    public GameObject deathPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizar barras
        healthBarFill.fillAmount = player.currentHealth / player.maxHealth;
        staminaBarFill.fillAmount = player.currentStamina / player.maxStamina;

        // Feedback visual si está agotado
        staminaBarFill.color = player.isExhausted ? exhaustedColor : normalStaminaColor;

        // Actualizar contador de pociones
        potionText.text = "x" + player.potionCount;
        if (player.potionCount > 0)
        {
            potionIconGroup.alpha = 1f; // Mostrar icono de poción
        }
        else
        {
            potionIconGroup.alpha = 0.3f; // Atenuar icono de poción si no hay
        }

        // Feedback visual del arma
        // Si está equipada, opacidad al 100%, si no, al 30%
        weaponIconGroup.alpha = player.isWeaponEquipped ? 1f : 0.3f;

        // Mostrar panel de muerte
        if (player.isDead && !deathPanel.activeSelf)
        {
            deathPanel.SetActive(true);
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (Time.timeScale == 0f)
            {
                Continue(); // Si ya está pausado, reanudar el juego
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
        pausePanel.SetActive(false); // Ocultar el panel de pausa
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor
        Cursor.visible = false; // Ocultar el cursor
        Time.timeScale = 1f; // Reanudar el juego
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté normalizado antes de reiniciar
        SceneManager.LoadScene("Game");
    }

    public void ReturntoMenu()
    {
        Time.timeScale = 1f; // Asegurarse de que el tiempo esté normalizado antes de cargar el menú
        SceneManager.LoadScene("Menu");
    }
}
