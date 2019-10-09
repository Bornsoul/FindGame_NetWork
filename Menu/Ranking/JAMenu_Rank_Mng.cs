using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JAMenu_Rank_Mng : MonoBehaviour
{
    public JAMenu_RankMng m_pRoot = null;
    public HL_PrefabPool m_pObject = null;
    public UIScrollView m_pScrollView = null;
    public UIGrid m_pGrid = null;
    
    public void Enter()
    {
        
        for (int i = 0; i< JAManager.I.GetSearchLenght(); i++)
        {
            Create_Item((i+1), JAManager.I.GetUser_Ranking(JAManager.eRanking.E_RANK_ACCOUNT)[i]);
        }

        m_pScrollView.ResetPosition();
        m_pGrid.Reposition();
    }

    public void Destroy()
    {
        StartCoroutine(Cor_Destroy());
    }

    IEnumerator Cor_Destroy()
    {
        List<GameObject> pObj = m_pObject.GetList_Active("Item_Ranking");

        for (int i = 0; i < pObj.Count; i++)
        {
            JAMenu_Rank_Item pSrc = pObj[i].GetComponent<JAMenu_Rank_Item>();

            pSrc.gameObject.SetActive(false);
        }
        while (true)
        {
            if (pObj.Count == 0) break;
            yield return null;
        }

        m_pRoot.transform.gameObject.SetActive(false);
        yield return null;
    }

    public JAMenu_Rank_Item Create_Item(int nCount, string sAccount)
    {
        JAMenu_Rank_Item pObj = m_pObject.GetObject("Item_Ranking").GetComponent<JAMenu_Rank_Item>();

        if (pObj == null) return null;
        pObj.transform.localPosition = Vector3.zero;
        pObj.transform.localScale = Vector3.one;
        pObj.Enter(nCount, sAccount);

        return pObj;
    }
}
