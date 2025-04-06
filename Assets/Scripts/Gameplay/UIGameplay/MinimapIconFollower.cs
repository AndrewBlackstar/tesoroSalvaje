using UnityEngine;

public class MinimapIconFollower : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;

    [Header("Altura sobre el objetivo (Y)")]
    public float distanceY = 1f;

    [Header("Inclinación adicional en el eje X (rotación)")]
    public float rotationXOffset = 0f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        if (target == null)
        {
            Debug.LogWarning($"No se asignó target al puntero '{name}'");
        }
    }

    void Update()
    {
        if (target == null) return;

        // POSICIÓN: Seguimos en X/Z, con altura ajustable en Y
        Vector3 newPos = target.position;
        newPos.y += distanceY;
        transform.position = newPos;

        // ROTACIÓN: Seguimos forward del target y aplicamos inclinación en X
        Vector3 forward = target.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude > 0.001f)
        {
            Quaternion baseRotation = Quaternion.LookRotation(forward);
            Quaternion xTilt = Quaternion.Euler(rotationXOffset, 0f, 0f);

            transform.rotation = baseRotation * xTilt;
        }
    }
}
