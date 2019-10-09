using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_Pop_Connect : MonoBehaviour
{
    enum eConnectMod
    {
        E_CONNECT_NONE = 0,
        E_CONNECT_CONNECTING
    };

    enum eState
    {
        E_STATE_NONE = 0,
        E_STATE_CONNECT,
        E_STATE_GAME
    };

    public GameObject[] m_pConnect_Obj = null;

    public UIInput m_pInput_IP = null;
    public UIInput m_pInput_Port = null;
    public UILabel m_pConnecting_Lbl = null;

    public string m_sConnect_IP = string.Empty;
    public string m_sConnect_Port = string.Empty;
    private eState m_eState = eState.E_STATE_NONE;

    private void OnEnable()
    {
        m_eState = eState.E_STATE_NONE;

        m_pInput_IP.value = "127.0.0.1";
        m_pInput_IP.savedAs = "127.0.0.1";
        m_pInput_Port.savedAs = "50765";
        SelectMode(eConnectMod.E_CONNECT_NONE);
    }

    private void Update()
    {
        switch (m_eState)
        {
            case eState.E_STATE_NONE:
                break;
            case eState.E_STATE_CONNECT:
                GetConnected();
                break;
            case eState.E_STATE_GAME:
                break;
        }
    }

    public void Button_Cencel()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 0.6f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        gameObject.SetActive(false);
        JAMenu_Scene.I.m_pButton_Mng.m_bClick = true;
    }

    public void Button_Connect()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(0.6f, 0.7f));
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        m_sConnect_IP = m_pInput_IP.label.text;
        m_sConnect_Port = m_pInput_Port.label.text;

        bool bConnect = TransportTCP.I.Connect(m_sConnect_IP, int.Parse(m_sConnect_Port));
        if (bConnect == true)
        {
            m_pConnecting_Lbl.text = "접속 대기 중..";
        }
        else
        {
            JAPopupManager.I.Create_Notice("서버를 찾지 못했습니다..", 1f);
            m_pConnecting_Lbl.text = "서버를 찾지 못했습니다..";
        }

        SelectMode(eConnectMod.E_CONNECT_CONNECTING);
        m_eState = eState.E_STATE_CONNECT;

        Debug.Log("IP : " + m_sConnect_IP + " Port : " + m_sConnect_Port);
    }

    public void Button_Connecting()
    {
        if ( m_pInput_IP.value == "" || m_pInput_Port.value == "")
        {
            JAPopupManager.I.Create_Notice("IP 또는 Port 값은 비워둘수 없습니다!", 1f);
            return;
        }

        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(0.6f, 0.7f));
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        m_eState = eState.E_STATE_NONE;
        TransportTCP.I.StopServer();
        SelectMode(eConnectMod.E_CONNECT_NONE);
    }

    void SelectMode(eConnectMod eMod)
    {
        for (int i = 0; i < m_pConnect_Obj.Length; i++)
            m_pConnect_Obj[i].SetActive(false);

        switch (eMod)
        {
            case eConnectMod.E_CONNECT_NONE:
                m_pConnect_Obj[0].SetActive(true);
                break;
            case eConnectMod.E_CONNECT_CONNECTING:
                m_pConnect_Obj[1].SetActive(true);
                break;
        }
    }

    void GetConnected()
    {
        if (TransportTCP.I.IsConnected() == true)
        {
            m_eState = eState.E_STATE_GAME;
            AutoFade.LoadLevel("Game", 0.3f, 0.3f, Color.black);
        }
    }
}
