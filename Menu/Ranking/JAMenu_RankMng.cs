using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_RankMng : MonoBehaviour
{
    public JAMenu_Rank_Mng m_pRank_Mng = null;


    void OnEnable()
    {
        m_pRank_Mng.Enter();
    }
    
    public void Button_Cencel()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", Random.RandomRange(0.6f, 0.7f));
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        m_pRank_Mng.Destroy();
    }
}
