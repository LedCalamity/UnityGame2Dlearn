using UnityEngine;
using UnityEngine.UI;

public class ButtonLoadLevel : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] string level_name;

    void Start()
    {
        button.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneMgr.Instance.StartGame(level_name);
    }
}
