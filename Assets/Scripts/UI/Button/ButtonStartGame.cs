using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStartGame : MonoBehaviour
{
    [SerializeField] private Button button;
    void Start()
    {
        button.onClick.AddListener(CallScene);
    }
    void CallScene()
    {
        SceneMgr.Instance.LoadScene("Level0");
    }
}
