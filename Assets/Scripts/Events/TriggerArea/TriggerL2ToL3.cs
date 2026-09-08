using UnityEngine;

public class TriggerL2ToL3 : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            SaveManager.Instance.UnlockLevel(3);
            SceneMgr.Instance.LoadScene("Level3");
        }
    }
}
