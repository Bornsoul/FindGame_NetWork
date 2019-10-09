using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_Rank_Item : MonoBehaviour
{
    public UILabel m_pLbl_Count = null;
    public UILabel m_pLbl_Name = null;
    public UILabel m_pLbl_Online = null;
    public UILabel m_pLbl_Rate = null;

    
    public void Enter(int nCount, string sAccount)
    {
        switch (nCount)
        {
            case 1:
                m_pLbl_Count.text = "[D1FF00FF]" + nCount + "등[-]";
                m_pLbl_Count.fontSize = 45;
                break;
            case 2:
                m_pLbl_Count.text = "[FFFFFFFF]" + nCount + "등[-]";
                m_pLbl_Count.fontSize = 40;
                break;
            case 3:
                m_pLbl_Count.text = "[AE4E38FF]" + nCount + "등[-]";
                m_pLbl_Count.fontSize = 37;
                break;
            default:
                m_pLbl_Count.text = "[818181FF]" + nCount + "등[-]";
                m_pLbl_Count.fontSize = 35;
                break;
        }

        m_pLbl_Name.text = sAccount == JAManager.I.m_sMyAccount ? sAccount + "[sup][8DFF4BFF][나][-][-]" : sAccount;
        m_pLbl_Online.text = JAManager.I.GetUser_LoginCheck(sAccount) >= 1 ? "[8DFF4BFF]접속중[-]" : "[898989FF]오프라인[-]";

        m_pLbl_Rate.text = "[58FF6EFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_WIN, JAManager.I.GetUser_UID(sAccount), sAccount) + " 승[-] [FFEC4FFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_DRAW, JAManager.I.GetUser_UID(sAccount), sAccount) + " 무[-] [FF5858FF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_LOSE, JAManager.I.GetUser_UID(sAccount), sAccount) + " 패[-]";
    }
}
