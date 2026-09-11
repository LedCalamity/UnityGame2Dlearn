using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [SerializeField] GameObject player_prefab;
    [SerializeField] CinemachineVirtualCamera virtual_camera;

    IEnumerator Start()
    {
        GameObject player = Instantiate(player_prefab);

        PlayerControlGroundPound ground_pound = player.GetComponent<PlayerControlGroundPound>();
        ground_pound.SetUnlocked(SaveManager.Instance.GroundPoundUnlocked);
        PlayerControlGravityReverse gravity_reverse = player.GetComponent<PlayerControlGravityReverse>();
        Transform player_follow_target = player.transform;
        if(gravity_reverse != null)
        {
            gravity_reverse.SetUnlocked(SaveManager.Instance.GravityReverseUnlocked);
            player_follow_target = gravity_reverse.CenterAnchor;
        }

        CameraFocusController camera_focus = virtual_camera.GetComponent<CameraFocusController>();
        if(camera_focus != null)
        {
            camera_focus.SetPlayerFollowTarget(player_follow_target);
        }
        else
        {
            virtual_camera.Follow = player_follow_target;
        }
        Playerhp.Instance.SetMaxHealth(SaveManager.Instance.MaxHP, true);
        BulletData.Instance.SetBulletDamage("Bullet", SaveManager.Instance.BulletDamage);

        yield return null;
        SceneMgr.Instance.LoadSelectedLevel();
    }
}
