using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAPop_Mng : MonoBehaviour
{
    public UILabel m_pText = null;

    public void Enter(string sText, float fTime = 0.8f)
    {
        m_pText.text = sText;
        StartCoroutine(Cor_Destroy(fTime));

        StartCoroutine(Cor_Red());
    }

    IEnumerator Cor_Red()
    {
        yield return new WaitForSeconds(0.25f);
        for ( int i = 0; i< 2; i++)
        {
            m_pText.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            m_pText.color = Color.white;

            yield return null;
        }

        yield return null;
    }

    IEnumerator Cor_Destroy(float m_fTime)
    {
        yield return new WaitForSeconds(m_fTime);

        gameObject.SetActive(false);
    }
}
