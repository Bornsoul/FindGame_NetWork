using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_Option : MonoBehaviour
{
    public UILabel[] m_pSoundLbl = null;
    public UISprite[] m_pSoundSprite = null;

    public UISlider[] m_pSlider = null;
    public GameObject m_pUI = null;

    public bool m_bBGM = true;
    public bool m_bSFX = true;
    public float m_fMasterVol = 1f;

    public void Enter(float fX)
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 0.7f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        m_pUI.transform.localPosition = new Vector3(fX, 0f, 0f);

        m_bBGM = JAManager.I.m_bSoundBGMMute;
        m_bSFX = JAManager.I.m_bSoundSFXMute;
        m_fMasterVol = JAManager.I.m_fSoundMasterVol;

        Debug.Log(m_bBGM + " , " + m_bSFX);
        SoundBtnCheck();

    }

    void SoundBtnCheck()
    {
        if (m_bBGM == true)
            m_pSoundLbl[0].text = "배경음 [5EFF51FF]On[-]";
        else
            m_pSoundLbl[0].text = "배경음 [FF5151FF]Off[-]";
        
        if ( m_bSFX == true )
            m_pSoundLbl[1].text = "효과음 [5EFF51FF]On[-]";
        else
            m_pSoundLbl[1].text = "효과음 [FF5151FF]Off[-]";

        m_pSoundSprite[0].gameObject.SetActive(m_bBGM);
        m_pSoundSprite[1].gameObject.SetActive(m_bSFX);

        m_pSlider[0].value = JAManager.I.m_fSoundMasterVol;
    }

    public void Slide_BGM()
    {
        JAManager.I.m_fSoundMasterVol = m_pSlider[0].value;
        JAManager.I.SoundVolume(m_pSlider[0].value);
    }

    public void Button_SoundBGM()
    {
        m_bBGM = m_bBGM == true ? m_bBGM = false : m_bBGM = true;

        SoundBtnCheck();

        JAManager.I.m_bSoundBGMMute = m_bBGM;
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
    }

    public void Button_SoundSFX()
    {
        m_bSFX = m_bSFX == true ? m_bSFX = false : m_bSFX = true;

        SoundBtnCheck();

        JAManager.I.m_bSoundSFXMute = m_bSFX;
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
    }

    public void Button_Cancel()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(0.6f, 0.7f));
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        gameObject.SetActive(false);
    }
}
