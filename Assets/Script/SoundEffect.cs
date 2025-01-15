using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public static SoundEffect Instance = null;
    private AudioSource audio;

    void Awake()
    {
     
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }

        // AudioSourceの初期化
        audio = GetComponent<AudioSource>();
        if (audio == null)
        {
            Debug.LogError("AudioSource component is missing. Please attach an AudioSource to this GameObject.");
        }
    }



    public void PlaySoundEffect(string soundName)
    {
        if (audio == null)
        {
            Debug.LogError("AudioSource is not initialized.");
            return;
        }

        // リソースからサウンドをロード
        AudioClip music = (AudioClip)Resources.Load($"SoundEffect/{soundName}");
        if (music != null)
        {
            audio.Stop();
            audio.PlayOneShot(music);
            Debug.Log($"Playing sound: {soundName}");
        } else
        {
            Debug.LogError($"Sound '{soundName}' not found in Resources/Sound_Effects/");
        }
    }

    public void Door_Sound()
    {
        PlaySoundEffect("Door_Sound");
        Debug.Log("ドアの効果音");
    }

}
