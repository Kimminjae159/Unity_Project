using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance = null;
    private AudioSource audioSource;

    private const string BGM_VOLUME_KEY = "BgmVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            LoadVolume();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, audioSource.volume);
        }
    }

    private void LoadVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.8f); // 기본 볼륨 80%
        SetVolume(savedVolume);
    }
    
    public float GetCurrentVolume()
    {
        return (audioSource != null) ? audioSource.volume : 1.0f;
    }
}