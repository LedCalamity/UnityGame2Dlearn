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
        virtual_camera.Follow = player.transform;

        PlayerControlGroundPound ground_pound = player.GetComponent<PlayerControlGroundPound>();
        ground_pound.SetUnlocked(SaveManager.Instance.GroundPoundUnlocked);

        yield return null;
        SceneMgr.Instance.LoadSelectedLevel();
    }
}
