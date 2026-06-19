using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("기본 플레이어 추적")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    [Header("마우스 방향 카메라 이동")]
    [SerializeField] private float mouseFocusDistance = 3f;
    [SerializeField] private float mouseFocusSpeed = 5f;

    private Transform target;
    private Vector3 currentMouseOffset;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

       
        Vector3 mouseViewportPosition =Camera.main.ScreenToViewportPoint(Input.mousePosition);  // 마우스 위치를 화면 좌표 기준 0~1로 변환
        Vector2 mouseDirection = new Vector2( mouseViewportPosition.x - 0.5f,mouseViewportPosition.y - 0.5f);   // 화면 중앙을 0, 0으로 변환
        Vector3 desiredMouseOffset = new Vector3(mouseDirection.x,mouseDirection.y,0f) * mouseFocusDistance;   // 마우스 방향으로 이동할 카메라 오프셋
       
        currentMouseOffset = Vector3.Lerp(currentMouseOffset,desiredMouseOffset,mouseFocusSpeed * Time.deltaTime); // 마우스 이동도 부드럽게 처리
        Vector3 targetPosition =target.position + offset + currentMouseOffset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position,targetPosition,followSpeed * Time.deltaTime);
    }


}
