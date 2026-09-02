using UnityEngine;
using UnityEngine.UI;

public class ButtonRestart : MonoBehaviour
{
    [SerializeField] Button button_restart;

    void Start()
    {
        button_restart.onClick.AddListener(Restart);
    }

    void Restart()
    {
        SceneMgr.Instance.LoadScene("MainMenu");
    }
}
