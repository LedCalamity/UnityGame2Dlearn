using UnityEngine;

public class Level0Audio : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.AudioPlay(0, "Caliburne", true);
    }
}
