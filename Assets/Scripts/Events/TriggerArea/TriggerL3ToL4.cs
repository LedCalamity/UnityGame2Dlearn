using UnityEngine;

public class TriggerL3ToL4 : MonoBehaviour
{
    bool is_loading;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(is_loading || !other.CompareTag("Player")) return;
        is_loading = true;
        SaveManager.Instance.UnlockLevel(4);
        SceneMgr.Instance.LoadScene("Level4");
    }
}
