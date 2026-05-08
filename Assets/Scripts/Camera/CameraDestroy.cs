using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDestroy : MonoBehaviour
{
    public string[] destroyScenesCamera = { "DeathScene", "MainMenu" };
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (destroyScenesCamera.Contains(scene.name))
        {
            Destroy(gameObject);
            return;
        }
    }
}
