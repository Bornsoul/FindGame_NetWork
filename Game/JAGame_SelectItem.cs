using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_SelectItem : MonoBehaviour
{
    public string m_sName = string.Empty;
    public bool m_bFinded = false;
    public int m_nIndex = 0; 
  
    public void SetCreateCircle(string sName)
    {
        JAGame_Scene.I.m_pPopup_Mng.Create_Circle(gameObject.transform.localPosition, sName);
        m_bFinded = true;
    }

    public void Button_Click()
    {
        if (JAGame_Scene.I.m_bMyTurn == false) return;

        if (m_bFinded == true)
        {
            Debug.Log("이미 선택됨");
            return;
        }

        JAGame_Scene.I.m_sClickName = m_sName;
        m_bFinded = true;
       
    }

}
