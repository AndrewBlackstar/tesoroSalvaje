using UnityEngine;

public class MinimapIconFollower : MonoBehaviour
{
    public Transform targetToFollow; // jugador o enemigo
    public RectTransform minimapPanel;
    public float mapWidthWorld = 50f;  // tamaño del mundo en Unity
    public float mapHeightWorld = 50f;
    public float panelWidth = 200f;    // tamaño del panel de mapa en UI
    public float panelHeight = 200f;

    private RectTransform pointer;

    void Start()
    {
        pointer = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (targetToFollow == null) return;

        Vector3 localPos = targetToFollow.position;

        // Normalizar coordenadas
        float normX = localPos.x / mapWidthWorld;
        float normY = localPos.z / mapHeightWorld;

        float uiX = normX * panelWidth - panelWidth / 2;
        float uiY = normY * panelHeight - panelHeight / 2;

        pointer.anchoredPosition = new Vector2(uiX, uiY);
    }
}
