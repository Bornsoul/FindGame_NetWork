using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_AccountInfo : MonoBehaviour
{

    public UILabel m_pName = null;
    public UILabel m_pRate = null;
    public UILabel m_pCoonect = null;

    float m_fTime = 0f;

    void OnEnable()
    {
        if (JAManager.I == null) return;
        m_pName.text = "[FFF358FF]" + JAManager.I.m_sMyAccount + "[-]님 반갑습니다!";
        m_pRate.text = "[58FF6EFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_WIN, JAManager.I.GetSearchAccount(JAManager.I.m_sMyAccount, "UID").ToString(), JAManager.I.m_sMyAccount) + " 승[-] [FFEC4FFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_DRAW, JAManager.I.GetSearchAccount(JAManager.I.m_sMyAccount,"UID").ToString(), JAManager.I.m_sMyAccount) + " 무[-] [FF5858FF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_LOSE, JAManager.I.GetSearchAccount(JAManager.I.m_sMyAccount, "UID").ToString(), JAManager.I.m_sMyAccount) + " 패[-]";
        m_pCoonect.text = "현재 [FFF358FF]" + JAManager.I.GetSearchLenght("ULOGIN", "ULOGIN='1'") + "명[-] 접속중";
    }

    private void Update()
    {
        if (gameObject.activeSelf == false) return;

        m_fTime += Time.deltaTime;
        
        if ( m_fTime >= 3f)
        {
            m_pCoonect.text = "현재 [FFF358FF]" + JAManager.I.GetSearchLenght("ULOGIN", "ULOGIN='1'") + "명[-] 접속중";
            m_fTime = 0f;
        }
    }
    
}
