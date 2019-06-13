using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioClip audio_rotate;
    public AudioClip audio_btnClick;
    public AudioClip audio_startLevel;
    public AudioClip audio_selectModule;

    private AudioSource source
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    public static AudioControl instance;

    private void Awake()
    {
        instance = this;
    }

    public void PlayRotate()
    {
        source.clip = audio_rotate;
        source.Play();
    }

    public void PlayBtnClick()
    {
        source.clip = audio_btnClick;
        source.Play();
    }

    public void PlayStartLevel()
    {
        source.clip = audio_startLevel;
        source.Play();
    }

    public void PlayModule()
    {
        source.clip = audio_selectModule;
        source.Play();
    }
}
