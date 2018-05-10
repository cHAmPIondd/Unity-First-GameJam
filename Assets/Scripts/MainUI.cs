using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MainUI : MonoBehaviour,IPointerClickHandler
{
	public void StartGame(GameObject go)
    {
        //保存需要加载的目标场景  
        Globe.nextSceneName = "Game";

        SceneManager.LoadScene("Loading");     
    }
    [SerializeField]
    private GameObject m_MainMainGameObject;
    [SerializeField]
    private GameObject m_HelpGameObject;

    public void Help()
    {
        m_MainMainGameObject.SetActive(false);
        m_HelpGameObject.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    [SerializeField]
    private GameObject[] m_HelpGameObjects;
    [SerializeField]
    private Button m_PreviousButton;
    [SerializeField]
    private Button m_NextButton;
    private int m_Index=0;
    public void Previous()
    {
        m_HelpGameObjects[m_Index].SetActive(false);
        m_Index--;
        if (m_Index == 0)
            m_PreviousButton.interactable=false;
        m_NextButton.interactable = true;
        m_HelpGameObjects[m_Index].SetActive(true);
    }
    public void Next()
    {
        m_HelpGameObjects[m_Index].SetActive(false);
        m_Index++;
        if (m_Index == m_HelpGameObjects.Length - 1)
            m_NextButton.interactable = false;
        m_PreviousButton.interactable = true;
        m_HelpGameObjects[m_Index].SetActive(true);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        m_MainMainGameObject.SetActive(true);
        m_HelpGameObject.SetActive(false);
        m_HelpGameObjects[0].SetActive(true);
        m_HelpGameObjects[1].SetActive(false);
        m_HelpGameObjects[2].SetActive(false);
        m_PreviousButton.interactable = false;
        m_NextButton.interactable = true;
        m_Index = 0;
    }
}
