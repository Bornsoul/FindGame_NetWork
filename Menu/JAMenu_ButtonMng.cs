using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_ButtonMng : MonoBehaviour
{

    public bool m_bClick = false;
    
    void Start()
    {
        m_bClick = true;
    }

    
    void Update()
    {

    }

    public void Button_Connect()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(1f, 1.1f));
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        JAManager.I.m_eHost = JAManager.eHost.E_HOST_CLIENT;
        JAMenu_Scene.I.Create_Connect(true);
    }

    public void Button_Host()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(1f, 1.1f));
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        JAManager.I.m_eHost = JAManager.eHost.E_HOST_SERVER;
        JAMenu_Scene.I.Create_Host(true);
    }

    public void Button_Ranking()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(1f, 1.1f));
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        JAManager.I.m_eHost = JAManager.eHost.E_HOST_CLIENT;
        JAMenu_Scene.I.Create_Ranking(true);
    }

    public void Button_Option()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(1f, 1.1f));
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        JAManager.I.m_eHost = JAManager.eHost.E_HOST_CLIENT;
        JAPopupManager.I.Create_Option(0f);
    }

}
