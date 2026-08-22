using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioClip general_music;
    public AudioClip death_sef;
    public AudioClip fire_sef;
    public AudioClip hit_sef;
    public AudioClip skill_4dir_fire_sef;
    public AudioClip skill_fire_aoe_sef;
    public AudioClip ground_pound_sef;

    List<AudioSource> audios = new List<AudioSource>();
    Dictionary<string,AudioClip> audioClipDict = new Dictionary<string,AudioClip>();
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        //0: bgm; 1: status sef; 2: action sef; 3: enemy action sef
        for (int i = 0; i < 4; i++)  
        {
            var audio = gameObject.AddComponent<AudioSource>();
            audio.volume = 0.3f;
            audios.Add(audio);
        }
        //add new clip here
        audioClipDict.Add("Caliburne", general_music);
        audioClipDict.Add("Death_sef",death_sef);
        audioClipDict.Add("Fire_sef", fire_sef);
        audioClipDict.Add("Hit_sef", hit_sef);
        audioClipDict.Add("4DirFire_sef", skill_4dir_fire_sef);
        audioClipDict.Add("FireAOE_sef", skill_fire_aoe_sef);
        audioClipDict.Add("GroundPound_sef", ground_pound_sef);
    }
    public void AudioPlay(int idx, string name, bool isLoop)
    {
        var clip = GetAudioClip(name);
        if (clip != null)
        {
            var cur_audio = audios[idx];
            cur_audio.clip = clip;
            cur_audio.loop = isLoop;
            cur_audio.Play();
        }
    }
    AudioClip GetAudioClip(string name)
    {
        return audioClipDict[name];
    }
}
