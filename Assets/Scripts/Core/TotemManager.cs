using UnityEngine;

public class TotemManager : MonoBehaviour
{
    float rotSpeed = 1f;

   

    void Update()
    {
        transform.Rotate(0, 0, rotSpeed);
    }
}
