using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [System.Serializable]
    public class SfxEntry
    {
        public string name;
        public AudioClip clip;
    }

    public List<SfxEntry> sfxList;
    private Dictionary<string, AudioClip> sfxDict;
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        // Preenche o dicionário
        sfxDict = new Dictionary<string, AudioClip>();
        foreach (var entry in sfxList)
        {
            if (!sfxDict.ContainsKey(entry.name))
                sfxDict.Add(entry.name, entry.clip);
        }
    }

    public void PlaySfxOneShot(string soundName)
    {
        if (sfxDict.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{soundName}' não encontrado!");
        }
    }
}
