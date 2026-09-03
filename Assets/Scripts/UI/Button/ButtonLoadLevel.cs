using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadLevel : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] string level_name;
    [SerializeField, Min(0)] int level_index;
    [SerializeField] GameObject level_lock_panel;

    PanelChanger panel_changer;

    void Start()
    {
        panel_changer = GetComponent<PanelChanger>();
        button.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        if(!SaveManager.Instance.IsLevelUnlocked(level_index))
        {
            panel_changer.ShowPanel(level_lock_panel);
            return;
        }

        SceneMgr.Instance.StartGame(level_name, level_index);
    }
}
