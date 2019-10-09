using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading;

public class JALogin_CreateAccount : MonoBehaviour
{
    public UIInput m_pInput_AC = null;
    public UIInput m_pInput_PS = null;
    public UIInput m_pInput_PSRe = null;

    void Start()
    {
        m_pInput_PS.label.text = "";
        m_pInput_PS.savedAs = "";
        m_pInput_PS.value = "";
        m_pInput_PSRe.label.text = "";
        m_pInput_PSRe.savedAs = "";
        m_pInput_PSRe.value = "";
    }
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (m_pInput_AC.isSelected == true)
            {
                m_pInput_PS.Submit();
            }
        }
    }

    public void Button_Create()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 0.7f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        if (m_pInput_AC.value == "")
        {
            JAPopupManager.I.Create_Notice("계정명을 입력해주세요!", 1f);
            return;
        }
        if (m_pInput_PS.value == "")
        {
            JAPopupManager.I.Create_Notice("비밀번호를 입력해 주세요!", 1f);
            return;
        }
        if (m_pInput_PSRe.value == "")
        {
            JAPopupManager.I.Create_Notice("비밀번호 재입력을 입력 해주세요!", 1f);
            return;
        }
        else
        {
            if (m_pInput_PS.value != m_pInput_PSRe.value)
            {
                JAPopupManager.I.Create_Notice("재입력한 비밀번호가 서로 틀립니다!", 1f);
                return;
            }

            if (JAManager.I.GetUser_AccountCheck(m_pInput_AC.value) == true)
            {
                JAPopupManager.I.Create_Notice("이미 존재하는 계정명 입니다!", 1.5f);
                return;
            }
        }


        StartCoroutine(Cor_Create());

        JALogin_LoginScene.I.m_bPanel = true;
        gameObject.SetActive(false);
    }

    IEnumerator Cor_Create()
    {

        Debug.Log(m_pInput_AC.value + " , " + m_pInput_PSRe.value);
        JAManager.I.CreateUser(m_pInput_AC.value, m_pInput_PSRe.value);
        JAPopupManager.I.Create_Notice("계정 생성 완료!", 1f);

        yield return null;
    }

    public void Button_Cancel()
    {
        HL_SoundMng.I.Play("SFX", "button");
        HL_SoundMng.I.SetPitch("SFX", "button", 0.7f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        JALogin_LoginScene.I.m_bPanel = true;
        gameObject.SetActive(false);
    }
}
