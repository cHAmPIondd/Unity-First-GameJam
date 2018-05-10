using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager s_Instance;
    private AudioSource[] m_AudioSources;
    void Awake()
    {
        s_Instance = this;
        m_AudioSources = GetComponents<AudioSource>();
    }
    public void Play(AudioClip clip)
    {
        for (int i = 0; i < m_AudioSources.Length; i++)
        {
            if (!m_AudioSources[i].isPlaying)
            {
                m_AudioSources[i].clip = clip;
                m_AudioSources[i].Play();
                break;
            }
        }
    }
}

