using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio_Source
{
    public AudioClip clip;//播放声音片段
    public Transform transform;//音源位置
}

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Play(Audio_Source audio,float volume = 1.0f, float length = 0,float pitch=1.0f)
    {
        Debug.Log(length);
        GameObject go = new GameObject("Audio:"+audio.clip.name);
        AudioSource source=go.AddComponent<AudioSource>();
        go.transform.position = audio.transform.position;
        go.transform.parent = audio.transform;

        source.clip = audio.clip;
        source.volume = volume;
        source.pitch = pitch;
        if (length > 0)
        {
            source.loop = true;
        }
        else
        {
            length = audio.clip.length;
        }
        source.Play();
        Destroy(go, length);
    }

    public void PlayMix(Audio_Source[] audios, float[] factor,float[] length=null,float[] pitch=null)
    {
        if (audios.Length == factor.Length &&
            (length == null || factor.Length == length.Length) &&
            (pitch == null || pitch.Length == factor.Length))
        {
            if (length == null)
            {
                length = new float[audios.Length];
                for(int i=0;i<length.Length;i++)
                {
                    length[i] = 0;
                }
            }

            if(pitch==null)
            {
                pitch= new float[audios.Length];
                for (int i = 0; i < pitch.Length; i++)
                {
                    pitch[i] = 1;
                }
            }

            for (int i=0;i<audios.Length;i++)
            {
                Play(audios[i], factor[i],length[i],pitch[i]);
            }
        }
        else
        {
            Debug.Log("Audios与factor的长度不匹配.");
        }
    }
  
    public static AudioManager getInstance()
    {
        return instance;
    }
}
