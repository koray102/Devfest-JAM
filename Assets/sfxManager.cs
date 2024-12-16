using System.Collections.Generic;
using UnityEngine;

public class sfxManager : MonoBehaviour
{
    public static sfxManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name;            // Effect name
        public AudioClip clip;         // AudioClip for the sound
        [Range(0f, 1f)] public float volume = 1f; // Volume level
        [Range(0.1f, 3f)] public float pitch = 1f; // Pitch level
    }

    public Sound[] sounds; // Array of sounds
    private Dictionary<string, Sound> soundDictionary;
    private Dictionary<GameObject, Dictionary<string, AudioSource>> objectSoundSources;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize dictionaries
        soundDictionary = new Dictionary<string, Sound>();
        objectSoundSources = new Dictionary<GameObject, Dictionary<string, AudioSource>>();
        foreach (var sound in sounds)
        {
            soundDictionary[sound.name] = sound;
        }
    }

    public void PlaySFX(string soundName, GameObject ownerObject)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            Sound sound = soundDictionary[soundName];

            // Create a temporary AudioSource for this object and sound
            GameObject soundObject = new GameObject($"SFX_{soundName}_{ownerObject.name}");
            soundObject.transform.position = ownerObject.transform.position;
            soundObject.transform.parent = ownerObject.transform; // Attach to the owner

            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 20f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;

            audioSource.Play();

            Destroy(soundObject, sound.clip.length / sound.pitch); // Destroy after playback
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void PlaySFXOnLoop(string soundName, GameObject ownerObject)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            if (!objectSoundSources.ContainsKey(ownerObject))
            {
                objectSoundSources[ownerObject] = new Dictionary<string, AudioSource>();
            }

            if (objectSoundSources[ownerObject].ContainsKey(soundName))
            {
                Debug.LogWarning($"Sound '{soundName}' is already looping for {ownerObject.name}!");
                return;
            }

            Sound sound = soundDictionary[soundName];

            // Create a looping AudioSource
            GameObject soundObject = new GameObject($"LoopingSFX_{soundName}_{ownerObject.name}");
            soundObject.transform.position = ownerObject.transform.position;
            soundObject.transform.parent = ownerObject.transform; // Attach to the owner

            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.spatialBlend = 1f;
            audioSource.loop = true;

            audioSource.Play();
            objectSoundSources[ownerObject][soundName] = audioSource;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }

    public void StopSFX(string soundName, GameObject ownerObject)
    {
        if (objectSoundSources.ContainsKey(ownerObject) && objectSoundSources[ownerObject].ContainsKey(soundName))
        {
            AudioSource audioSource = objectSoundSources[ownerObject][soundName];

            if (audioSource != null)
            {
                Debug.Log($"Stopping sound '{soundName}' for {ownerObject.name}");
                audioSource.Stop();
                Destroy(audioSource.gameObject);
            }
            else
            {
                Debug.LogWarning($"AudioSource for '{soundName}' is null for {ownerObject.name}!");
            }

            objectSoundSources[ownerObject].Remove(soundName);

            if (objectSoundSources[ownerObject].Count == 0)
            {
                objectSoundSources.Remove(ownerObject);
            }
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found or not playing for {ownerObject.name}!");
        }
    }



    public void SetFXVolume(string soundName, GameObject ownerObject, float volume)
    {
        if (objectSoundSources.ContainsKey(ownerObject) && objectSoundSources[ownerObject].ContainsKey(soundName))
        {
            objectSoundSources[ownerObject][soundName].volume = Mathf.Clamp(volume, 0f, 1f);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found for {ownerObject.name}!");
        }
    }

    public void SetFXPitch(string soundName, GameObject ownerObject, float pitch)
    {
        if (objectSoundSources.ContainsKey(ownerObject) && objectSoundSources[ownerObject].ContainsKey(soundName))
        {
            objectSoundSources[ownerObject][soundName].pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found for {ownerObject.name}!");
        }
    }

    public void SetFXBlend(string soundName, GameObject ownerObject, float spatialBlend)
    {
        if (objectSoundSources.ContainsKey(ownerObject) && objectSoundSources[ownerObject].ContainsKey(soundName))
        {
            objectSoundSources[ownerObject][soundName].spatialBlend = Mathf.Clamp(spatialBlend, 0f, 1f);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found for {ownerObject.name}!");
        }
    }

    public void UpdateSFXPosition(GameObject ownerObject)
    {
        if (objectSoundSources.ContainsKey(ownerObject))
        {
            foreach (var audioSource in objectSoundSources[ownerObject].Values)
            {
                audioSource.transform.position = ownerObject.transform.position;
            }
        }
    }

    public bool IsSFXPlaying(string soundName, GameObject ownerObject)
    {
        return objectSoundSources.ContainsKey(ownerObject) && objectSoundSources[ownerObject].ContainsKey(soundName) && objectSoundSources[ownerObject][soundName].isPlaying;
    }
}
