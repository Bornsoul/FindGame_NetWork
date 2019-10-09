using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JA_Animation : MonoBehaviour
{

    public Animation m_pAnimation = null;
    public float m_fStartDelay = 0f;
    public List<string> m_stAniName = null;

    public int m_nStartIndex = 0;

    private void OnEnable()
    {
        if (m_pAnimation == null)
            m_pAnimation = gameObject.GetComponent<Animation>();

        if (m_pAnimation == null)
        {
            Debug.Log("애니메이션 컴퍼넌트가 존재하지 않습니다.");
            return;
        }

        if (m_stAniName.Count <= 0)
        {
            Debug.Log("애니메이션 이름이 존재하지 않습니다.");
            return;
        }

        if (m_fStartDelay <= 0f)
            SetAnimation(m_stAniName[m_nStartIndex]);
        else        
            StartCoroutine(Delay(m_fStartDelay));
    }

    IEnumerator Delay(float fTime)
    {
        yield return new WaitForSeconds(fTime);
       
        SetAnimation(m_stAniName[m_nStartIndex]);
    }

    public void GetAnimationDone()
    {
        if (m_stAniName.Count <= 1) return;
        m_nStartIndex++;
        SetAnimation(m_stAniName[m_nStartIndex]);        
    }

    private void SetAnimation(string sName)
    {
        m_pAnimation.Stop();
        m_pAnimation.Play(sName);
    }
}
