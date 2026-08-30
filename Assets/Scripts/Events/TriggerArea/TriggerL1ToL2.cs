using UnityEngine;

public class TriggerL1ToL2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) SceneMgr.Instance.LoadScene("Level2");
    }
}
