using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1Controller : MonoBehaviour {
    [SerializeField]
    private Vector2 m_Size;
    private RigidbodyEntity m_RigidbodyEntity;
    private Rigidbody2D m_Rigidbody2D;
    // Use this for initialization
    void Start()
    {
        m_RigidbodyEntity = GetComponent<RigidbodyEntity>();
        m_Rigidbody2D=GetComponent<Rigidbody2D>();
        StartCoroutine(AutoTurnDirction());
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        m_RigidbodyEntity.Move(); 
        float gAdd = m_Rigidbody2D.gravityScale * Physics2D.gravity.y / m_Rigidbody2D.mass;
        float deltaTime = 0.1f;
        float gVelo = -0.5f * gAdd * deltaTime;
        float m_JumpForce = m_RigidbodyEntity.JumpForce *0.85f;
        float m_MoveRate = m_RigidbodyEntity.MaxRate.x * (m_RigidbodyEntity.IsFacingRight?1:-1)*1.4f;
        var rd = m_Rigidbody2D;
        var currentPos=(Vector2)transform.position ;
        for (int i = 0; i < 40; i++)
        {
            gVelo += (gAdd * deltaTime);
            var nextPos=currentPos + (new Vector2(0, m_JumpForce / rd.mass * Time.fixedDeltaTime) + new Vector2(0, gVelo) + new Vector2(m_MoveRate, 0)) * deltaTime;
            var raycast=Physics2D.BoxCast(currentPos, m_Size, 0, nextPos - currentPos, (nextPos - currentPos).magnitude,1<<LayerMask.NameToLayer("Ground")|1<<LayerMask.NameToLayer("Hurt"));
            if (raycast && transform.parent!=null&&transform.parent!=raycast.transform)
            {
                if (raycast.normal.y > 0 && raycast.transform.position.y != transform.parent.position.y &&Mathf.Abs(raycast.transform.position.y - transform.parent.position.y)<4)
                {
                    if(raycast.transform.gameObject.layer!=LayerMask.NameToLayer("Hurt"))
                    {
                        m_RigidbodyEntity.Jump();
                        return;
                    }
                }
                break;
            }
            currentPos =nextPos;
            if(currentPos.y<transform.position.y-5)
            {
                break;
            }
        }
        if(transform.parent!=m_RigidbodyEntity.InitalParent)
        {
            if (!Physics2D.Raycast(transform.position +(m_RigidbodyEntity.IsFacingRight?1:-1)*Vector3.right * 0.4f, Vector2.down, 4, 1 << LayerMask.NameToLayer("Ground")))
                m_RigidbodyEntity.IsFacingRight = !m_RigidbodyEntity.IsFacingRight;
        }
	}
    IEnumerator AutoTurnDirction()
    {
        var posX = transform.position.x;
        var facing = m_RigidbodyEntity.IsFacingRight;
        while(true)
        {
            yield return new WaitForSeconds(0.2f);
            if (Mathf.Abs(transform.position.x - posX) < 0.05 && facing == m_RigidbodyEntity.IsFacingRight)
            {
                m_RigidbodyEntity.IsFacingRight = !m_RigidbodyEntity.IsFacingRight;
            }
            posX = transform.position.x;
            facing = m_RigidbodyEntity.IsFacingRight;
        }
    }
    
   /* private void OnDrawGizmos()
    {
        float gAdd = m_Rigidbody2D.gravityScale * Physics2D.gravity.y / m_Rigidbody2D.mass;
        float deltaTime = 0.1f;
        float gVelo = -0.5f * gAdd * deltaTime;
        float m_JumpForce = m_RigidbodyEntity.JumpForce * 0.85f;
        float m_MoveRate = m_RigidbodyEntity.MaxRate * (m_RigidbodyEntity.IsFacingRight ? 1 : -1) * 1.4f;
        var rd = m_Rigidbody2D;
        var currentPos = (Vector2)transform.position;
        for (int i = 0; i < 40; i++)
        {
            gVelo += (gAdd * deltaTime);
            var nextPos = currentPos + (new Vector2(0, m_JumpForce / rd.mass * Time.fixedDeltaTime) + new Vector2(0, gVelo) + new Vector2(m_MoveRate, 0)) * deltaTime;

            Gizmos.DrawLine(currentPos, nextPos);
            currentPos = nextPos;
        }
    }*/
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Hero"))
            other.GetComponent<LifeEntity>().BeHurt();
    }
}
