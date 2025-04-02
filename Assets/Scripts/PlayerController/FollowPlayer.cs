using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    
    private readonly Vector3 offset = new Vector3(20f, 0f, 33f); // Offset fijo
    public Vector3 rotationOffset = new Vector3(15, 180, 0);
    public float RotationSpeed = 200.0f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float smoothSpeed = 5f;
    public float zoomSmoothSpeed = 10f;

    private float yaw;
    private float pitch;
    private float targetZoom;
    private bool isFirstPerson = false;
    public Vector3 firstPersonOffset = new Vector3(0f, 1.8f, 0f);

    private float hudTimer = 0f;
    public float hudDisplayTime = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetZoom = offset.magnitude;

        if (player != null)
        {
            yaw = player.transform.eulerAngles.y;
            pitch = player.transform.eulerAngles.x;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFirstPerson = !isFirstPerson;
            hudTimer = hudDisplayTime;

            if (player != null)
            {
                player.SetActive(!isFirstPerson);
            }
        }
    }

    void LateUpdate()
{
    float mouseX = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;

    yaw += mouseX;
    pitch -= mouseY;
    pitch = Mathf.Clamp(pitch, 30f, 80f);

    if (isFirstPerson)
    {
        transform.position = player.transform.position + firstPersonOffset;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
    else
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoom = Mathf.Clamp(targetZoom - scroll * zoomSpeed, minZoom, maxZoom);
        
        Quaternion rotationOffsetQuat = Quaternion.Euler(rotationOffset);
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0) * rotationOffsetQuat;
        Vector3 rotatedOffset = targetRotation * offset.normalized * targetZoom;

        // Ajustar el punto de enfoque (altura de la cabeza)
        Vector3 focusPoint = player.transform.position + Vector3.up * 1.5f; // 1.5f es la altura del torso/cabeza

        Vector3 desiredPosition = focusPoint + rotatedOffset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.LookAt(focusPoint);
    }

    if (hudTimer > 0)
    {
        hudTimer -= Time.deltaTime;
    }
}


    void OnGUI()
    {
        if (hudTimer > 0)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = 20,
                normal = { textColor = Color.white },
                fontStyle = FontStyle.Bold
            };

            string modeText = isFirstPerson ? "First Person Mode" : "Third Person Mode";
            GUI.Label(new Rect(10, 10, 300, 30), modeText, style);
        }
    }
}
