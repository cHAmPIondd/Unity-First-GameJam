using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Controller : MonoBehaviour
{
    [SerializeField]
    private bool m_IsCollection = false;
    [SerializeField]
    private int m_RotateRate = 5;
    private float m_Angle = 90;

    public RigidbodyEntity RigidbodyEntity { get;private set; }
	// Use this for initialization
	void Awake () {
        RigidbodyEntity = GetComponent<RigidbodyEntity>();
	}
    void Start()
    {
        if (!m_IsCollection)
            StartCoroutine(AutoTurnDirction());
    }

    void FixedUpdate()
    {
        m_Angle = (m_Angle+Random.Range(-m_RotateRate, m_RotateRate) * Time.deltaTime);
        if (Mathf.Abs(m_Angle) > 90)
        {
            m_Angle = -(m_Angle%90);
            RigidbodyEntity.IsFacingRight = !RigidbodyEntity.IsFacingRight;
        }
        RigidbodyEntity.Move(dir: new Vector2(Mathf.Cos(m_Angle * Mathf.Deg2Rad), Mathf.Sin(m_Angle * Mathf.Deg2Rad)));
	}
    IEnumerator AutoTurnDirction()
    {
        var pos = transform.position;
        var facing = RigidbodyEntity.IsFacingRight;
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (Vector3.SqrMagnitude(pos - transform.position) < 0.01 && facing == RigidbodyEntity.IsFacingRight)
            {
                RigidbodyEntity.IsFacingRight = !RigidbodyEntity.IsFacingRight;
            }
            pos = transform.position;
            facing = RigidbodyEntity.IsFacingRight;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hero"))
            other.GetComponent<LifeEntity>().BeHurt();
    }
}
