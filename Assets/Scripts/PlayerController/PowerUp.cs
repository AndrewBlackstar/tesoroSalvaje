using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string powerName; // Ej: "SpeedBoost", "JumpBoost"

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.ActivatePower(powerName);
            Destroy(gameObject); // Destruir el power-up
        }
    }
}
