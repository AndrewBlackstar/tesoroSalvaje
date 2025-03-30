using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // Variables públicas para ajustar las fuerzas de movimiento y salto
    public float moveForce = 10f;  // Fuerza de movimiento
    public float jumpForce = 1250.0f;  // Fuerza de salto

    // Referencias a otros componentes y objetos en la escena
    public Transform cameraTransform;  // Cámara para orientar el movimiento
    private Rigidbody rb;  // Componente Rigidbody de la esfera
    private Animator animatorPlayer;  // Animator del jugador

    // Variables de estado del jugador
    private bool isGrounded; // Indica si el jugador está en el suelo
    private bool hasJumped;  // Indica si el jugador ha saltado recientemente

    private void Awake()
    {
        // Asegurarse de que la cámara esté asignada
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // Asignar la cámara principal si no se ha asignado
        }
    }

    void Start()
    {
        // Inicialización de componentes y variables
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 5f;  // Ajuste del drag para reducir el deslizamiento
        animatorPlayer = GetComponent<Animator>();
        
        hasJumped = false; // Inicialmente no ha saltado
        isGrounded = true; // Se asume que empieza en el suelo
    }

    void FixedUpdate()
    {
        // Capturar entrada del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Obtener las direcciones de la cámara para orientar el movimiento
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0; // Ignorar componente vertical
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Calcular la dirección de movimiento basada en la cámara
        Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

        // Aplicar velocidad directamente en el eje X y Z
        rb.linearVelocity = new Vector3(moveDirection.x * moveForce, rb.linearVelocity.y, moveDirection.z * moveForce);

        // Si hay movimiento, rotar el jugador hacia la dirección de movimiento
        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        // Control de animaciones del jugador
        animatorPlayer.SetBool("isWalking", moveX != 0 || moveZ != 0);

        // Si el jugador presiona espacio y está en el suelo, salta
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        // Aplicar fuerza de salto al Rigidbody
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        hasJumped = true; // Registrar que ha saltado

        if (animatorPlayer != null)
    {
        animatorPlayer.SetBool("isJumping", true);
    }
        // Desactivar la animación de caminar al saltar
        animatorPlayer.SetBool("isWalking", false);
        // Desactivar la animación de correr al saltar
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Asegúrate de etiquetar el suelo como "Ground"
        {
            isGrounded = true;
            hasJumped = false;
             // Desactivar la animación de salto
        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isJumping", false);
        }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
