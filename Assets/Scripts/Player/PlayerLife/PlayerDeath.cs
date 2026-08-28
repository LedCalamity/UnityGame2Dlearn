using System.Collections;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField, Min(1)] int max_lives = 3;

    int current_lives;
    bool is_dying;
    PlayerRespawn respawnmr;

    public int CurrentLives => current_lives;
    public int MaxLives => max_lives;

    void Awake()
    {
        respawnmr = GetComponent<PlayerRespawn>();
        current_lives = max_lives;
    }

    void Start()
    {
        RefreshLifeDisplay();
    }

    public void PlayerDie()
    {
        if(is_dying)
        {
            return;
        }

        StartCoroutine(DieRoutine());
    }

    public bool AddLife(int amount)
    {
        if(amount <= 0 || current_lives >= max_lives)
        {
            return false;
        }

        current_lives = Mathf.Min(current_lives + amount, max_lives);
        RefreshLifeDisplay();
        return true;
    }

    public bool IncreaseMaxLives(int amount, bool restore_added_lives = true)
    {
        if(amount <= 0)
        {
            return false;
        }

        max_lives += amount;
        if(restore_added_lives)
        {
            current_lives += amount;
        }

        RefreshLifeDisplay();
        return true;
    }

    IEnumerator DieRoutine()
    {
        is_dying = true;
        current_lives = Mathf.Max(0, current_lives - 1);
        RefreshLifeDisplay();

        if(current_lives <= 0)
        {
            respawnmr.RespawnPlayerTotal();
            yield break;
        }

        respawnmr.RespawnPlayerLight();
        yield return null;
        is_dying = false;
    }

    void RefreshLifeDisplay()
    {
        UIManager.Instance?.LifeDisplay?.SetLives(current_lives, max_lives);
    }
}
