using System;
using System.Collections;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public GameObject hiteffectPrefab;
    public GameObject skill4DirFireEffectPrefab;
    public GameObject skillFireAOEEffectPrefab;
    public static EffectManager Instance;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void Generate4DirFireEffect(Vector3 pos)
    {
        Instantiate(skill4DirFireEffectPrefab, pos, Quaternion.identity);
    }
    public void GenerateHitEffect(Vector3 pos)
    {
        Instantiate(hiteffectPrefab, pos, Quaternion.identity);
    }
    public GameObject GenerateFireAOEEffect(Vector3 pos)
    {
        return Instantiate(skillFireAOEEffectPrefab, pos, Quaternion.identity);
    }
    public void GenerateDeathSlowMotion(float l_time)
    {
        StartCoroutine(SlowMotionTask(l_time));
    }
    IEnumerator SlowMotionTask(float l_time)
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(l_time);
        Time.timeScale = 1f;
    }
}
