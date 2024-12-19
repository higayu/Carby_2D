using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioClip bgmClip; // BGM用のオーディオクリップ
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSourceコンポーネントが見つかりません。");
            return;
        }

        if (bgmClip == null)
        {
            Debug.LogError("BGMのAudioClipが設定されていません。");
            return;
        }

        audioSource.clip = bgmClip;
        audioSource.loop = true; // ループ再生
        //audioSource.Play(); // BGMを再生
    }

    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void PauseBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause();
        }
    }
}
