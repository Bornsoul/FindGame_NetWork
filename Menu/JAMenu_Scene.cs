using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_Scene : HL_Singleton<JAMenu_Scene>
{
    public JAMenu_Pop_Connect m_pPop_Connect = null;
    public JAMenu_Pop_Host m_pPop_Host = null;
    public JAMenu_RankMng m_pRank_Mng = null;

    public JAMenu_ButtonMng m_pButton_Mng = null;

    private void Awake()
    {
        if (TransportTCP.I == null)
        {
            Application.LoadLevel("Init");
            return;
        }
    }

    void Start()
    {
        HL_SoundMng.I.FadeOut("BGM", "bgm1");
        HL_SoundMng.I.FadeOut("BGM", "bgm2");
        HL_SoundMng.I.FadeOut("AMB", "amb_bgm");

        if (JAManager.I.m_bLoginScene == true)
        {
            HL_SoundMng.I.Play("BGM", "bgm3", true);
        }
        HL_SoundMng.I.SetVolue("BGM", "bgm3", 0.8f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        JAManager.I.m_bLoginScene = true;
        AllPopupUnShow();
    }
    

    public void AllPopupUnShow()
    {
        m_pPop_Host.gameObject.SetActive(false);
        m_pPop_Connect.gameObject.SetActive(false);
        m_pRank_Mng.gameObject.SetActive(false);
    }

    public void Create_Connect(bool bShow)
    {
        m_pPop_Host.gameObject.SetActive(!bShow);
        m_pRank_Mng.gameObject.SetActive(!bShow);
        m_pPop_Connect.gameObject.SetActive(bShow);        
    }

    public void Create_Host(bool bShow)
    {
        m_pPop_Connect.gameObject.SetActive(!bShow);
        m_pRank_Mng.gameObject.SetActive(!bShow);
        m_pPop_Host.gameObject.SetActive(bShow);
    }

    public void Create_Ranking(bool bShow)
    {
        m_pPop_Connect.gameObject.SetActive(!bShow);
        m_pPop_Host.gameObject.SetActive(!bShow);
        m_pRank_Mng.gameObject.SetActive(bShow);
    }

}
