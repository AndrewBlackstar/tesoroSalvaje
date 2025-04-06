using UnityEngine;

public class MenuManagement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic("Menu");  
    }

    public void clicSound()
    {
        AudioManager.Instance.PlayFX("click");
    }

    public void playertakedamageSound()
    {
        AudioManager.Instance.PlayFX("playergolpe");
    }

        public void enemytakedamageSound()
    {
        AudioManager.Instance.PlayFX("enemygolpe");
    }

        public void playerwalkSound()
    {
        AudioManager.Instance.PlayFX("walk1");
    }

          public void enemywalkSound()
    {
        AudioManager.Instance.PlayFX("walk");
    }

          public void winSound()
    {
        AudioManager.Instance.PlayFX("Win");
    }

              public void loseound()
    {
        AudioManager.Instance.PlayFX("lose");
    }

    public void OpenOptions() 
    {
        GameManager.Instance.OpenOptionsMenu();
    }

    public void SetMusicVolume(float volume)
    {
        if (AudioManager.Instance.audioMixer != null)
        {
            // Convierte el valor lineal del slider a logar�tmico para el AudioMixer
            AudioManager.Instance.audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("MusicVolume", volume); // Guarda el valor
        }
    }

    public void SetFxVolume(float volume)
    {
        if (AudioManager.Instance.audioMixer != null)
        {
            // Convierte el valor lineal del slider a logar�tmico para el AudioMixer
            AudioManager.Instance.audioMixer.SetFloat("Fx", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("FxVolume", volume); // Guarda el valor
        }
    }

    public void SetGeneralVolume(float volume)
    {
        if (AudioManager.Instance.audioMixer != null)
        {
            // Convierte el valor lineal del slider a logar�tmico para el AudioMixer
            AudioManager.Instance.audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("GeneralVolume", volume); // Guarda el valor
        }
    }

    public void OpenCredits()
    {
        GameManager.Instance.OpenCreditsMenu();
    }
    public void OpenControls()
    {
        GameManager.Instance.OpenControlsMenu();
    }

    public  void NextLevel()
    {
        GameManager.Instance.LoadNextScene();
    }
    
}
