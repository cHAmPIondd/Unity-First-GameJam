using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private RigidbodyEntity m_RigidbodyEntity;
	// Use this for initialization
	void Start () {
        m_RigidbodyEntity=GetComponent<RigidbodyEntity>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        m_RigidbodyEntity.Move(horizontal); 
        if(Input.GetButtonDown("Jump"))
        {
            m_RigidbodyEntity.Jump();
        }
        
	}
}
