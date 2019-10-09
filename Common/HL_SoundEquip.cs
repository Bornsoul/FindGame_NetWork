using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HL_SoundEquip : MonoBehaviour
{
    [System.Serializable]
    public struct ST_Sound
    {
        public string sName;
        public AudioClip pAudio;
        public bool SolidSound;

        public void Destroy()
        {
            pAudio = null;
        }
    }


    public List<ST_Sound> m_pSoundList = null;

    private Dictionary<string, AudioSource> m_pAudioSourceList = null;

    private GameObject m_pEmpty = null;

    bool m_bOnce = false;
    public void Enter()
    {
        if (m_bOnce==true) return;
        m_bOnce = true;
        m_pAudioSourceList = new Dictionary<string, AudioSource>();

        m_pEmpty = new GameObject("SoundList");
        m_pEmpty.transform.SetParent(transform);
        m_pEmpty.transform.localPosition = Vector3.zero;

        for (int i = 0; i < m_pSoundList.Count; i++)
        {
            AudioSource audioSource = null;
            audioSource = m_pEmpty.AddComponent<AudioSource>();
            audioSource.clip = m_pSoundList[i].pAudio;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.minDistance = 10.0f;
            audioSource.maxDistance = 15.0f;
            if (m_pSoundList[i].SolidSound==true)
            {
                audioSource.minDistance = 1.0f;
                audioSource.maxDistance = 15.0f;

                audioSource.spatialBlend = 1.0f;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
            }

           

            m_pAudioSourceList.Add(m_pSoundList[i].sName, audioSource);
        }
    }

    public void Play(string sName, bool bLoop = false)
    {
        // return;
        if (m_pAudioSourceList == null) return;
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            if(bLoop==false)
            {
                m_pAudioSourceList[sName].PlayOneShot(m_pAudioSourceList[sName].clip);
                m_pAudioSourceList[sName].loop = bLoop;
            }
            else
            {
                m_pAudioSourceList[sName].Play();
                m_pAudioSourceList[sName].loop = bLoop;
            }

           
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("ERROR_Not Exist Sound : " + sName);
#endif
        }
    }

    

    public int GetListCount()
    {
        return m_pAudioSourceList.Count;
    }

    public bool IsPlay(string sName)
    {
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            return m_pAudioSourceList[sName].isPlaying;
        }
#if UNITY_EDITOR
        Debug.Log("ERROR_Not Exist Sound");
#endif
        return false;
    }

    public void SetPitch(string sName, float fValue)
    {
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            m_pAudioSourceList[sName].pitch = fValue;
        }
    }

    public void SetVolume(string sName, float fValue)
    {
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            m_pAudioSourceList[sName].volume = fValue;
        }
    }

    public void Stop(string sName)
    {
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            if (m_pAudioSourceList[sName].isPlaying == false) return;
            m_pAudioSourceList[sName].Stop();
            
        }
    }
    Coroutine m_pCorCPWF = null;
    public void ChangePlayWithFadeOut(string sName1, string sName2, bool bLoop = false)
    {
        AudioSource pAudio1 = null;
        AudioSource pAudio2 = null;
        if (m_pAudioSourceList.ContainsKey(sName1))
        {
            pAudio1=m_pAudioSourceList[sName1];
        }
        if (m_pAudioSourceList.ContainsKey(sName2))
        {
            pAudio2 = m_pAudioSourceList[sName2];
        }
        if (pAudio1 == null || pAudio2 == null) return;
        if(m_pCorCPWF!=null)
        {
            StopCoroutine(m_pCorCPWF);
            m_pCorCPWF = null;
        }
        m_pCorCPWF= StartCoroutine(Cor_ChangePlayWithFadeOut(pAudio1, pAudio2, bLoop));
    }

   
    IEnumerator Cor_ChangePlayWithFadeOut(AudioSource pAudio1, AudioSource pAudio2, bool bLoop)
    {
        float fOriVolum = pAudio1.volume;
        for(float i=0.0f;i<=1.0f;i+=Time.deltaTime)
        {
            pAudio1.volume = Mathf.Lerp(fOriVolum, 0.0f, i);
            yield return null;
        }
        pAudio1.Stop();
        pAudio1.volume = fOriVolum;


        pAudio2.Play();
        pAudio2.loop = bLoop;


        m_pCorCPWF = null;
        yield return null;
    }

    Coroutine m_pCorFO = null;
    public void FadeOut(string sName)
    {
      // if (IsPlay(sName) == false) return;
        AudioSource pAudio1 = null;
        if (m_pAudioSourceList.ContainsKey(sName))
        {
            pAudio1 = m_pAudioSourceList[sName];
        }
        if (pAudio1 == null) return;
        if (m_pCorFO != null)
        {
            StopCoroutine(m_pCorFO);
            m_pCorFO = null;
        }
        m_pCorFO = StartCoroutine(Cor_FadeOut(pAudio1));
    }

    IEnumerator Cor_FadeOut(AudioSource pAudio1)
    {
        float fOriVolum = pAudio1.volume;
        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime)
        {
            pAudio1.volume = Mathf.Lerp(fOriVolum, 0.0f, i);
            yield return null;
        }
        pAudio1.Stop();
        pAudio1.volume = fOriVolum;


        m_pCorFO = null;
        yield return null;
    }

    /// <summary>
    /// 모든 음악 중지 함수
    /// </summary>
    public void AllStop()
    {
        if (m_pAudioSourceList == null) return;
        foreach (AudioSource obj in m_pAudioSourceList.Values)
        {
            obj.Stop();
        }
    }

    public void Destroy()
    {
        if (m_pAudioSourceList == null)
            return;

        if (m_pAudioSourceList.Count == 0)
        {
            return;
        }

        foreach (AudioSource obj in m_pAudioSourceList.Values)
        {
            if (obj != null)
            {
                Destroy(obj);
                continue;
            }
        }
        m_pAudioSourceList.Clear();
        m_pAudioSourceList = null;

        

        for(int i=0;i<m_pSoundList.Count;i++)
        {
            m_pSoundList[i].Destroy();
        }
        m_pSoundList.Clear();
        m_pSoundList = null;


        Destroy(m_pEmpty);
        m_pEmpty = null;

      
    }
}
