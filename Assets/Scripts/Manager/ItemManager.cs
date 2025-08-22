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

    private bool isActive = false;
    private UnityEvent script;      // 외부에서 호출할 때 할당 함
    private bool isReusable;

    void Awake()
    {
        if (!_instance) _instance = this;
        else DestroyImmediate(this);
        instance = _instance;
    }
    /// <summary>
    /// isActive : 사용 가능 여부 결정. false 인 경우 즉시 return 하여 아래의 키다운을 인식하지 못하도록 함
    /// isReuable : 재사용 가능시에는 ItemSprite를 변경하지 않지만, 재사용 불가시에는 ItemSprite를 제거
    /// 재사용 가능시에도 UI를 띄우거나 아니면 아이템 효과가 어느정도 지나야 가능한 것들이 있기 때문에 일단 isActive를 false로 변경
    /// 
    /// "게이트 키퍼"라는 방식을 사용한 것으로, 궁극적인 디자인 패턴은 "전략 패턴"을 사용해 효과들을 각각 부품으로 만들어서 조립하는 형태
    /// 다만, 어짜피 구현해야 하는 아이템 수는 적고 한정적이니 게이트 키퍼로 필요조건을 Manager에서 확인하도록 함
    /// </summary>
    void Update()
    {
        if (!isActive) { return; }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isActive = false;
            if (!isReusable)
            {
                itemImgUI.sprite = null;
                itemImgUI.color = new Color(itemImgUI.color.r, itemImgUI.color.g, itemImgUI.color.b, 0);
            }
            script.Invoke();
        }
    }
    /// <summary>
    /// Item을 다시 사용가능하게 만드는 함수 
    /// 호출시 IsActive를 true로 만듦
    /// UI를 띄우는 아이템등을 위한 것
    /// </summary>
    public void ItemIsActiveSetting()
    {
        isActive = true;
    }

    /// <summary>
    /// 아이템 획득시 해당 함수를 호출
    /// 반드시 아이템 획득시에 어떤 Script를 실행할 것인지 할당해 주어야 함
    /// 사용 아이템일 경우 인벤토리에 띄울 Sprite 이미지를 할당해주고 IsAutoUsing을 해제해 주어야 함.
    /// </summary>
    /// <param name="script"></param>
    /// <param name="itemSprite"></param>
    /// <param name="isAutoUsing"></param>
    public void ItemGet(UnityEvent script, Sprite itemSprite = null, bool isAutoUsing = true, bool isReusable = false)
    {
        if (isAutoUsing) { script.Invoke(); return; }
        this.script = script;
        isActive = true;
        itemImgUI.sprite = itemSprite;  // 전달 받은 이미지 할당
        itemImgUI.color = new Color(itemImgUI.color.r, itemImgUI.color.g, itemImgUI.color.b, 1); // alpha 값 설정

        // 즉시 사용 아이템이 아닐 경우 = isAutoUsing이 false일 경우
        // 아이템 인벤토리는 1개이기 때문에 기존에 먹은 것을 밀어냄
        // 따라서 재사용가능을 알리는 isReusable을 갱신
        if (!isAutoUsing)
        {
            this.isReusable = isReusable;
        }
    }
}
