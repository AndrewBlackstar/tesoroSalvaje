using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManagement : MonoBehaviour
{
    public void OpenMainMenu()
    {
        GameManager.Instance.OpenMainMenu();
    }
    public void clicSound()
    {
        AudioManager.Instance.PlayFX("start");
    }
}
