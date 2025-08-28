using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionManager : MonoBehaviour
{
    [Header("옵션 창의 BGM Slider bar를 할당")]
    [SerializeField] private Slider bgmSlider;

    [Header("Project의 AudioMixer를 할당")]
    // 인스펙터에서 오디오 믹서를 연결해 주세요.
    public AudioMixer masterMixer;

    // 인스펙터에서 SFX 볼륨 조절용 슬라이더를 연결해 주세요.
    [Header("옵션 창의 SFX Slider bar를 할당")]
    public Slider sfxSlider;

    

    void Awake()
    {
        gameObject.SetActive(false);
    }

    void Start()
    {
        // 슬라이더 초기값 설정
        if (BGMManager.instance != null)
        {
            bgmSlider.value = BGMManager.instance.GetCurrentVolume();
        }

        // 슬라이더 값 변경 리스너 추가
        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);


        if (masterMixer.GetFloat("SFXVolume", out float dbValue))
        {
            // 💡 중요: 데시벨 값을 슬라이더 값(0.0 ~ 1.0)으로 다시 변환합니다.
            // 이는 Mathf.Log10(sliderValue) * 20 의 역산입니다.
            // sliderValue = 10^(dbValue / 20)
            float sliderValue = Mathf.Pow(10f, dbValue / 20f);

            // 슬라이더의 값을 계산된 값으로 설정합니다.
            sfxSlider.value = sliderValue;
        }
    }

    // SFX 볼륨을 조절하는 함수
    // 이 함수를 UI 슬라이더의 OnValueChanged 이벤트에 연결하면 됩니다.
    public void SetSfxVolume(float sliderValue)
    {
        // 💡 중요: 오디오 믹서의 볼륨은 데시벨(dB) 단위입니다.
        // 슬라이더 값(0.0 ~ 1.0)을 데시벨(-80dB ~ 0dB)로 변환해야 합니다.
        // 슬라이더 값이 0이면 소리가 나지 않도록 -80dB로 설정합니다.
        float volume = (sliderValue == 0) ? -80f : Mathf.Log10(sliderValue) * 20;

        // "SFXVolume"은 3단계에서 설정한 노출된 파라미터의 이름입니다.
        masterMixer.SetFloat("SFXVolume", volume);
    }

    private void OnBgmSliderChanged(float value)
    {
        if (BGMManager.instance != null)
        {
            BGMManager.instance.SetVolume(value);
        }
    }

    // Option창의 Confirm Button에 할당
    public void OnClickConfirm()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        }
    }
}