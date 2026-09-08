using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public void RespawnPlayerLight()
    {
        GameObject respawnpt = GameObject.FindGameObjectWithTag("SpawnPoint");
        transform.position = respawnpt.transform.position;
        PlayerControlGravityReverse gravity_reverse = GetComponent<PlayerControlGravityReverse>();
        if(gravity_reverse != null) gravity_reverse.ResetGravityState();
        PlayerMana.Instance.ResetMana();
        Playerhp.Instance.ResetHP();
    }
    public void RespawnPlayerTotal()
    {
        SceneMgr.Instance.LoadScene("DeathScene");
    }
}
