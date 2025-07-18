using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [SerializeField] private AudioClip sky_music;

    public enum SfxType
    {
        AgarrandoJanela,
        MovimentoNegado,
        Pause,
        PegandoMoeda,
        Pulo,
        QuicandoPlataforma,
        QuicandoInimigo,
        SoltandoJanela,
        SomDoSapo
    }

    [System.Serializable]
    public class SfxEntry
    {
        public SfxType type;
        public AudioClip clip;
        [Range(0, 100)]
        public int volumePercent = 100;
    }

    public List<SfxEntry> sfxList;
    private Dictionary<SfxType, SfxEntry> sfxDict;

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    void Awake()
    {
        Instance = this;

        sfxDict = new Dictionary<SfxType, SfxEntry>();
        foreach (var entry in sfxList)
        {
            if (!sfxDict.ContainsKey(entry.type))
                sfxDict.Add(entry.type, entry);
        }
    }

    private void Start()
    {
        PlayMusic(sky_music, 0.1f);
    }

    public void PlaySfxOneShot(SfxType type)
    {
        if (sfxDict.TryGetValue(type, out SfxEntry entry))
        {
            float volume = Mathf.Clamp01(entry.volumePercent / 100f);
            sfxSource.PlayOneShot(entry.clip, volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{type}' não encontrado!");
        }
    }

    public void PlayMusic(AudioClip musicClip, float volume = 1f)
    {
        if (musicClip == null)
        {
            Debug.LogWarning("Música nula passada para PlayMusic.");
            return;
        }

        musicSource.clip = musicClip;
        musicSource.volume = Mathf.Clamp01(volume);
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
