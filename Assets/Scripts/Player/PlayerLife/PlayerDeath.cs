using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    int cur_death = 0, max_death = 3;
    PlayerRespawn respawnmr;
    void Start()
    {
        respawnmr = GetComponent<PlayerRespawn>();
    }
    public void PlayerDie() //logic of death? no it's a coroutine
    {
        StartCoroutine(DieRoutine());
    }
    IEnumerator DieRoutine() //the logic of death
    {
        cur_death++;
        if (cur_death >= max_death)
        {
            //below are complete death (death scene loading etc)
            respawnmr.RespawnPlayerTotal();
        }
        else respawnmr.RespawnPlayerLight();
        yield return null;
    }
}
