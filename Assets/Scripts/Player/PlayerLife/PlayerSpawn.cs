using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public string[] destroyScenes = { "MainMenu", "DeathScene" };
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
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
        PlayerControlGravityReverse gravity_reverse = GetComponent<PlayerControlGravityReverse>();
        if(gravity_reverse != null) gravity_reverse.ResetGravityState();

        if (destroyScenes.Contains(scene.name))
        {   
            Destroy(gameObject);
            return;
        }
        // Only numbered gameplay scenes (Level0, Level1, ...), not Bootstrap or menus.
        if(!scene.name.StartsWith("Level") || !int.TryParse(scene.name.Substring(5), out int level_index) || level_index < 0)
        {
            return;
        }

        if (scene.name == "Level1")
        {
            gameObject.GetComponent<PlayerControlDash>().is_unlocked = false;
        }
        GameObject spawnpt = GameObject.FindGameObjectWithTag("SpawnPoint"); //Now it only applies to single spawning point, will improve after learning
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.position = spawnpt.transform.position;
        rb.linearVelocity = Vector2.zero;
        Playerhp.Instance.ResetHP();
        PlayerMana.Instance.ResetMana();
        GetComponent<PlayerDeath>().ResetLives();
    }
}
