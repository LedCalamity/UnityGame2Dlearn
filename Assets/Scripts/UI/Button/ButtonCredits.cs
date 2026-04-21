using UnityEngine;
using UnityEngine.UI;

public class ButtonCredits : MonoBehaviour
{
    [SerializeField] private Button button_cred;
    public GameObject panel_cred;
    PanelChanger changer;
    void Start()
    {
        changer = GetComponent<PanelChanger>();
        button_cred.onClick.AddListener(ToCredits);
    }
    void ToCredits()
    {
        changer.ShowPanel(panel_cred);
    }
}
