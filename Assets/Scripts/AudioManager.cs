using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource fxSource;
    private AudioSource musicSource;

    [SerializeField] public AudioMixer audioMixer;

    // Clips organizados por tipo
    public AudioClip footstepClip;
    public AudioClip hitClip;
    public AudioClip hurtClip;
    public AudioClip menuClip;
    public AudioClip winClip;
    public AudioClip loseClip;

    private Dictionary<string, AudioClip> fxClips;

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

        // Inicializamos el diccionario
        fxClips = new Dictionary<string, AudioClip>
        {
            { "Footstep", footstepClip },
            { "Hit", hitClip },
            { "Hurt", hurtClip },
            { "Menu", menuClip },
            { "Win", winClip },
            { "Lose", loseClip }
        };
    }

    // 🔊 Reproduce un efecto según su nombre clave
    public void PlayFX(string clipKey)
    {
        if (fxClips.ContainsKey(clipKey) && fxClips[clipKey] != null)
        {
            fxSource.PlayOneShot(fxClips[clipKey]);
        }
        else
        {
            Debug.LogWarning($"No se encontró el sonido FX con clave: {clipKey}");
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

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
