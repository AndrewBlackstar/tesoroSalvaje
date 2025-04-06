using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveForce = 3f;
    public float runMultiplier = 4f;
    public float rotationSpeed = 5f;

    [Header("Salto")]
    public float jumpForce = 5f;
    public float runJumpVerticalRatio = 0.3f;
    public float runJumpForwardRatio = 1.5f;

    [Header("Dash Settings")]
    public float dashSpeed = 30f; // Velocidad lineal del dash
    public float dashDuration = 0.3f; // Tiempo que dura el impulso del dash
    public float dashCooldown = 1f; // Tiempo de espera entre dashes
    public float dashVerticalReduction = 0.2f; // Reducción del movimiento vertical
    private bool isDashing = false;
    private bool canDash = true;
    
    [Header("Alas y Mascara")]
    public GameObject mask;
    public GameObject wings;

    [Header("Cámara")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator animatorPlayer;

    private bool isGrounded;
    private Vector3 moveDirection;

    [Header("UIbottoms")]
    public GameObject jaguarBottom;
    public GameObject condorBottom;
    public PlayerUIBars uiBars; // Asignás esto en el Inspector
    
    [Header("PowerUps")]
    private int treasureCount = 0;
    private List<string> activePowers = new List<string>();

    

    private void Awake()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animatorPlayer = GetComponent<Animator>();
        isGrounded = true;
    }

    private void Update()
    {
        CheckGroundStatus();

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * moveZ + right * moveX).normalized;

        if (moveDirection.magnitude > 0.1f)
        {
            Rotation(moveDirection);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            NormalJump();
        }

        if (Input.GetKeyDown(KeyCode.E) && isGrounded && activePowers.Contains("JumpBoost"))
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        bool runInput = Input.GetKey(KeyCode.R);
        if (!isDashing) // Solo mover si no estamos en dash
        {
            MoveCharacter(moveDirection, runInput);
        }
    }

    private void CheckGroundStatus()
    {
        float rayDistance = 1.1f;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, rayDistance);

        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isJumping", !isGrounded);
        }
    }

    private void MoveCharacter(Vector3 direction, bool isRunningInput)
    {
        float currentForce = moveForce;

        bool isRunning = false;
        bool isWalking = false;
        bool isIdle = true;

        if (direction.magnitude > 0.1f)
        {
            isIdle = false;

            if (isRunningInput)
            {
                currentForce *= runMultiplier;
                isRunning = true;
            }
            else
            {
                isWalking = true;
            }
        }

        rb.linearVelocity = new Vector3(direction.x * currentForce, rb.linearVelocity.y, direction.z * currentForce);

        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isIdle", isIdle);
            animatorPlayer.SetBool("isWalking", isWalking);
            animatorPlayer.SetBool("isRunning", isRunning);
        }
    }

    private void Rotation(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void NormalJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;

        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isJumping", true);
        }
    }

    private void StartDash()
    {
        if (!canDash) return;

        isDashing = true;
        canDash = false;
        
        // Calcular dirección del dash (usa input o dirección actual)
        Vector3 dashDir = (moveDirection.magnitude > 0.1f) ? moveDirection.normalized : transform.forward;
        
        // Resetear velocidades existentes
        rb.linearVelocity = Vector3.zero;
        
        // Aplicar fuerza del dash (principalmente horizontal)
        Vector3 dashForce = new Vector3(
            dashDir.x * dashSpeed,
            dashDir.y * dashSpeed * dashVerticalReduction, // Muy poca componente vertical
            dashDir.z * dashSpeed
        );
        
        rb.AddForce(dashForce, ForceMode.VelocityChange);
        
        // Configurar animación
        animatorPlayer?.SetBool("isJumping", true);
        
        // Temporizadores
        Invoke(nameof(EndDash), dashDuration);
        Invoke(nameof(ResetDash), dashCooldown);
    }
    private void EndDash()
    {
        isDashing = false;
        // No es necesario restaurar gravedad porque no la desactivamos
    }

    private void ResetDash()
    {
        canDash = true;
    }

    public void AddTreasure()
    {
        treasureCount++;
        Debug.Log("Tesoros recolectados: " + treasureCount);
    }

     // ----------- TESOROS Y PODERES -----------
    public void ActivatePower(string powerName)
    {
        if (!activePowers.Contains(powerName))
        {
            activePowers.Add(powerName);
            Debug.Log("¡Poder activado!: " + powerName);

            switch (powerName)
            {
                case "SpeedBoost":
                    //jaguarBottom.gameObject.SetActive(true);
                    mask.gameObject.SetActive(true);
                    runMultiplier *= 10f;
                    Invoke(nameof(ResetSpeed), 5f);
                    break;

                case "JumpBoost":
                    //condorBottom.gameObject.SetActive(true);
                    wings.gameObject.SetActive(true);
                    Invoke(nameof(ResetJump), 5f);
                    break;
            }
        }
    }

    private void ResetSpeed()
    {
        runMultiplier /= 10f;
        activePowers.Remove("SpeedBoost");
        jaguarBottom.gameObject.SetActive(false);
        wings.gameObject.SetActive(false);
    }

    private void ResetJump()
    {
        activePowers.Remove("JumpBoost");
        condorBottom.gameObject.SetActive(false);
        wings.gameObject.SetActive(true);
    }
}