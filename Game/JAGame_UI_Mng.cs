using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_UI_Mng : MonoBehaviour
{
    public UILabel m_pLbl_Fight = null;
    public UILabel m_pLbl_Count = null;
    public UILabel m_pLbl_Time = null;
    public UILabel m_pLbl_TurnTime = null;

    public GameObject m_pTurnTime_Obj = null;

    public GameObject m_pFindYou_Obj = null;

    private int m_nWorldTimer = 0;
    private float m_fWorldTimeDt = 0f;
    private bool m_bWorldTime = false;

    private int m_nTimer = 0;
    private float m_fTimeDt = 0f;
    private bool m_bTime = false;

    private void Update()
    {
        if (m_bWorldTime == false)
        {
            return;
        }

        m_fWorldTimeDt += Time.deltaTime;
        if( m_fWorldTimeDt >= 1)
        {
            m_nWorldTimer -= 1;
            m_pLbl_Time.text = m_nWorldTimer + " 초";
            if (m_nWorldTimer > 0 && m_nWorldTimer <= 10)
            {
                HL_SoundMng.I.Play("SFX", "ticktoc");
                JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
                StartCoroutine(Cor_Red(m_pLbl_Time));
            }
            m_fWorldTimeDt = 0f;
        }

        if ( m_nWorldTimer <= 0 )
        {
            HL_SoundMng.I.Play("SFX", "timeup");
            JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
            JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
            m_nWorldTimer = 0;
            m_pLbl_Time.text = m_nWorldTimer + " 초";
            m_bWorldTime = false;
        }

        if (m_bTime == false) return;
        m_fTimeDt += Time.deltaTime;
        if ( m_fTimeDt >= 1 )
        {
            StartCoroutine(Cor_TurnTime());
            m_nTimer -= 1;
            m_pLbl_TurnTime.text = m_nTimer + " 초";
            if (m_nTimer > 0 && m_nTimer <= 3)
            {
                HL_SoundMng.I.Play("SFX", "ticktoc");
                JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
                StartCoroutine(Cor_Red(m_pLbl_TurnTime));
            }
            m_fTimeDt = 0f;
        }
        
        if ( m_nTimer <= 0 )
        {
            HL_SoundMng.I.Play("SFX", "timeup");
            HL_SoundMng.I.Play("SFX", "turn");
            JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
            JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
            m_nTimer = 0;
            m_pLbl_TurnTime.text = m_nTimer + " 초";
            m_bTime = false;
        }

    }

    IEnumerator Cor_Red(UILabel pLabel)
    {
        for ( int i = 0; i< 2; i++)
        {
            pLabel.fontSize = 60;
            pLabel.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            pLabel.fontSize = 50;
            pLabel.color = Color.white;

            yield return null;
        }

        yield return null;
    }
    
    IEnumerator Cor_TurnTime()
    {
        Vector3 stPos = m_pTurnTime_Obj.transform.localPosition;

        for( int i = 0; i< 2; i++)
        {
            m_pTurnTime_Obj.transform.localPosition = new Vector3(stPos.x, stPos.y + 5, stPos.z);
            yield return new WaitForSeconds(0.1f);
            m_pTurnTime_Obj.transform.localPosition = new Vector3(stPos.x, stPos.y, stPos.z);
            yield return null;
        }

        yield return null;
    }
    public void SetBlackScreen(bool bShow)
    {
        m_pFindYou_Obj.SetActive(bShow);
    }

    public void SetGameWorldTime(int nTime)
    {
        m_nWorldTimer = nTime;
        m_pLbl_Time.text = m_nWorldTimer + " 초";
        m_bWorldTime = true;
    }

    public bool GetWorldTimerState()
    {
        return m_bWorldTime;
    }

    public int GetWorldTimerCount()
    {
        return m_nWorldTimer;
    }

    public void SetTimeStart(int nTime)
    {
        m_nTimer = nTime;
        m_pLbl_TurnTime.text = m_nTimer + " 초";
        m_bTime = true;
    }

    public bool GetTimerState()
    {
        return m_bTime;
    }

    public int GetTimerCount()
    {
        return m_nTimer;
    }

    public void SetTimerState(bool bState)
    {
        m_bTime = bState;
    }

    public void SetWorldTimerState(bool bState)
    {
        m_bWorldTime = bState;
    }

    public void SetFightCount(int nMy, int nYou)
    {
        StartCoroutine(Cor_CountRed(m_pLbl_Fight, 32));
        m_pLbl_Fight.text = nMy + "개" + " / " + nYou + "개";
    }

    public void SetCount(int nCurr, int nMax)
    {
        StartCoroutine(Cor_CountRed(m_pLbl_Count, 26));
        m_pLbl_Count.text = nCurr + " / " + nMax;
    }

    IEnumerator Cor_CountRed(UILabel pLabel, int nSize)
    {
        for (int i = 0; i < 2; i++)
        {
            pLabel.fontSize = nSize+3;
            pLabel.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            pLabel.fontSize = nSize;
            pLabel.color = Color.white;

            yield return null;
        }

        yield return null;
    }
}
