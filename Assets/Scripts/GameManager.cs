using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager s_Instance;
    public CameraController CameraController;
    public Transform HeroTransform;
    public GameObject CameraGameObject;
    public GameObject[] LevelScenes;
    public Vector3 HeroInitalPos { get; set; }
    public ImageAnimation[] StarImages;
    public GameObject WinUI;
    private int m_RestNumOfStar = 4;
    [SerializeField]
    private AudioClip m_WinClip;
    public int RestNumOfStar 
    { 
        get 
        { 
            return m_RestNumOfStar;
        } 
        set 
        {
            StartCoroutine(StarImages[value].PlayAnimation()); 
            m_RestNumOfStar = value;
            if(m_RestNumOfStar==0)
            {
                WinUI.SetActive(true);
                CameraController.enabled = false;
                HeroTransform.gameObject.SetActive(false);
                SoundManager.s_Instance.Play(m_WinClip);
            }
        }
    }
	// Use this for initialization
	void Awake () {
        s_Instance = this;
	}

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }
}
