using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Animator anim; 
    NewCutsceneManager manager;
    bool moveTime = false, endTime = false, cutsceneComponent = true;
    IEnumerator running;
    private void Start()
    {
        if (GetComponent<NewCutsceneManager>() == null)
        {
            cutsceneComponent = false;
        }
        else manager = GetComponent<NewCutsceneManager>();
    }
    public void MoveCamera(CameraElements element)
    {
        if (running != null) StopCoroutine(running);
        running = RunCameraMovement(element);
        StartCoroutine(running);
    }
    IEnumerator RunCameraMovement(CameraElements element)
    {
        if(element.trackObject != null)
        {
            element.movePos = new Vector3(element.trackObject.position.x, element.trackObject.position.y, element.movePos.z);
        }
        element.movePos = new Vector3(element.movePos.x, element.movePos.y, (element.movePos.z == 0) ? -10.0f : element.movePos.z);
        if (element.instant)
        {
            if (element.blackening)
            {
                anim.SetTrigger("Blackening");
                moveTime = false;
                while (true)
                {
                    if (moveTime)
                    {
                        if (element.movingCam == null)
                        {
                            Camera.main.transform.position = element.movePos;
                        }
                        else
                        {
                            element.movingCam.transform.position = element.movePos;
                        }
                        break;
                    }
                    yield return null;
                }
                if (element.progressCutscene && cutsceneComponent)
                {
                    endTime = false;
                    while (true)
                    {
                        if (endTime)
                        {
                            manager.CutsceneProgress();
                            break;
                        }
                        yield return null;
                    }
                }
            }
            else
            {
                if (element.movingCam == null)
                {
                    Camera.main.transform.position = element.movePos;
                }
                else
                {
                    element.movingCam.transform.position = element.movePos;
                }
                if (element.progressCutscene && cutsceneComponent) manager.CutsceneProgress();
            }
        }
        else
        {
            while (true)
            {
                if (element.movingCam == null)
                {
                    if (Vector3.Distance(Camera.main.transform.position, element.movePos) <= 0.15f)
                    {
                        Camera.main.transform.position = element.movePos;
                        break;
                    }
                    else Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, element.movePos, element.moveSpeed * Time.deltaTime);
                }
                else
                {
                    if (Vector3.Distance(element.movingCam.transform.position, element.movePos) <= 0.15f)
                    {
                        element.movingCam.transform.position = element.movePos;
                        break;
                    }
                    else element.movingCam.transform.position = Vector3.Lerp(element.movingCam.transform.position, element.movePos, element.moveSpeed * Time.deltaTime);
                }
                yield return null;
            }
            if (element.progressCutscene && cutsceneComponent)
            {
                manager.CutsceneProgress();
            }
        }
    }
    public void MoveTime()
    {
        moveTime = true;
    }
    public void EndTime()
    {
        endTime = true;
    }
}
[System.Serializable] public struct CameraElements
{
    [Tooltip("ī�޶� �̵���ų ��ġ. Ư���� ������ ������ z�� -10���� �����ȴ�.")] public Vector3 movePos;
    [Tooltip("ī�޶� Ư�� ��ü�� ��ġ�� �̵�. ��� ���� ������ movePos�� z �ܴ̿� ����.")] public Transform trackObject;
    [Tooltip("�̵���ų ī�޶�. ��� ������ �⺻������ ���� ī�޶� �̵���Ų��.")] public Camera movingCam;
    [Space(5)][Tooltip("ī�޶� ��� �̵��ϴ���, �������� ���̸� �̵��ϴ��� ����")] public bool instant;
    public bool blackening;
    public float moveSpeed;
    [HideInInspector] public bool progressCutscene;
}
