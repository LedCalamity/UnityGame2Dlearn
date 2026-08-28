using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDisplay : MonoBehaviour
{
    [SerializeField] GameObject heart_base_prefab;

    readonly List<GameObject> heart_bases = new();
    readonly List<Image> full_hearts = new();

    void Awake()
    {
        CollectExistingHearts();
    }

    void Start()
    {
        PlayerDeath player_death = FindAnyObjectByType<PlayerDeath>();
        if(player_death != null)
        {
            SetLives(player_death.CurrentLives, player_death.MaxLives);
        }
    }

    public void SetLives(int current_lives, int max_lives)
    {
        max_lives = Mathf.Max(0, max_lives);
        current_lives = Mathf.Clamp(current_lives, 0, max_lives);

        EnsureHeartCount(max_lives);

        for(int i = 0; i < heart_bases.Count; i++)
        {
            bool is_life_slot = i < max_lives;
            heart_bases[i].SetActive(is_life_slot);

            if(is_life_slot && full_hearts[i] != null)
            {
                full_hearts[i].enabled = i < current_lives;
            }
        }
    }

    void CollectExistingHearts()
    {
        heart_bases.Clear();
        full_hearts.Clear();

        for(int i = 0; i < transform.childCount; i++)
        {
            RegisterHeart(transform.GetChild(i).gameObject);
        }
    }

    void EnsureHeartCount(int required_count)
    {
        if(heart_base_prefab == null)
        {
            if(heart_bases.Count < required_count)
            {
                Debug.LogWarning("PlayerLifeDisplay needs a Heart Base Prefab to display additional lives.", this);
            }
            return;
        }

        while(heart_bases.Count < required_count)
        {
            GameObject new_heart = Instantiate(heart_base_prefab, transform);
            RegisterHeart(new_heart);
        }
    }

    void RegisterHeart(GameObject heart_base)
    {
        Transform full_heart = heart_base.transform.Find("heartfull");

        heart_bases.Add(heart_base);
        full_hearts.Add(full_heart != null ? full_heart.GetComponent<Image>() : null);
    }
}
