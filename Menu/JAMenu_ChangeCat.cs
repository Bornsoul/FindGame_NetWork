using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_ChangeCat : MonoBehaviour
{
    public enum eState
    {
        E_STATE_NONE = 0,
        E_STATE_CHANGE1,
        E_STATE_CHANGE2
    }

    public UISprite[] m_pCatSprite = null;
    private eState m_eState = eState.E_STATE_NONE;

    public float m_fSpeed = 1f;
    public float m_fNextTime = 3f;

    private float m_fTimeDt = 0f;
    private float m_fAlpha1 = 0f;
    private float m_fAlpha2 = 0f;

    bool m_bClick = false;

    void FixedUpdate()
    {
        switch (m_eState)
        {
            case eState.E_STATE_NONE:
                m_eState = eState.E_STATE_CHANGE1;
                break;
            case eState.E_STATE_CHANGE1:
                m_fTimeDt += Time.deltaTime;

                if ( m_fAlpha1 >= 0f )
                m_fAlpha1 -= m_fSpeed * Time.deltaTime;

                if ( m_fAlpha2 < 1f)
                m_fAlpha2 += m_fSpeed * Time.deltaTime;
                
                m_pCatSprite[0].color = new Color(1f, 1f, 1f, m_fAlpha1);
                m_pCatSprite[1].color = new Color(1f, 1f, 1f, m_fAlpha2);

                if ( m_fTimeDt >= m_fNextTime)
                {
                    m_pCatSprite[0].spriteName = "LogoCat_" + NGUITools.RandomRange(1, 5);

                    m_fAlpha1 = 1f;
                    m_fAlpha2 = 0f;

                    m_fTimeDt = 0f;
                    m_eState = eState.E_STATE_CHANGE2;
                }
                
                break;
            case eState.E_STATE_CHANGE2:
                m_fTimeDt += Time.deltaTime;

                if (m_fAlpha1 >= 0f)
                    m_fAlpha1 -= m_fSpeed * Time.deltaTime;

                if (m_fAlpha2 < 1f)
                    m_fAlpha2 += m_fSpeed * Time.deltaTime;

                m_pCatSprite[1].color = new Color(1f, 1f, 1f, m_fAlpha1);
                m_pCatSprite[0].color = new Color(1f, 1f, 1f, m_fAlpha2);

                if (m_fTimeDt >= m_fNextTime)
                {
                    m_pCatSprite[1].spriteName = "LogoCat_" + NGUITools.RandomRange(1, 5);

                    m_fAlpha1 = 1f;
                    m_fAlpha2 = 0f;
                    m_fTimeDt = 0f;
                    m_eState = eState.E_STATE_CHANGE1;
                }
                
                break;
        }
    }

    public void OnClick()
    {
        if (m_bClick == true) return;
        m_bClick = true;

        int nRand = NGUITools.RandomRange(1, 2);
        HL_SoundMng.I.Play("SFX", "cat" + nRand);
        HL_SoundMng.I.SetPitch("SFX", "cat" + nRand, Random.RandomRange(0.9f, 1.1f));
        HL_SoundMng.I.SetVolue("SFX", "cat2", 0.7f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        StartCoroutine(Cor_Click());
    }

    IEnumerator Cor_Click()
    {
        yield return new WaitForSeconds(0.6f);

        m_bClick = false;
    }
}
