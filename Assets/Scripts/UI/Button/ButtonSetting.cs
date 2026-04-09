using UnityEngine;
using UnityEngine.UI;

public class ButtonSetting : MonoBehaviour
{
    [SerializeField] private Button button_set;
    public GameObject panel_set;
    PanelChanger changer;
    void Start()
    {
        button_set.onClick.AddListener(ToSetting);
        changer = GetComponent<PanelChanger>();
    }

    void ToSetting()
    {
        changer.ShowPanel(panel_set);
    }
}
