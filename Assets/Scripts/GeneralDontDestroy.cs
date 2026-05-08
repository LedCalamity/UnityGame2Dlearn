using UnityEngine;

public class GeneralDontDestroy : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
