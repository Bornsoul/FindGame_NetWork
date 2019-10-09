using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_PopupMng : MonoBehaviour
{
    public HL_PrefabPool m_pObject = null;

    public JAGame_CrossItem Create_Corss(Vector2 stPos, string sAccount)
    {
        JAGame_CrossItem pObj = m_pObject.GetObject("Click_Cross").GetComponent<JAGame_CrossItem>();
        
        if (pObj == null) return null;
        pObj.transform.localPosition = Vector3.zero;
        pObj.transform.localScale = Vector3.one;
        pObj.Enter(stPos, sAccount);
        HL_SoundMng.I.Play("SFX", "cross1");
        HL_SoundMng.I.SetVolue("SFX", "cross1", 0.8f);
        HL_SoundMng.I.SetPitch("SFX", "cross1", Random.RandomRange(0.9f, 1.05f));

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        return pObj;
    }

    public JAGame_Cirecleitem Create_Circle(Vector2 stPos, string sAccount)
    {
        JAGame_Cirecleitem pObj = m_pObject.GetObject("Click_Circle").GetComponent<JAGame_Cirecleitem>();
        int nRand = NGUITools.RandomRange(1, 3);
        if (pObj == null) return null;
        pObj.transform.localPosition = Vector3.zero;
        pObj.transform.localScale = Vector3.one;
        pObj.Enter(stPos, sAccount);
        HL_SoundMng.I.Play("SFX", "circle" + nRand);
        HL_SoundMng.I.SetPitch("SFX", "circle" + nRand, Random.RandomRange(0.9f, 1.05f));

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
        return pObj;
    }
}
