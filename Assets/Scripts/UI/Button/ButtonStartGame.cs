using UnityEngine;
using UnityEngine.UI;

public class ButtonStartGame : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] GameObject level_select_panel;

    PanelChanger panel_changer;

    void Start()
    {
        panel_changer = GetComponent<PanelChanger>();
        button.onClick.AddListener(OpenLevelSelect);
    }

    void OpenLevelSelect()
    {
        panel_changer.ShowPanel(level_select_panel);
    }
}
