using System.Collections;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraFocusController : MonoBehaviour
{
    CinemachineVirtualCamera virtual_camera;
    Transform player_follow_target;
    Transform focus_target;
    Coroutine focus_task;

    void Awake()
    {
        virtual_camera = GetComponent<CinemachineVirtualCamera>();
        focus_target = new GameObject("CameraFocusTarget").transform;
        DontDestroyOnLoad(focus_target.gameObject);
    }

    void OnDestroy()
    {
        if(focus_target != null)
        {
            Destroy(focus_target.gameObject);
        }
    }

    public void SetPlayerFollowTarget(Transform player_target)
    {
        player_follow_target = player_target;
        if(focus_task == null)
        {
            virtual_camera.Follow = player_follow_target;
        }
    }

    public void FocusAt(Vector3 position, float duration)
    {
        if(focus_task != null)
        {
            StopCoroutine(focus_task);
        }

        focus_target.position = position;
        focus_task = StartCoroutine(FocusTask(duration));
    }

    IEnumerator FocusTask(float duration)
    {
        virtual_camera.Follow = focus_target;
        yield return new WaitForSeconds(duration);
        virtual_camera.Follow = player_follow_target;
        focus_task = null;
    }
}
