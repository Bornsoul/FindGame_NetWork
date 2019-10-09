using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAGame_ItemsMng : MonoBehaviour
{
    public JAGame_SelectItem[] m_pItems = null;
    public UILabel m_pLbl_List = null;

    string m_sItemList = string.Empty;
    public List<string> m_sItemName = new List<string>();
    private List<string> m_sItemCopy = new List<string>();
    void Start()
    {
        for ( int i = 0; i<m_pItems.Length; i++)
        {
            m_pItems[i].m_nIndex = i;
            m_sItemList += "[" + m_pItems[i].m_sName + "]  ";
            m_sItemName.Add(m_pItems[i].m_sName);
        }

        m_pLbl_List.text = m_sItemList;
    }

    public void SetItemNameUdt()
    {
        m_sItemCopy.Clear();
        m_sItemList = "";

        for (int i = 0; i < m_pItems.Length; i++)
        {
            m_sItemCopy.Add(m_pItems[i].m_sName);
        }

        for (int i = 0; i < m_pItems.Length; i++)
        {
            if (m_pItems[i].m_bFinded == true)
            {
                m_sItemCopy[i] = "[858585FF]" + m_sItemCopy[i] + "[-]";
                
            }
            m_sItemList += "[" + m_sItemCopy[i] + "]  ";
        }

        m_pLbl_List.text = m_sItemList;
    }
}
