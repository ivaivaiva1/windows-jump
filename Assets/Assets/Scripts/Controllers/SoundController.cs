using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

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
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        sfxDict = new Dictionary<SfxType, SfxEntry>();
        foreach (var entry in sfxList)
        {
            if (!sfxDict.ContainsKey(entry.type))
                sfxDict.Add(entry.type, entry);
        }
    }

    public void PlaySfxOneShot(SfxType type)
    {
        if (sfxDict.TryGetValue(type, out SfxEntry entry))
        {
            float volume = Mathf.Clamp01(entry.volumePercent / 100f);
            audioSource.PlayOneShot(entry.clip, volume);
        }
        else
        {
            Debug.LogWarning($"SFX '{type}' não encontrado!");
        }
    }
}
