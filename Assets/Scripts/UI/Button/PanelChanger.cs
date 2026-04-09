using UnityEngine;

public class PanelChanger : MonoBehaviour
{
    public GameObject[] panels;
    public void ShowPanel(GameObject panel)
    {
        foreach(var p in panels)
        {
            p.SetActive(p == panel);
        }
    }
}
