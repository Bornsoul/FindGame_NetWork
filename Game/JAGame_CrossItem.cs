using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_CrossItem : MonoBehaviour
{
    public UILabel m_pName = null;
    public UISprite m_pSprite = null;

    public void Enter(Vector2 stPos, string sName)
    {
        gameObject.transform.position = stPos;

        if (JAManager.I.m_sMyAccount == sName)
        {
            m_pSprite.spriteName = "badjob2";
            m_pName.gradientBottom = new Color(1f, 16f / 255, 0f);
        }
        else
        {
            m_pSprite.spriteName = "badjob";
            m_pName.gradientBottom = new Color(0f, 118 / 255, 1f);
            
        }

        m_pName.text = sName;


        StartCoroutine(Cor_Destroy());
    }

    IEnumerator Cor_Destroy()
    {
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
    }
}
