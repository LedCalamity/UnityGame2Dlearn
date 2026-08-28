using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image playerhpbar;
    public Image cdimagecover;
    public Image aoeCdImageCover;
    public TextMessageShow MessageText { get; private set; }
    public PlayerLifeDisplay LifeDisplay { get; private set; }
    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            MessageText = GetComponentInChildren<TextMessageShow>(true);
            LifeDisplay = GetComponentInChildren<PlayerLifeDisplay>(true);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
}
