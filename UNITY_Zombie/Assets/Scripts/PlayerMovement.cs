using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    private Vector3 direction;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private Camera worldCam;
    public LayerMask layerMask;

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        worldCam = Camera.main; // 메인카메라 태그 붙은 객체를 반환해줌
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate() {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        Move();
        Rotate();
    }

    private void Update()
    {
        var forward = worldCam.transform.forward; // 앞뒤 움직임
        forward.y = 0f;
        forward.Normalize();

        var right = worldCam.transform.right; // 좌우 움직임
        right.y = 0f;
        right.Normalize();

        direction = forward * playerInput.move;
        direction += right * playerInput.rotate;

        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        playerAnimator.SetFloat("Move", direction.magnitude);
    }

    // 입력값에 따라 캐릭터를 앞뒤로 움직임
    private void Move() {
        var position = playerRigidbody.position;
        position += direction * moveSpeed *Time.deltaTime;
        playerRigidbody.MovePosition(position);
    }

    // 입력값에 따라 캐릭터를 좌우로 회전
    private void Rotate() {
        // a,d 키입력으로 캐릭터 회전
        //var rotation = playerRigidbody.rotation;
        //rotation *= Quaternion.Euler(Vector3.up * playerInput.rotate * rotateSpeed * Time.deltaTime); // up이 y축

        // 플레이어가 마우스를 바라보도록 회전(레이캐스트)
        var ray = worldCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f, layerMask)) //  , 검사할 거리, 충돌체크할 레이어
        {
            Vector3 lookPoint = hitInfo.point; // 충돌지점
            lookPoint.y = transform.position.y; // 플레이어의 y좌표와 동화
            var look = lookPoint - playerRigidbody.position;
            playerRigidbody.MoveRotation(Quaternion.LookRotation(look.normalized)); // 벡터 만들고 노말라이즈
        }

        //playerRigidbody.MoveRotation(rotation);
    }
}