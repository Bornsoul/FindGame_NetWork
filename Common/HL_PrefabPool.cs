using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PoolObject
{
    public GameObject m_pPrefab = null;
    public int m_nPool_Size;

    public void Destroy()
    {
        m_pPrefab = null;
    }
}

public class HL_PrefabPool : MonoBehaviour {
    public List<PoolObject> m_pPrefabList = null;

    Dictionary<string, List<GameObject>> m_pPoolList = null;
    Dictionary<string, List<GameObject>> m_pPoolList_Active = null;

    void Awake()
    {
        Init();
    }

    void OnDestroy()
    {
        Destroy();
    }

    void Update()
    {
        Update_Pooling();
    }

    private void Init()
    {
        if (m_pPrefabList == null) return;
        m_pPoolList = new Dictionary<string, List<GameObject>>();
        m_pPoolList_Active = new Dictionary<string, List<GameObject>>();

        for(int i=0;i< m_pPrefabList.Count;i++)
        {
            List<GameObject> pList = new List<GameObject>();
            List<GameObject> pList_Active = new List<GameObject>();
            string sPrefabName = m_pPrefabList[i].m_pPrefab.name;

            for (int j=0;j<m_pPrefabList[i].m_nPool_Size;j++)
            {
                GameObject pObj = Instantiate(m_pPrefabList[i].m_pPrefab) as GameObject;
                pObj.name = sPrefabName;// + "HLPP_" + j.ToString();
                pObj.transform.SetParent(transform);
                pObj.SetActive(false);
                
                pList.Add(pObj);
            }
            m_pPoolList.Add(sPrefabName, pList);
            m_pPoolList_Active.Add(sPrefabName, pList_Active);
        }
    }

    private void Destroy()
    {
        if (m_pPoolList_Active != null)
        {
            foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList_Active)
            {
                for (int i = 0; i < pObj.Value.Count; i++)
                {
                    ReturnObject(pObj.Value[i], pObj.Key);
                }
                pObj.Value.Clear();
            }
            m_pPoolList_Active.Clear();
            m_pPoolList_Active = null;
        }

        if (m_pPoolList!=null)
        {
            foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList)
            {
                for(int i=0;i<pObj.Value.Count;i++)
                {
                   // Debug.Log(pObj.Value[i].name);
                    GameObject.Destroy(pObj.Value[i]);
                    
                }
                pObj.Value.Clear();
            }
            m_pPoolList.Clear();
            m_pPoolList = null;
        }


        if (m_pPrefabList != null)
        {
            for (int i = 0; i < m_pPrefabList.Count; i++)
            {
                m_pPrefabList[i].Destroy();
            }
            m_pPrefabList.Clear();
        }
        m_pPrefabList = null;
    }

    private void Update_Pooling()
    {
       
        bool bLoop = false;
        GameObject pObj_Ac = null;
        do
        {
            bLoop = false;
            foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList_Active)
            {
                pObj_Ac = null;
                for (int i=0;i<pObj.Value.Count;i++)
                {
                    if(pObj.Value[i].activeSelf==false)
                    {
                        pObj_Ac = pObj.Value[i];
                        bLoop = true;
                        break;
                    }
                }
                if (pObj_Ac != null)
                {
                 //   Debug.Log(pObj_Ac.name + ", " + pObj.Key);
                    ReturnObject(pObj_Ac, pObj.Key);
                }
            }
       } while (bLoop == true);
    }

    public GameObject GetObject(string sPrefabName)
    {
        GameObject pObject = null;
        foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList)
        {
            if (pObj.Key != sPrefabName) continue;
            
            for(int i=0;i<pObj.Value.Count;i++)
            {
                if(pObj.Value[i].activeSelf==false)
                {
                    pObject = pObj.Value[i];
                    break;
                }
            }

            if (pObject != null)
            {
                pObject.SetActive(true);
                pObj.Value.Remove(pObject);
                break;
            }
            else
            {
                GameObject pPrefab = null;
                for(int i=0;i< m_pPrefabList.Count;i++)
                {
                    if(m_pPrefabList[i].m_pPrefab.name==sPrefabName)
                    {
                        pPrefab = m_pPrefabList[i].m_pPrefab;
                        break;
                    }
                }

                pObject = Instantiate(pPrefab) as GameObject;
                pObject.name = sPrefabName;// + "HLPP_" + pObj.Value.Count.ToString();
                pObject.transform.SetParent(transform);
                pObject.SetActive(true);
                break;
            }
        }

        foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList_Active)
        {
            if (pObj.Key == sPrefabName)
            {
              
                m_pPoolList_Active[sPrefabName].Add(pObject);
                break;
            }
        }

        
        return pObject;
    }

    void ReturnObject(GameObject pObject, string sKey)
    {
        foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList_Active)
        {
            if (pObj.Key == sKey)
            {
                m_pPoolList_Active[sKey].Remove(pObject);
                break;
            }
        }

        foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList)
        {
            if (pObj.Key == sKey)
            {
                m_pPoolList[sKey].Add(pObject);
                break;
            }
        }
    }

    public List<GameObject> GetList_Active(string sPrefabName)
    {
        foreach (KeyValuePair<string, List<GameObject>> pObj in m_pPoolList_Active)
        {
            if (pObj.Key == sPrefabName)
            {
                return pObj.Value;
            }
        }
        return null;
    }
}
