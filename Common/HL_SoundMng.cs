using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HL_SoundMng : HL_Singleton<HL_SoundMng> {
    [Serializable]
    public struct ST_Sound
    {
       internal string m_sSoundThemeName;
        public HL_SoundEquip m_pSound;
    }

    public ST_Sound[] m_pSounds = null;




    void Awake()
    {
        for (int i = 0; i < m_pSounds.Length; i++)
        {
            m_pSounds[i].m_sSoundThemeName = m_pSounds[i].m_pSound.transform.name;
            m_pSounds[i].m_pSound.Enter();
        }

        DontDestroyOnLoad(this);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
   
        for(int i=0;i< m_pSounds.Length;i++)
        {
            m_pSounds[i].m_pSound.Destroy();
            m_pSounds[i].m_pSound = null;
        }
        m_pSounds = null;
        
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    public void Play(string sTheme, string sName, bool bLoop = false)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;
        
        pSound.Play(sName, bLoop);
    }

    public void SetPitch(string sTheme, string sName, float fValue)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;

        pSound.SetPitch(sName, fValue);
    }

    public void SetVolue(string sTheme, string sName, float fValue)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;

        pSound.SetVolume(sName, fValue);
    }

    public bool IsPlay(string sTheme, string sName)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return false;
        return pSound.IsPlay(sName);
    }

    public void ChangePlayWithFadeOut(string sTheme, string sName1, string sName2, bool bLoop = false)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;
        pSound.ChangePlayWithFadeOut(sName1, sName2, bLoop);
    }

    public void FadeOut(string sTheme, string sName)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;
        pSound.FadeOut(sName);
    }

    public void Stop(string sTheme, string sName)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;
        pSound.Stop(sName);
    }

    public void StopTheme(string sTheme)
    {
        HL_SoundEquip pSound = GetTheme(sTheme);
        if (pSound == null) return;
        pSound.AllStop();
    }

    public HL_SoundEquip GetTheme(string sTheme)
    {
        for (int i = 0; i < m_pSounds.Length; i++)
        {
            if(m_pSounds[i].m_sSoundThemeName==sTheme)
            {
                return m_pSounds[i].m_pSound;
            }
        }
        return null;
    }

}
