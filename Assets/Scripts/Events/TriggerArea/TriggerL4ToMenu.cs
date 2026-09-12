using UnityEngine;

public class TriggerL4ToMenu : MonoBehaviour
{
    [SerializeField] string target_panel_name = "PanelComplete";
    bool is_loading;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(is_loading || !other.CompareTag("Player") || SceneMgr.Instance == null) return;

        is_loading = true;
        SceneMgr.Instance.LoadMainMenu(target_panel_name);
    }
}
