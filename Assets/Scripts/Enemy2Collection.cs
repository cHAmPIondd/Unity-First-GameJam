using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Collection : MonoBehaviour {
    [SerializeField]
    private Enemy2Controller[] m_Enemy2Collection;
	// Use this for initialization
	void Start () {
        StartCoroutine(AutoTurnDirction());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator AutoTurnDirction()
    {
        var pos = transform.position;
        var facing = m_Enemy2Collection[0].RigidbodyEntity.IsFacingRight;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Vector3.SqrMagnitude(pos-transform.position) < 0.01 && facing == m_Enemy2Collection[0].RigidbodyEntity.IsFacingRight)
            {
                for (int i = 0; i < m_Enemy2Collection.Length;i++ )
                    m_Enemy2Collection[i].RigidbodyEntity.IsFacingRight = !m_Enemy2Collection[i].RigidbodyEntity.IsFacingRight;
            }
            pos = transform.position;
            facing = m_Enemy2Collection[0].RigidbodyEntity.IsFacingRight;
        }
    }
}
