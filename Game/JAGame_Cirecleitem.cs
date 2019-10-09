using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_Cirecleitem : MonoBehaviour
{
    public UILabel m_pName = null;
    public UISprite m_pSprite = null;

    public void Enter(Vector2 stPos, string sName)
    {
        gameObject.transform.localPosition = stPos;

        if (JAManager.I.m_sMyAccount == sName)
        {
            m_pSprite.spriteName = "goodjob";
            m_pName.gradientBottom = new Color(1f, 16f / 255, 0f);
        }
        else
        {
            m_pSprite.spriteName = "goodjob2";
            m_pName.gradientBottom = new Color(0f, 118 / 255, 1f);
        }

        m_pName.text = sName;
    }

}
