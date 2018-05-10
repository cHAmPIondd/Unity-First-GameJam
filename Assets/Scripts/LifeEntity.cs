using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeEntity : MonoBehaviour {
    [SerializeField]
    private int m_MaxHealthPoint=1;
    [SerializeField]
    private Sprite[] m_Sprites;
    [SerializeField]
    private float m_RecoverTime = 5;
    [SerializeField]
    private GameObject m_DeathFeedbackPrefab;
    [SerializeField]
    private AudioClip m_DeathCilp;

    private Vector3 m_InitPosition;
    private int m_CurrentHealthPoint;
    private SpriteRenderer m_SpriteRenderer;
	// Use this for initialization
	void Start () {
        if (transform.CompareTag("Hero"))
        {
            GameManager.s_Instance.HeroInitalPos = transform.localPosition;
        }
        m_InitPosition = transform.localPosition;
        m_CurrentHealthPoint = m_MaxHealthPoint;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.sprite = m_Sprites[m_CurrentHealthPoint];
	}
	
    public void BeHurt(int point = 1)
    {
        if (m_CurrentHealthPoint > 0)
        {
            m_CurrentHealthPoint -= point;
            m_SpriteRenderer.sprite = m_Sprites[m_CurrentHealthPoint];
            if (m_CurrentHealthPoint <= 0)
                StartCoroutine(DeadAndRecover());
        }
    }
    IEnumerator DeadAndRecover()
    {
        var colliders = GetComponents<Collider2D>();
        var rigidbody = GetComponent<Rigidbody2D>();
        if (rigidbody != null)
        {
            rigidbody.Sleep();
            rigidbody.isKinematic = true;
        }
        if (m_DeathFeedbackPrefab != null)
            Instantiate(m_DeathFeedbackPrefab,transform.position,Quaternion.identity);
        for (int i = 0; i < colliders.Length;i++ )
            colliders[i].enabled = false;
        m_SpriteRenderer.enabled=false;
        if (m_RecoverTime < 0)
            yield break;
        if (m_DeathCilp != null)
            SoundManager.s_Instance.Play(m_DeathCilp);

        yield return new WaitForSeconds(m_RecoverTime);

        var rigidbodyEntity = GetComponent<RigidbodyEntity>();
        if (rigidbodyEntity != null)
            transform.parent = rigidbodyEntity.InitalParent;
        if (transform.CompareTag("Hero"))
        {
            //StartCoroutine(GameManager.s_Instance.CameraController.ResetCamera());
            m_InitPosition = GameManager.s_Instance.HeroInitalPos;
        }
        m_CurrentHealthPoint = m_MaxHealthPoint;
        m_SpriteRenderer.sprite = m_Sprites[m_CurrentHealthPoint];
        transform.localPosition = m_InitPosition;
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = true;
        m_SpriteRenderer.enabled = true;
        if (rigidbody != null)
        {
            rigidbody.isKinematic = false;
        }
    }
    public void Reset()
    {
        
    }
}
