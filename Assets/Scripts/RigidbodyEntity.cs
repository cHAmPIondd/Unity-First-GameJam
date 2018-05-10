using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RigidbodyEntity : MonoBehaviour
{

    public Vector2 MaxRate = new Vector2(3,10000000);
    public bool IsFacingRight { get; set; }
    public Transform InitalParent { get; set; }
    public float JumpForce = 700f;
    [SerializeField]
    private float m_MoveForce = 5;
    [SerializeField]
    private float m_GroundCheckLength = 0.1f;
    [SerializeField]
    private AudioClip m_JumpCilp;

    private Rigidbody2D m_Rigidbody2D;
    private Animator m_Animator;

    private bool m_IsGround;
    void Start()
    {
        InitalParent = transform.parent;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_Animator=GetComponent<Animator>();
        IsFacingRight = true;
    }

    void FixedUpdate()
    {
        var raycast = Physics2D.CircleCast(transform.position, 0.35f, Vector2.down, m_GroundCheckLength, 1 << LayerMask.NameToLayer("Ground"));
        if (raycast)
        {
            m_Rigidbody2D.drag = 4f;
            transform.parent = raycast.transform;
        }
        else
        {
            m_Rigidbody2D.drag = 0.2f;
            transform.parent = InitalParent;
        }
        transform.rotation = Quaternion.identity;
        m_IsGround = raycast; 
        if (transform.tag == "Hero")
            m_Animator.SetBool("Ground", m_IsGround);
    }
    public void Move(float intensity = -10000, Vector2? dir = null)
    {
        if (intensity == -10000)
            intensity = IsFacingRight ? 1 : -1;
        if (Mathf.Abs(intensity) > 0.1f)
            IsFacingRight = intensity > 0;
        var scale = transform.localScale;
        scale.x = (IsFacingRight ? 1 : -1) * Mathf.Abs(scale.x);
        transform.localScale = scale;
        m_Rigidbody2D.AddForce((dir ?? Vector2.right) * intensity * m_MoveForce);
        m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(m_Rigidbody2D.velocity.x, -MaxRate.x, MaxRate.x), Mathf.Clamp(m_Rigidbody2D.velocity.y, -MaxRate.y, MaxRate.y));
        if(transform.tag=="Hero")
            m_Animator.SetFloat("Rate", Mathf.Abs(intensity));
    }
    public void Jump()
    {
        StartCoroutine(JumpCoroutine());
    }
    IEnumerator JumpCoroutine()
    {
        float time = 0;
        while (true)
        {
            time += Time.deltaTime;
            if(time>0.3f)
            {
                yield break;
            }
            if (m_IsGround)
            {
                m_IsGround = false;
                m_Rigidbody2D.drag = 0.2f;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
                m_Rigidbody2D.AddForce(Vector2.up * JumpForce); 
                if (transform.tag == "Hero")
                    m_Animator.SetTrigger("Jump");
                if (m_JumpCilp != null)
                    SoundManager.s_Instance.Play(m_JumpCilp);
                yield break;
            }
            yield return 0;
        }
    }
}
