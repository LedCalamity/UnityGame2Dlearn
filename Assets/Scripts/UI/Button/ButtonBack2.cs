using UnityEngine;
using UnityEngine.UI;

public class ButtonBack2 : MonoBehaviour
{
    [SerializeField] private Button button_back;
    PanelChanger changer;
    public GameObject panelcommon;
    void Start()
    {
        button_back.onClick.AddListener(BackCommon);
        changer = GetComponent<PanelChanger>();
    }
    void BackCommon()
    {
        changer.ShowPanel(panelcommon);
    }
}
