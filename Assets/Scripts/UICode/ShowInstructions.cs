using UnityEngine;
using UnityEngine.UI;

public class ShowInstructions : MonoBehaviour
{
    public Text messageText; // Asigna el texto desde el Inspector

    void Start()
    {
        messageText.gameObject.SetActive(false); // Ocultar el mensaje al inicio
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageText.gameObject.SetActive(true); // Mostrar el mensaje cuando el jugador entra en la zona
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            messageText.gameObject.SetActive(false); // Ocultar el mensaje cuando el jugador sale de la zona
        }
    }
}

