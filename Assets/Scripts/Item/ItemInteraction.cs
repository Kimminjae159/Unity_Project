using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events; // UnityEvent를 사용하기 위해 필요

public class ItemInteraction : MonoBehaviour
{
    [Header("아이템 획득시 설정할 UI")]
    public Image itemImageUI; // ItemImage (UI Image)
    public Sprite collectedItemSprite; // TempItemImage_0 Sprite

    [Header("Item 획득여부 표기")]
    public bool itemHave = false; // 아이템 획득 여부

    [Header("아이템 사용 시 실행될 스크립트")]
    public UnityEvent OnItemCollected; // 아이템 사용 시 실행될 이벤트

    private Renderer objectRenderer; // 이 Script가 존재하는 Object의 Render 저장
    private Collider itemCollider; // Item 오브젝트의 Collider (IsTrigger 활성화)


    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        itemCollider = GetComponent<Collider>();
        if (itemCollider == null)
        {
            Debug.LogError("Item 오브젝트에 Collider가 없습니다. IsTrigger가 활성화되어 있는지 확인하세요.");
        }

        // 초기 ItemImage 상태 설정 (Alpha 값 0, Source Image None)
        if (itemImageUI != null)
        {
            itemImageUI.color = new Color(itemImageUI.color.r, itemImageUI.color.g, itemImageUI.color.b, 0);
            itemImageUI.sprite = null; // Source Image를 None으로 설정
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Player 오브젝트와 겹쳤을 때
        if (other.CompareTag("Player") && !itemHave) // "Player" 태그를 가진 오브젝트인지 확인 및 아이템 미획득 상태
        {
            CollectItem();
        }
    }

    void Update()
    {
        // 아이템을 획득했고, 'I' 키를 눌렀을 때
        if (itemHave && Input.GetKeyDown(KeyCode.I))
        {
            UseItem();
        }
    }

    void CollectItem()
    {
        itemHave = true;
        Debug.Log("Item 획득!");

        // ItemImage 업데이트
        if (itemImageUI != null && collectedItemSprite != null)
        {
            itemImageUI.sprite = collectedItemSprite;
            itemImageUI.color = new Color(itemImageUI.color.r, itemImageUI.color.g, itemImageUI.color.b, 1); // Alpha 255 (1.0f)
        }

        // // 아이템과, 기능 수행하는 오브젝트가 별개일 경우 아래의 방식 채택
        // // 아이템 획득 후 충돌 감지 비활성화 또는 오브젝트 비활성화
        // if (itemCollider != null)
        // {
        //     itemCollider.enabled = false; // 더 이상 충돌 감지하지 않음
        // }

        // 지금은 같은 아이템 내에 기능 수행하는 Script가 공존함
        // 따라서 아이템 획득시 아이템 Render와 Collider를 끄는 방식으로 변형
        // Destroy가 안되는 이유는, 코루틴 사용 중이므로 아이템 사용시에 완전히 삭제되도록 변경
        objectRenderer.enabled = false;
        itemCollider.enabled = false;

        // 혹은 gameObject.SetActive(false); 로 아이템 오브젝트 자체를 비활성화할 수도 있습니다.
    }

    void UseItem()
    {
        Debug.Log("Item 사용!");
        itemHave = false; // 아이템 사용 후 다시 미획득 상태로 변경 (재사용이 아니라면)

        // ItemImage 초기화
        if (itemImageUI != null)
        {
            itemImageUI.sprite = null;
            itemImageUI.color = new Color(itemImageUI.color.r, itemImageUI.color.g, itemImageUI.color.b, 0);
        }

        // 특정 스크립트 실행
        OnItemCollected?.Invoke();
    }
}