using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Interaction
{
    public Transform[] Position;
    public float Speed = 0.5f;
    public float launchSpeed = 0.002f;
    private float _t = 0f;
    private Rigidbody2D R;
    private GameObject Character;
    private bool _isMoving = false;

    private int _currentPositionIndex = 0;

    Monkey monkey;
    PlayerCharacter p;

    private bool _hasStartedBezierCurve = false;
    private Vector2 _lastBezierDirection; // 마지막 베지어 곡선 이동 방향 저장
    private float _originalGravityScale; // 원래 중력 스케일 저장



    public override void Start()
    {
        base.Start();
        requireType = true;
        requiredType = AnimalType.monkey;
        monkey = (Monkey)GameManager.Instance.animals[1];
        Character = GameManager.Instance.player;

        // 게임 오브젝트의 Rigidbody2D 컴포넌트가 있다면 중력 스케일 저장
        R = Character.GetComponent<Rigidbody2D>();
        _originalGravityScale = R.gravityScale;
    }

    // 다른 이벤트나 조건에 따라 베지어 곡선 이동을 시작하는 메서드
    public void StartBezierCurve()
    {
        if (!_hasStartedBezierCurve)
        {
            interaction.RequestInteract(this);
        }
    }

    private IEnumerator BezierCurveStart()
    {
        if (_isMoving) yield break;
        _isMoving = true;
        _currentPositionIndex = 0;
        GameManager.Instance.M_PlayerCharacter.canChange = false;
        GameManager.Instance.M_PlayerMovements.canMove = false;

        Vector2 currentPosition = Character.transform.position;

        int endIndex = (_currentPositionIndex + 1) % Position.Length;

        Vector2[] positions = new Vector2[4];
        positions[0] = Position[_currentPositionIndex].position;
        positions[1] = Position[_currentPositionIndex + 1].position; // 다음 위치를 제어점으로 설정
        positions[2] = Position[endIndex].position;
        positions[3] = Position[(endIndex + 1) % Position.Length].position; // 다음 다음 위치를 제어점으로 설정

        // 중력을 베지어 곡선 중에 끔
        Rigidbody2D rb = Character.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
        }

        while (_t < 1f)
        {
            _t += Time.deltaTime * Speed;

            Vector2 curvePosition = CalculateBezierPoint(_t, positions[0], positions[1], positions[2], positions[3]);
            Vector2 targetDirection = (curvePosition - currentPosition).normalized;

            R.velocity = targetDirection * 3f;
            Character.transform.position = curvePosition;

            // 베지어 곡선 이동 방향 저장
            _lastBezierDirection = targetDirection;

            yield return null;
        }

        _isMoving = false;
        _t = 0f;

        // 현재 위치를 루프되도록 업데이트
        _currentPositionIndex = endIndex;

        // 베지어 곡선 이동이 끝나면 해당 방향으로 날아가도록 호출
        LaunchInDirection(_lastBezierDirection);

        // 중력을 원래대로 돌림
        if (rb != null)
        {
            rb.gravityScale = _originalGravityScale;
        }

        yield return null;
    }

    private Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1f - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector2 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    // 해당 방향으로 캐릭터를 날아가게 하는 메서드
    private void LaunchInDirection(Vector2 direction)
    {

        // 중력 스케일을 원래대로 복원합니다.
        R.gravityScale = _originalGravityScale;

        // 힘을 계산하고 적용합니다.
        Vector2 force = direction.normalized * launchSpeed;
        R.AddForce(force, ForceMode2D.Impulse);
        GameManager.Instance.M_PlayerMovements.antiAirMove = true;
        GameManager.Instance.M_PlayerCharacter.canChange = true;
        GameManager.Instance.M_PlayerMovements.canMove = true;
    }

    public override void Interact()
    {

        base.Interact();
        _hasStartedBezierCurve = true;
        StartCoroutine(BezierCurveStart());
    }
}
