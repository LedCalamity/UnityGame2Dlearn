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
        if(gravity_reverse != null)
        {
            gravity_reverse.SetUnlocked(SaveManager.Instance.GravityReverseUnlocked);
            virtual_camera.Follow = gravity_reverse.CenterAnchor;
        }
        else
        {
            virtual_camera.Follow = player.transform;
        }
        Playerhp.Instance.SetMaxHealth(SaveManager.Instance.MaxHP, true);
        BulletData.Instance.SetBulletDamage("Bullet", SaveManager.Instance.BulletDamage);

        yield return null;
        SceneMgr.Instance.LoadSelectedLevel();
    }
}
