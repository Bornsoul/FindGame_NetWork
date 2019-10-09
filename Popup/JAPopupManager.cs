using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAPopupManager : HL_Singleton<JAPopupManager>
{
    public HL_PrefabPool m_pObject = null;

    private void Awake()
    {
        if (TransportTCP.I == null)
        {
            Application.LoadLevel("Init");
            return;
        }
    }

    public JAPop_Mng Create_Notice(string sText, float fTime=0.8f)
    {
        JAPop_Mng pObj = m_pObject.GetObject("Pop_Popup").GetComponent<JAPop_Mng>();
                
        if (pObj == null) return null;
        HL_SoundMng.I.Play("SFX", "popup");
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        pObj.transform.localPosition = Vector3.zero;
        pObj.transform.localScale = Vector3.one;
        pObj.Enter(sText, fTime);
        
        return pObj;
    }

    public JAMenu_Option Create_Option(float fX = 0f)
    {
        JAMenu_Option pObj = m_pObject.GetObject("Pop_Option").GetComponent<JAMenu_Option>();
        
        if (pObj == null) return null;

        pObj.transform.localPosition = Vector3.zero;
        pObj.transform.localScale = Vector3.one;
        pObj.Enter(fX);

        return pObj;
    }
}
