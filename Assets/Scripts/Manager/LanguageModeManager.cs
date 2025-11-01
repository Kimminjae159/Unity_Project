using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
    Option Panel은 Option Manager에서 Awake로 비활성화 함
    Title에서 Option을 활성화시켰을 때, 자동으로 Option Panel(그리고 자식들)의 컴포넌트로 존재하는 Script의 Start가 시작됨
    이를 이용하여 여기서 Start로 지정해도 큰 문제 없음
*/ 

public class LanguageModeManager : MonoBehaviour
{
    public TMP_Text languageTextField;
    List<string> languageModes = new List<string>();
    int ModeCounts;
    int currentMode;

    // 언어를 추가할거라면 여기에 추가하세요. 
    void Start()
    {
        languageModes.Add("한국어");    // 한국어 : index = 0
        languageModes.Add("English");  // English : index = 1
        ModeCounts = languageModes.Count;
        currentMode = GameManager.instance.language; // Title이 로드될 때 기본 값 = GameManager에 설정된 값(초기엔 0)
        languageTextField.text = languageModes[currentMode];
    }

    // currentMode -= 1
    public void OnLangChangeLeftBtn()
    {
        // 0 이하로 내려가면 List에 마지막으로 저장된 언어의 인덱스로 초기화
        if (currentMode <= 0)
        {
            SetLanguageMode(ModeCounts - 1);
        }
        else
        {
            SetLanguageMode(currentMode - 1);
        }
    }
    // currentMode += 1
    public void OnLangChangeRightBtn()
    {
        // List에 저장된 언어 갯수보다 더 많을 경우 0으로 초기화
        if (currentMode >= ModeCounts - 1)
        {
            SetLanguageMode(0);
        }
        else
        {
            SetLanguageMode(currentMode + 1);
        }
    }
    public void SetLanguageMode(int mode)
    {
        currentMode = mode;
        GameManager.instance.languageSet(currentMode);
        languageTextField.text = languageModes[currentMode];
    }
}
