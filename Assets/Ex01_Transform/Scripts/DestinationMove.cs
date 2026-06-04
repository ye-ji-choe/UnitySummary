using UnityEngine;
using UnityEngine.InputSystem;

public class DestinationMove : MonoBehaviour
{
    public LayerMask detectLayer;       //감지하고 싶은 레이어
    public float moveSpeed = 1f;        //이동속도
    public float rotateSpeed = 360f;    //회전속도

    private bool isPressed = false;     //버튼이 눌러져 있는지 확인
    private Vector3 destination;        //목적지
    private Quaternion toward;          //목적지를 바라보는 방향


    //Pick 액션이 발동될 때 호출될 콜백 함수
    public void OnPick(InputValue value)//값을 확인하려면 확인할 값을 ()에 넣어주기
    {
        isPressed = value.isPressed;
        Debug.Log($"마우스 왼쪽 버튼 => {isPressed}");
    }

    private void Update()
    {
        //마우스 왼쪽 버튼이 눌러져 있는 상태에서만 발동
        if(isPressed)
        {
            //카메라 기준에서 마우스 포인터의 위치에 Ray 생성
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            
            //생성된 Ray를 발사해 위치 확인
            if(Physics.Raycast(ray, out RaycastHit hit, 100f, detectLayer))//레이가 어떤 콜라이더에 히트되면 히트 변수에 위치를 넣기
            {
                destination = hit.point;
                //목적지 - 현재 위치 => 목적지를 향한 벡터값을 알아낼 수 있다
                Vector3 direction = destination - transform.position;
                //현재 위치가 목적지와 달라졌다면 목적 방향에 대입
                if(Vector3.SqrMagnitude(direction) > 0.001f)
                {
                    //LookRotation => 방향 단위 벡터를 넣어주면 그 방향으로 회전해야하는 회전값을 계산해서 알려줌
                    toward = Quaternion.LookRotation(direction.normalized, Vector3.up);
                }
            }
        }
        //현재 위치에서 목표 위치를 향해 이동속도만큼 이동한 결과 위치를 계산해서 알려줌
        Vector3 position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime); //오버해서 넘어가지 않음, 정확한 목적지에 도착
        //현재 방향에서 목표 방향을 향해 회전 속도만큼 회전한 결과 방향을 게산해서 알려줌
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, toward, rotateSpeed * Time.deltaTime);

        //transform.position = position;
        //transform.rotation = rotation;
        //이동할 위치와 방향으로 적용
        transform.SetPositionAndRotation(position, rotation);

    }

}
