using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveForce = 2f;
    public float runMultiplier = 4f;
    public float rotationSpeed = 10f;

    [Header("Salto")]
    public float jumpForce = 200f;

    [Header("Cámara")]
    public Transform cameraTransform;

    private Rigidbody rb;
    private Animator animatorPlayer;

    private bool isGrounded;
    private Vector3 moveDirection;

    [Header("UIbottoms")]
    public GameObject jaguarBottom;
    public GameObject condorBottom;

    // -------- Recolección y poderes --------
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

        // Rotación
        if (moveDirection.magnitude > 0.1f)
        {
            Rotation(moveDirection);
        }

        // Saltos
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (Input.GetKey(KeyCode.R))
                RunJump();
            else
                NormalJump();
        }
    }

    private void FixedUpdate()
    {
        bool runInput = Input.GetKey(KeyCode.R);
        MoveCharacter(moveDirection, runInput);
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

    private void RunJump()
    {
        rb.AddForce(Vector3.up * (jumpForce * 1.2f), ForceMode.Impulse);
        isGrounded = false;

        if (animatorPlayer != null)
        {
            animatorPlayer.SetBool("isJumping", true);
        }
    }

    // ----------- TESOROS Y PODERES -----------

    public void AddTreasure()
    {
        treasureCount++;
        Debug.Log("Tesoros recolectados: " + treasureCount);
    }

    public void ActivatePower(string powerName)
    {
        if (!activePowers.Contains(powerName))
        {
            activePowers.Add(powerName);
            Debug.Log("¡Poder activado!: " + powerName);

            switch (powerName)
            {
                case "SpeedBoost":
                    //moveForce *= 10f;
                    jaguarBottom.gameObject.SetActive(true);
                    runMultiplier *= 10f;
                    Invoke(nameof(ResetSpeed), 5f);
                    
                    break;

                case "JumpBoost":
                    condorBottom.gameObject.SetActive(true);
                    jumpForce *= 50f;
                    Invoke(nameof(ResetJump), 5f);
                    break;

                // Aquí puedes agregar más poderes fácilmente
            }
        }
    }

    private void ResetSpeed()
    {
        moveForce /= 2f;
        activePowers.Remove("SpeedBoost");
        jaguarBottom.gameObject.SetActive(false);
    }

    private void ResetJump()
    {
        jumpForce /= 1.5f;
        activePowers.Remove("JumpBoost");
        condorBottom.gameObject.SetActive(false);
    }
}
