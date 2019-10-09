using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JALogin_LoginScene : HL_Singleton<JALogin_LoginScene>
{
    public UIInput m_pInput_ID = null;
    public UIInput m_pInput_Pass = null;
    public GameObject m_pCreateAC_Obj = null;
    public UIPanel m_pLoginBox_Panel = null;

    public bool m_bPanel = false;

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

        HL_SoundMng.I.Play("BGM", "bgm3", true);
        HL_SoundMng.I.SetVolue("BGM", "bgm3", 0.4f);

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        m_bPanel = false;
        m_pLoginBox_Panel.alpha = 0f;

        StartCoroutine(Cor_Panel());
        StartCoroutine(Cor_CheckServer(5f));
    }

    void Update()
    {
       
        if (m_bPanel == true)
            m_pLoginBox_Panel.alpha = NGUIMath.Lerp(m_pLoginBox_Panel.alpha, 1f, 3f * Time.deltaTime);
        else
            m_pLoginBox_Panel.alpha = NGUIMath.Lerp(m_pLoginBox_Panel.alpha, 0f, 3f * Time.deltaTime);
    }

    IEnumerator Cor_Panel()
    {
        yield return new WaitForSeconds(0.5f);
        m_bPanel = true;
        
    }

    IEnumerator Cor_CheckServer(float fTime)
    {
        yield return new WaitForSeconds(1f);
        JAManager.I.StartServerAliveCheck(fTime);
    }

    public void Button_Login()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 1f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        StartCoroutine(Cor_Login());
    }

    IEnumerator Cor_Login()
    {
        //Debug.Log("ID : " + m_pInput_ID.value + " , PASS : " + m_pInput_Pass.value);
        
        bool bConnect = JAManager.I.SetUser_AccountInfo(m_pInput_ID.value, m_pInput_Pass.value);

        //JAPopupManager.I.Create_Notice("" +JAManager.I.GetAccountSearch(m_pInput_ID.value, m_pInput_Pass.value), 1f);

        if (bConnect == true)
        {
            if (JAManager.I.GetUser_LoginCheck(JAManager.I.m_sMyAccount) <= 0)
            {
                JAManager.I.SetUpdate_LoginCheck(JAManager.I.m_sMyAccount, 1);
                
                HL_SoundMng.I.Play("SFX", "stinger");
                HL_SoundMng.I.SetVolue("SFX", "stinger", 0.35f);
                JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

                AutoFade.LoadLevel("Menu", 0.3f, 0.3f, Color.white);
            }
            else
            {
                JAPopupManager.I.Create_Notice("이미 로그인 상태입니다.", 1f);
                StopCoroutine(Cor_Login());
                
            }

        }
        else
        {
            JAPopupManager.I.Create_Notice("존재하지 않는 계정입니다", 1f);
            StopCoroutine(Cor_Login());
        }

        yield return null;
    }

    public void Button_CreateAccount()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 1f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        m_bPanel = false;
        m_pCreateAC_Obj.SetActive(true);
    }

    public void Button_Option()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(1f, 1.1f));
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        JAPopupManager.I.Create_Option(190f);
    }
}
