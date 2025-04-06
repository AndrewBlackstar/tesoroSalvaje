using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource fxSource;
    private AudioSource musicSource;

    [SerializeField] public AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        // Inicializa las fuentes de audio
        fxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();

        // Cargar el Mixer
        audioMixer = Resources.Load<AudioMixer>("AudioMaster");

        if (audioMixer != null)
        {
            fxSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Fx")[0];
            musicSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        }
        else
        {
            Debug.LogWarning("El mixer de audio no se pudo cargar.");
        }
    }

        public void PlayFX(string clipName)
    {
        string path = $"Audio/Fx/{clipName}"; // Ruta dentro de la carpeta Resources
        AudioClip clip = Resources.Load<AudioClip>(path); // Carga el archivo .wav
        if (clip != null)
        {
            fxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"No se encontró el archivo de audio: {path}");
        }
    }

    // 🎵 Reproduce música
    public void PlayMusic(string clipName, bool loop = true)
    {
        string path = $"Audio/Music/{clipName}";
        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"No se encontró el archivo de música: {path}");
        }
    }

}