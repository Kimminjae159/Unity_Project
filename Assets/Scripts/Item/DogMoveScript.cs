using UnityEngine;
using System.Collections; // 코루틴 사용을 위해 필요
using System.Text.RegularExpressions; // 정규식을 이용한 숫자 추출을 위해 필요

public class DogMoveScript : MonoBehaviour
{
    // === 인스펙터에서 설정할 변수들 ===
    [Header("Dependencies")]
    public GameObject platformParent; // Correct## 발판들의 부모 오브젝트 (Hierarchy에서 드래그앤드롭)
    public GameObject playerObject; // 플레이어 오브젝트 (Hierarchy에서 드래그앤드롭)

    [Header("Movement Settings")]
    public float jumpHeight = 2.0f; // 점프 시 포물선의 최대 높이
    public float jumpDuration = 1.0f; // 한 발판에서 다음 발판으로 이동하는 데 걸리는 시간
    public float groundCheckDistance = 1f; // 발판 감지를 위한 Raycast 거리
    public LayerMask groundLayer; // 발판들이 속한 Layer (Inspector에서 설정)

    // === 내부 상태 변수들 ===
    private bool isDogMoving = false; // Dog가 현재 이동 중인지 여부
    private GameObject currentPlatform = null; // Dog가 현재 밟고 있는 발판

    // === Dog 오브젝트 초기 설정 (ApplyDog 함수에서 사용) ===
    private Vector3 correct01Position; // Correct01 발판의 위치 저장

    // --- MonoBehaviour LifeCycle Methods ---

