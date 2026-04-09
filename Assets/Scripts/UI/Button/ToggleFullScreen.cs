using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullScreen : MonoBehaviour
{
    [SerializeField] private Toggle toggle_fullscreen;
    void Start()
    {
        toggle_fullscreen.onValueChanged.AddListener(ConvertFullScreen);
    }
    void ConvertFullScreen(bool is_on)
    {
        Screen.fullScreen = is_on;
    }
}
