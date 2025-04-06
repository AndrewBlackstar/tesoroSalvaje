using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;

    [Header("Altura de la cámara")]
    public float height = 20f;

    [Header("Suavizado de movimiento (opcional)")]
    public float smoothSpeed = 10f;

    void LateUpdate()
    {
        if (target == null) return;

        // Calculamos la nueva posición deseada
        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + height, target.position.z);

        // Movimiento suave (opcional)
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Aseguramos que la cámara mire directamente hacia abajo
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
