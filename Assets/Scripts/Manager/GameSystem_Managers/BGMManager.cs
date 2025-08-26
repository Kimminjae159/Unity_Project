using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance = null;

    [System.Serializable]
    public struct Sound
    {
        public string sceneName;
        public AudioClip bgmClip;
    }

    [SerializeField]
    private Sound[] bgmSounds;

    private AudioSource audioSource;
    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private const string BGM_VOLUME_KEY = "BgmVolume";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();

            foreach (Sound sound in bgmSounds)
            {
                bgmDictionary.Add(sound.sceneName, sound.bgmClip);
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
            
            // 볼륨 로드 추가
            LoadVolume(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (bgmDictionary.TryGetValue(scene.name, out AudioClip newClip))
        {
            if (audioSource.clip != newClip)
            {
                audioSource.clip = newClip;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    // --- 볼륨 조절 관련 함수들 (여기가 누락되었을 가능성이 높습니다!) ---

    /// <summary>
    /// 외부에서 볼륨을 설정하는 함수 (0.0f ~ 1.0f)
    /// </summary>
    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(BGM_VOLUME_KEY, audioSource.volume);
        }
    }

    /// <summary>
    /// 저장된 볼륨 값을 불러오는 함수
    /// </summary>
    private void LoadVolume()
    {
        // 저장된 값이 없으면 기본값 0.8f (80% 볼륨) 사용
        float savedVolume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 0.8f); 
        SetVolume(savedVolume);
    }

    /// <summary>
    /// 현재 볼륨 값을 반환하는 함수 (UI 초기화에 사용)
    /// </summary>
    public float GetCurrentVolume()
    {
        return (audioSource != null) ? audioSource.volume : 1.0f;
    }
}