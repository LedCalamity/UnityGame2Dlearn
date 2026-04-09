using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonQuitGame : MonoBehaviour
{
    [SerializeField] private Button button_quit;
    void Start()
    {
        button_quit.onClick.AddListener(QuitGame);
    }
    void QuitGame()
    {
        Application.Quit();
    }
}
