using UnityEngine;

public class PanelChanger : MonoBehaviour
{
    public GameObject[] panels;

    void Start()
    {
        if(gameObject.scene.name != "MainMenu" || SceneMgr.Instance == null) return;

        string panel_name = SceneMgr.Instance.TakeMainMenuPanel();
        if(string.IsNullOrEmpty(panel_name)) return;

        foreach(GameObject panel in panels)
        {
            if(panel != null && panel.name == panel_name)
            {
                ShowPanel(panel);
                return;
            }
        }

        Debug.LogWarning($"PanelChanger cannot find menu panel: {panel_name}", this);
    }

    public void ShowPanel(GameObject panel)
    {
        foreach(var p in panels)
        {
            p.SetActive(p == panel);
        }
    }
}
