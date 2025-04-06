using UnityEngine;

public class TreasureItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            player.AddTreasure();
            Destroy(gameObject); // Destruir el tesoro
        }
    }
}
