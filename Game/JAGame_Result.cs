using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_Result : MonoBehaviour
{
    public UILabel m_pLbl_Result = null;

    public void Enter(string sText)
    {
        gameObject.SetActive(true);
        m_pLbl_Result.text = sText;
    }
    

    public void Button_Exit()
    {
       

        switch (JAManager.I.m_eHost)
        {
            case JAManager.eHost.E_HOST_CLIENT:
                TransportTCP.I.Disconnect();
                break;
            case JAManager.eHost.E_HOST_SERVER:
                TransportTCP.I.StopServer();
                break;
        }

        HL_SoundMng.I.Play("SFX", "stinger");
        HL_SoundMng.I.SetVolue("SFX", "stinger", 0.35f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        AutoFade.LoadLevel("Menu", 0.3f, 0.3f, Color.black);
    }
}
