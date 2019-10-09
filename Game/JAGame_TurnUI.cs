using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_TurnUI : MonoBehaviour
{
    public enum eTurnState
    {
        E_TURN_MY = 0,
        E_TURN_YOUR
    };

    public GameObject[] m_pTurnPosObj = null;
    public GameObject m_pTurnObj = null;

    public UILabel m_pMyName = null;
    public UILabel m_pYouName = null;
    public UILabel m_pMyRate = null;
    public UILabel m_pYouRate = null;

    public void SetNameSet(string sMyName, string sYouName)
    {
        m_pMyName.text = sMyName;
        m_pYouName.text = sYouName;
    }

    public void SetRateSet(string sMy, string sYou)
    {
 
        m_pMyRate.text = "[58FF6EFF]" + 
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_WIN, JAManager.I.GetSearchAccount(sMy, "UID").ToString(), sMy) + " 승[-] [FFEC4FFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_DRAW, JAManager.I.GetSearchAccount(sMy, "UID").ToString(), sMy) + " 무[-] [FF5858FF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_LOSE, JAManager.I.GetSearchAccount(sMy, "UID").ToString(), sMy) + " 패[-]";

        m_pYouRate.text = "[58FF6EFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_WIN, JAManager.I.GetSearchAccount(sYou, "UID").ToString(), sYou) + " 승[-] [FFEC4FFF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_DRAW, JAManager.I.GetSearchAccount(sYou, "UID").ToString(), sYou) + " 무[-] [FF5858FF]" +
            JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_LOSE, JAManager.I.GetSearchAccount(sYou, "UID").ToString(), sYou) + " 패[-]";
    }

    public void SetTurnRefresh()
    {
        m_pTurnObj.SetActive(false);
        m_pTurnObj.SetActive(true);

        //Debug.Log("asd");
    }

    public void SetTurnObjectUpt(eTurnState eTurn)
    {
        switch (eTurn)
        {
            case eTurnState.E_TURN_MY:
                m_pTurnObj.transform.localPosition = m_pTurnPosObj[0].transform.localPosition;
                break;
            case eTurnState.E_TURN_YOUR:
                m_pTurnObj.transform.localPosition = m_pTurnPosObj[1].transform.localPosition;
                break;
        }
    }
}
