using UnityEngine;

public class TriggerDropDeath : MonoBehaviour
{
    GameObject death_player;
    void Start()
    {
        death_player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            death_player.GetComponent<PlayerDeath>().PlayerDie();
        }
    }
}
