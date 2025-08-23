using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField]
    private Slider bgmSlider;

    // 옵션 패널 오브젝트 자체 (닫기 버튼에 사용)
    [SerializeField]
    private GameObject optionsPanel; 

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
    
    // 닫기 버튼에 연결할 함수
    public void OnClick_CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveListener(OnBgmSliderChanged);
        }
    }
}