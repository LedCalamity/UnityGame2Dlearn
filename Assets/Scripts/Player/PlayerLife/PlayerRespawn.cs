using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public void RespawnPlayerLight()
    {
        GameObject respawnpt = GameObject.FindGameObjectWithTag("SpawnPoint");
        transform.position = respawnpt.transform.position;
        PlayerMana.Instance.ResetMana();
        Playerhp.Instance.ResetHP();
    }
    public void RespawnPlayerTotal()
    {
        SceneMgr.Instance.LoadScene("DeathScene");
    }
}
