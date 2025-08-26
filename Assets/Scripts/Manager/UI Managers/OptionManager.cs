using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [Header("옵션 창의 BGM Slider bar를 할당")]
    [SerializeField] private Slider bgmSlider;

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