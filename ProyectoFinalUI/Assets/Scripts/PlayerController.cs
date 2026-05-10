using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement & Camera")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;

    [Header("Stamina Cooldown")]
    public bool isExhausted = false; // El jugador no puede correr si está agotado
    public float staminaThreshold = 20f; // Necesita recuperar esto para volver a correr

    private CharacterController controller;
    private Vector2 moveInput;
    private bool isSprinting;
    private float verticalVelocity;

    public bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
        HandleStamina();
    }

    // Métodos vinculados al Player Input Component (Events)
    public void OnMove(InputAction.CallbackContext context) => moveInput = context.ReadValue<Vector2>();
    public void OnSprint(InputAction.CallbackContext context) => isSprinting = context.ReadValueAsButton();

    private void HandleMovement()
    {
        // Calcular dirección relativa a la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 moveDirection = (forward * moveInput.y + right * moveInput.x).normalized;

        // Solo corre si tiene estamina Y no está agotado
        bool canSprint = isSprinting && currentStamina > 0 && !isExhausted && moveInput.magnitude > 0;
        float speed = canSprint ? moveSpeed * sprintMultiplier : moveSpeed;

        // Movimiento y Gravedad simple
        if (controller.isGrounded) verticalVelocity = -0.5f;
        else verticalVelocity += Physics.gravity.y * Time.deltaTime;

        Vector3 finalVelocity = (moveDirection * speed) + (Vector3.up * verticalVelocity);
        controller.Move(finalVelocity * Time.deltaTime);

        // Rotación del personaje hacia donde camina
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleStamina()
    {
        if (isSprinting && moveInput.magnitude > 0 && !isExhausted)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true; // Se agota al llegar a 0
            }
        }
        else
        {
            currentStamina += staminaRegen * Time.deltaTime;
            // Si estaba agotado, solo puede volver a correr cuando recupere el umbral
            if (isExhausted && currentStamina >= staminaThreshold)
            {
                isExhausted = false;
            }
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.diePanel.SetActive(true);
    }
}