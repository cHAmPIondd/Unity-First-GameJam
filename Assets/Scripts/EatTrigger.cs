using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTrigger : MonoBehaviour {
    [SerializeField]
    private AudioClip m_EatClip;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (m_EatClip != null)
            SoundManager.s_Instance.Play(m_EatClip);
        GameManager.s_Instance.RestNumOfStar--;
        GameManager.s_Instance.HeroInitalPos=transform.position;
        Destroy(gameObject);
    }
}
