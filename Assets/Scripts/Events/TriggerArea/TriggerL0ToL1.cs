using UnityEngine;

public class TriggerL0ToL1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) SceneMgr.Instance.LoadScene("Level1");
    }
}
