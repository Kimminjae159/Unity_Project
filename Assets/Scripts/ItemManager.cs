using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager instance;

    // 각 Level 별로 사용 가능한 item에 대해서 종류는 즉발과 사용. 사용 아이템은 1개까지 보관이 최대라는 가정 하에 짠 코드. 그 이상을 원하면 Queue를 만들어서 보관하던가 해야 함
    [Tooltip("UI_Player 내부의 itemImage UI를 이곳에 할당")]
    public Image itemImgUI;               // ItemImage (UI Image)

    private bool IsActive = false;
    private UnityEvent script;      // 외부에서 호출할 때 할당 함

    void Awake()
    {
        if (!_instance) _instance = this;
        else DestroyImmediate(this);
        instance = _instance;
    }
    void Update()
    {
        if (!IsActive) { return; }
        if (Input.GetKeyDown(KeyCode.I))
        {
            IsActive = false;
            itemImgUI.sprite = null;
            itemImgUI.color = new Color(itemImgUI.color.r, itemImgUI.color.g, itemImgUI.color.b, 0);
            script.Invoke();
        }
    }

    /// <summary>
    /// 아이템 획득시 해당 함수를 호출
    /// 반드시 아이템 획득시에 어떤 Script를 실행할 것인지 할당해 주어야 함
    /// 사용 아이템일 경우 인벤토리에 띄울 Sprite 이미지를 할당해주고 IsAutoUsing을 해제해 주어야 함.
    /// </summary>
    /// <param name="script"></param>
    /// <param name="itemSprite"></param>
    /// <param name="IsAutoUsing"></param>
    public void ItemGet(UnityEvent script, Sprite itemSprite = null, bool IsAutoUsing = true)
    {
        if (IsAutoUsing) { script.Invoke(); return; }
        this.script = script;
        IsActive = true;
        itemImgUI.sprite = itemSprite;  // 전달 받은 이미지 할당
        itemImgUI.color = new Color(itemImgUI.color.r, itemImgUI.color.g, itemImgUI.color.b, 1); // alpha 값 설정
    }
}
