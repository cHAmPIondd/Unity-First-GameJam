using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform m_HeroTransform;
    [SerializeField]
    private Texture2D m_AimSignTexture2D;
    [SerializeField]
    private GameObject m_BoomPrefab;
    [SerializeField]
    private AudioClip m_BoomClip;
    [SerializeField]
    private GameObject m_ShootPrefab;
    [SerializeField]
    private AudioClip m_ShootClip;
 /*   [SerializeField]
    private Transform m_SignTransform;
    [SerializeField]
    private GameObject[] m_WarningGameObject;
    [SerializeField]
    private int m_SignSize;
    [SerializeField]
    private float m_MoveRate=2;*/
    [SerializeField]
    private float m_ShootForce = 2;
    [SerializeField]
    private float m_Radius = 2;
    [SerializeField]
    private float m_CoolDownTime = 0.4f;
    private bool m_CursorIsLocked=true;
    private Camera m_Camera;
    private bool m_IsWarning;
//    private bool m_IsRecovering;
    private Vector3 m_InitalPos;
    private Vector3 m_TargetPos;
	// Use this for initialization
	void Start () {
        m_Camera=GetComponent<Camera>();
        m_InitalPos = transform.position;
        Cursor.SetCursor(m_AimSignTexture2D, new Vector2(m_AimSignTexture2D.width,m_AimSignTexture2D.height)/2, CursorMode.ForceSoftware);
        StartCoroutine(	PlayerControl());    
    }
	// Update is called once per frame
	void Update () {
  //      InternalLockUpdate();
 //       if (!m_IsRecovering)
        {
            
        }
        m_TargetPos = m_HeroTransform.position + new Vector3(0,1,-10); 
        transform.position = Vector3.Lerp(transform.position, m_TargetPos,4*Time.deltaTime);
   /*     var screenPos=m_Camera.WorldToScreenPoint(m_HeroTransform.position);
        if ((screenPos.x > Screen.width || screenPos.x < 0 || screenPos.y > Screen.height || screenPos.y < 0)&&!m_IsRecovering)
        {
            if (!m_IsWarning)
            {
                m_IsWarning = true;
                StartCoroutine("Warning");
                m_SignTransform.gameObject.SetActive(true);
            }
            var x =Screen.width / 2-screenPos.x;
            var y =Screen.height / 2-screenPos.y ;
            if (Mathf.Abs(x) / 16 > Mathf.Abs(y) / 9)
            {
                m_SignTransform.position = m_Camera.ScreenToWorldPoint(new Vector3(screenPos.x < 0 ? m_SignSize : Screen.width - m_SignSize, (Mathf.Abs(x) - Screen.width / 2 + m_SignSize) / Mathf.Abs(x) * y + screenPos.y, 10));
            }
            else
            {
                m_SignTransform.position = m_Camera.ScreenToWorldPoint(new Vector3((Mathf.Abs(y) - Screen.height / 2 + m_SignSize) / Mathf.Abs(y) * x + screenPos.x, screenPos.y < 0 ? m_SignSize : Screen.height - m_SignSize, 10));
            }
        }
        else
        {
            if (m_IsWarning)
            {
                m_IsWarning = false;
                StopCoroutine("Warning");
                for (int i = 0; i < m_WarningGameObject.Length;i++ )
                    m_WarningGameObject[i].SetActive(false);
                m_SignTransform.gameObject.SetActive(false);
            }
        }*/
	}
    IEnumerator PlayerControl()
    {
    //    float mouseX = Input.GetAxis("Mouse X");
    //    float mouseY = Input.GetAxis("Mouse Y");
   //     var aimScreenPos = m_AimSignTransform.position + new Vector3(mouseX, mouseY, 0) * m_MoveRate;
    //    var leftdown = m_Camera.ScreenToWorldPoint(new Vector2(0, 0));
    //    var rightup= m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    //    aimScreenPos = new Vector2(Mathf.Clamp(aimScreenPos.x, leftdown.x, rightup.x), Mathf.Clamp(aimScreenPos.y, leftdown.y, rightup.y));
        while (true)
        {
            yield return 0;
            if (Input.GetButton("Fire1"))
            {
                bool hadBoom = false;
                var pos = m_Camera.ScreenToWorldPoint(Input.mousePosition);
                var r = Physics2D.CircleCastAll(pos, m_Radius, Vector2.down, 0.001f);
                for (int i = 0; i < r.Length; i++)
                {
                    if (r[i].transform != null)
                    {
                        var tran = r[i].transform;
                        if (tran.CompareTag("BounceEntity") )
                        {
                            tran.GetComponent<Rigidbody2D>().AddForce(Vector2.up* m_ShootForce*3);
                        }
                        else if(tran.CompareTag("Hero"))
                        {
                            tran.GetComponent<Rigidbody2D>().AddForce(Vector2.up * m_ShootForce);
                        }
                        else if (tran.CompareTag("DestroyEntity"))
                        {
                            tran.GetComponent<LifeEntity>().BeHurt(1);
                            if(!hadBoom)
                            {
                                hadBoom = true;
                                Instantiate(m_BoomPrefab, r[i].transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                                SoundManager.s_Instance.Play(m_BoomClip);
                            }
                        }
                        else if (tran.CompareTag("BounceAndDestroyEntity"))
                        {
                            tran.GetComponent<Rigidbody2D>().AddForce(Vector2.up * m_ShootForce);
                            tran.GetComponent<LifeEntity>().BeHurt(1);
                        }
                    }
                }
                if (!hadBoom)
                {
                    Instantiate(m_ShootPrefab, pos + new Vector3(0, 0, 10), Quaternion.Euler(new Vector3(-90, 0, 0)));
                    SoundManager.s_Instance.Play(m_ShootClip);
                }
                yield return new WaitForSeconds(m_CoolDownTime);
            }
        }
    }
    /*
    IEnumerator Warning()
    {
        m_WarningGameObject[2].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_WarningGameObject[2].SetActive(false);
        m_WarningGameObject[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_WarningGameObject[1].SetActive(false);
        m_WarningGameObject[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        m_WarningGameObject[0].SetActive(false);
        m_HeroTransform.GetComponent<LifeEntity>().Reset();
        StartCoroutine(ResetCamera());
    }
    public IEnumerator ResetCamera()
    {
        m_IsRecovering = true;
        while (true)
        {
            yield return 0;
            transform.position = Vector3.Lerp(transform.position, m_InitalPos, 3 * Time.deltaTime);
            if (Vector3.SqrMagnitude(transform.position - m_InitalPos) < 1)
            {
                break;
            }
        }
        m_IsRecovering = false;
    }
    */
    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_CursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_CursorIsLocked = true;
        }

        if (m_CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_CursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