    void Awake()
    {
        // Dog 오브젝트가 처음에는 비활성화 상태이므로, Awake에서 Correct01 위치를 미리 찾아둡니다.
        GameObject correct01 = FindPlatformByName("Correct01");
        if (correct01 != null)
        {
            // ApplyDog에서 실제로 사용될 위치를 위해 발판 위로 살짝 띄운 위치를 계산
            correct01Position = correct01.transform.position + Vector3.up * (correct01.GetComponent<Collider>().bounds.extents.y + 0.5f);
        }
        else
        {
            Debug.LogError("Correct01 발판을 찾을 수 없습니다. platformParent가 올바르게 설정되었는지 확인하세요.");
        }

        // Dog 오브젝트는 시작 시 비활성화 상태로 가정
        gameObject.SetActive(false);

        // Dog 오브젝트에 Collider와 Rigidbody가 올바르게 설정되었는지 확인 (경고)
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("Dog 오브젝트에 Collider 컴포넌트가 없습니다! OnTriggerEnter를 사용하려면 필수입니다.");
        }
        else if (!GetComponent<Collider>().isTrigger)
        {
            Debug.LogWarning("Dog 오브젝트의 Collider가 Is Trigger로 설정되어 있지 않습니다. OnTriggerEnter를 사용하려면 Is Trigger를 활성화해야 합니다.");
        }
        if (GetComponent<Rigidbody>() == null)
        {
            Debug.LogWarning("Dog 오브젝트에 Rigidbody 컴포넌트가 없습니다! OnTriggerEnter가 제대로 작동하지 않을 수 있습니다.");
        }
    }

    // Update에서는 더 이상 OverlapSphere로 플레이어 충돌을 직접 확인하지 않습니다.
    // 충돌 감지는 OnTriggerEnter 콜백으로 대체됩니다.
    void Update()
    {
        // Update에서는 현재 발판을 지속적으로 확인하여 Dog가 공중에 뜨지 않도록 할 수 있으나,
        // 이 스크립트에서는 DogMove 코루틴 내에서만 발판을 체크합니다.
        // 필요하다면 여기에 지속적인 'grounded' 로직을 추가할 수 있습니다.
    }

    // --- Collision/Trigger Callbacks ---

    // Dog 오브젝트의 Collider가 다른 Collider의 Trigger 영역에 진입했을 때 호출됩니다.
    void OnTriggerEnter(Collider other)
    {
        // 1) player object가 자신과 부딪혔는지 매 순간 확인 (이제 OnTriggerEnter가 대신함)
        // 2) player object가 자신과 부딪혔을 경우 DogMove 함수 실행
        if (other.gameObject == playerObject)
        {
            // Dog가 이동 중이 아닐 때만 DogMove를 시도
            if (!isDogMoving)
            {
                Debug.Log("Player entered Dog's trigger. Initiating DogMove.");
                StartCoroutine(DogMove());
            }
        }
    }

    // --- Dog Movement Logic ---

    // Dog를 다음 발판으로 이동시키는 코루틴
    private IEnumerator DogMove()
    {
        isDogMoving = true; // 이동 시작 플래그 설정

        // 1) 현재 밟고 있는 발판 명칭을 가져옴
        // Dog 발 밑으로 Raycast를 쏴서 현재 밟고 있는 발판을 정확히 감지
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            currentPlatform = hit.collider.gameObject;
            Debug.Log($"Dog is currently on platform: {currentPlatform.name}");
        }
        else
        {
            // 발판을 감지하지 못했을 경우 (공중에 있거나, 잘못된 레이어 설정 등)
            Debug.LogWarning("Dog cannot detect a platform beneath it. Is groundLayer set correctly?");
            isDogMoving = false;
            yield break; // 코루틴 종료
        }

        // 4-1) 만약 현재 발판 명칭이 Correct17이면 Dog 오브젝트 사라짐
        if (currentPlatform.name == "Correct17")
        {
            Debug.Log("Dog reached Correct17. Disabling Dog object.");
            gameObject.SetActive(false);
            isDogMoving = false;
            yield break; // 코루틴 종료
        }

        // 2) 해당 발판 명칭에서 Correct 다음 부분의 숫자를 추출, +1한 숫자를 붙임
        int currentPlatformNumber = ExtractNumberFromName(currentPlatform.name);
        int nextPlatformNumber = currentPlatformNumber + 1;

        string nextPlatformName = $"Correct{nextPlatformNumber:D2}"; // 예: Correct01, Correct10

        // 3) 2)에서 생성한 Correct##에 해당하는 object의 좌표를 구함
        GameObject nextPlatform = FindPlatformByName(nextPlatformName);
        if (nextPlatform == null)
        {
            Debug.LogError($"다음 발판 '{nextPlatformName}'을 찾을 수 없습니다. 경로가 올바른지 확인하세요.");
            isDogMoving = false;
            yield break; // 코루틴 종료
        }

        Vector3 startPos = transform.position;
        // 다음 발판의 표면 위로 목표 위치 설정 (발판의 높이를 고려)
        Vector3 endPos = nextPlatform.transform.position + Vector3.up * (nextPlatform.GetComponent<Collider>().bounds.extents.y + 0.5f);

        float timer = 0f;
        while (timer < jumpDuration)
        {
            float progress = timer / jumpDuration; // 0에서 1까지 진행

            // 수평 이동 (Lerp)
            Vector3 currentHorizontalPos = Vector3.Lerp(startPos, endPos, progress);

            // 수직 이동 (포물선)
            // 0 -> 1 -> 0 형태의 곡선을 위한 계산 (sin 함수 사용)
            float parabolicHeight = Mathf.Sin(progress * Mathf.PI) * jumpHeight;

            transform.position = new Vector3(currentHorizontalPos.x, currentHorizontalPos.y + parabolicHeight, currentHorizontalPos.z);

            timer += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 이동 완료 후 정확한 목표 위치로 설정
        transform.position = endPos;

        // 5) 다음 DogMove를 위해 현재 위치를 업데이트
        currentPlatform = nextPlatform;

        isDogMoving = false; // 이동 완료 플래그 해제
        Debug.Log($"Dog arrived at {nextPlatform.name}");
    }

    // --- Dog Activation Logic ---

    // 이 함수가 호출되면 Dog 오브젝트가 활성화되고 Correct01 발판 위에 배치됩니다.
    public void ApplyDog()
    {
        // 1) 이 함수가 호출되면 Disable 상태였던 Dog 오브젝트가 활성화 됨
        gameObject.SetActive(true);

        // 2) Dog 오브젝트는 Correct01 이름의 발판 좌표 위에 생성됨
        // Awake에서 미리 계산해 둔 Correct01 위치를 사용
        transform.position = correct01Position;

        // 초기 발판 설정 (Raycast를 통해 다시 확인할 수도 있지만, ApplyDog 시점에는 Correct01 위에 있다고 가정)
        currentPlatform = FindPlatformByName("Correct01");
        if (currentPlatform == null)
        {
            Debug.LogError("ApplyDog: Correct01 발판을 찾을 수 없습니다. 초기 위치 설정 실패.");
        }
        Debug.Log("Dog activated and placed on Correct01.");
    }

    // --- Helper Functions ---

    // 발판 이름에서 숫자 추출 (예: "Correct05" -> 5)
    private int ExtractNumberFromName(string name)
    {
        Match match = Regex.Match(name, @"Correct(\d+)");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        Debug.LogError($"Failed to extract number from platform name: {name}. Name must be in 'Correct##' format.");
        return 0; // 오류 발생 시 기본값 반환
    }

    // 이름으로 Correct## 발판 찾기 (Platform 오브젝트의 자식 중에서)
    private GameObject FindPlatformByName(string platformName)
    {
        if (platformParent == null)
        {
            Debug.LogError("platformParent가 설정되지 않았습니다. Inspector에서 Platform 오브젝트를 연결해주세요.");
            return null;
        }

        Transform platformTransform = platformParent.transform.Find(platformName);
        if (platformTransform != null)
        {
            return platformTransform.gameObject;
        }
        return null;
    }

    // 개발 편의를 위한 시각화 (기즈모)
    void OnDrawGizmos()
    {
        // Dog의 발 아래 Raycast 시각화
        Gizmos.color = Color.blue;
        // Gizmos.DrawRay는 Ray의 시작점과 방향을 그립니다.
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, Vector3.down * groundCheckDistance);
    }
}