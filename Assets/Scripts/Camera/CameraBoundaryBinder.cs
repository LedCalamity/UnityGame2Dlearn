using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CinemachineConfiner2D))]
public class CameraBoundaryBinder : MonoBehaviour
{
    private CinemachineConfiner2D confiner;

    private void Awake()
    {
        confiner = GetComponent<CinemachineConfiner2D>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        BindBoundary(SceneManager.GetActiveScene());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindBoundary(scene);
    }

    private void BindBoundary(Scene scene)
    {
        CameraBoundary boundary = null;

        foreach (GameObject rootObject in scene.GetRootGameObjects())
        {
            boundary = rootObject.GetComponentInChildren<CameraBoundary>(true);
            if (boundary != null)
            {
                break;
            }
        }

        confiner.m_BoundingShape2D = boundary != null ? boundary.BoundaryCollider : null;
        confiner.InvalidateCache();
    }
}
