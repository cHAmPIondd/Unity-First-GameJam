using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PatrolMove : MonoBehaviour {
    [SerializeField]
    private Vector3[] m_TargetPos;
    [SerializeField]
    private float m_Time = 3;
    [SerializeField]
    private float m_DelayTime = 3;
	// Use this for initialization
	void Start () {
        StartCoroutine(Init());
	}
	IEnumerator Init()
    {
        yield return new WaitForSeconds(m_DelayTime); 
        transform.DOLocalPath(m_TargetPos, m_Time).SetLoops(-1, LoopType.Restart);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
