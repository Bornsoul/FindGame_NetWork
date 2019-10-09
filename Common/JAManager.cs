using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;
using System.Data;
using System;

public class JAManager : HL_Singleton<JAManager>
{
    public enum eHost
    {
        E_HOST_CLIENT = 0,
        E_HOST_SERVER
    };

    public enum eRanking
    {
        E_RANK_ACCOUNT = 0,
        E_RANK_WIN,
        E_RANK_LOSE,
        E_RANK_DRAW,
        E_RANK_LOGIN
    };

    public enum eRate
    {
        E_RATE_DRAW,
        E_RATE_WIN,
        E_RATE_LOSE
    };


    public eHost m_eHost = eHost.E_HOST_CLIENT;
    private static string m_gServer = "45.112.165.106";
    private static string m_gID = "ckgame";
    private static string m_gPass = "dkagh1234.";

    public string m_sMyUID = string.Empty;
    public string m_sMyAccount = string.Empty;
    public string m_sYouAccount = string.Empty;

    public bool m_bSoundBGMMute = true;
    public bool m_bSoundSFXMute = true;
    public float m_fSoundMasterVol = 1f;

    public bool m_bLoginScene = false;

    void Start()
    {
        DontDestroyOnLoad(this);

        //Debug.Log("Count : " + GetUserUID());


        //CreateUser("test", "dkagh");
        //UpdateUserWIN("test", "1");
    }

    void OnApplicationQuit()
    {
        if (m_sMyAccount != "")
        {
            if (GetUser_LoginCheck(m_sMyAccount) >= 1)
                SetUpdate_LoginCheck(m_sMyAccount, 0);
        }

    }

    /// <summary>
    /// 서버 상태 체크
    /// </summary>
    /// <param name="fTime"></param>
    /// <returns></returns>
    public bool StartServerAliveCheck(float fTime = 1.5f)
    {
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {
            try
            {
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                JAPopupManager.I.Create_Notice("서버가 닫혀있거나 인터넷이 불안정합니다.", fTime);
                return false;
            }
        }
    }

    public void SoundBGMMute(bool bTrue)
    {
        m_bSoundBGMMute = bTrue;

        if (m_bSoundBGMMute == false)
            HL_SoundMng.I.StopTheme("BGM");
    }

    public void SoundSFXMute(bool bTrue)
    {
        m_bSoundSFXMute = bTrue;

        if (m_bSoundSFXMute == false)
            HL_SoundMng.I.StopTheme("SFX");
    }

    public void SoundVolume(float fValue)
    {
        AudioListener.volume = fValue;
    }

    /// <summary>
    /// 검색한 데이터의 존재하는 갯수
    /// WhereCode 추가시 ULOGIN='1' 식으로 풀로 작성
    /// </summary>
    /// <param name="sLengh"></param>
    /// <returns></returns>
    public int GetSearchLenght(string sLengh = "UID", string sWhereCode = "")
    {
        if (StartServerAliveCheck() == false) return -1;

        int nCnt = 0;
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        bool bWhere = sWhereCode == "" ? bWhere = false : bWhere = true;

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {
            conn.Open();
            string sql = string.Empty;

            if ( bWhere == false)
                sql = string.Format("SELECT {0} FROM userinfo", sLengh);
            else
                sql = string.Format("SELECT {0} FROM userinfo WHERE {1}", sLengh, sWhereCode);

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                nCnt++;

            }
            rdr.Close();
        }
        return nCnt;
    }

    /// <summary>
    /// 검색한것을 계정에서 정보를 읽음
    /// </summary>
    /// <param name="sAccount"></param>
    /// <param name="sFind"></param>
    /// <returns></returns>
    public string GetSearchAccount(string sAccount, string sFind)
    {
        if (StartServerAliveCheck() == false) return string.Empty;

        string sText = string.Empty;
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Format("select {0} from userinfo where UACCOUNT='{1}'", sFind, sAccount);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                sText = rdr[0].ToString();
            }

            rdr.Close();
        }

        return sText;
    }

    public string GetUser_UID(string sAccount)
    {
        if (StartServerAliveCheck() == false) return string.Empty;

        string sText = string.Empty;
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Format("select UID from userinfo where UACCOUNT='{0}'", sAccount);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                sText = rdr[0].ToString();
            }

            rdr.Close();
        }

        return sText;
    }

    /// <summary>
    /// 검색 결과 하나를 테이블 전체로 뽑아냄
    /// </summary>
    /// <param name="sFind"></param>
    /// <returns></returns>
    public List<string> GetSearchTables(string sFind, string sWhereCode = "")
    {
        if (StartServerAliveCheck() == false) return null;

        List<string> sList = new List<string>();
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        bool bWhere = sWhereCode == "" ? bWhere = false : bWhere = true;

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Empty;

            if( bWhere == false )
                sql = string.Format("select {0} from userinfo", sFind);
            else
                sql = string.Format("select {0} from userinfo where {1}", sFind, sWhereCode);

            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                sList.Add(rdr[0].ToString());
            }

            rdr.Close();
        }

        return sList;
    }

    public bool SetUser_AccountInfo(string sID, string sPass)
    {
        if (StartServerAliveCheck() == false) return false;

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();

            string sql = "SELECT UACCOUNT, UPASSWORD, UID FROM userinfo";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (sID == rdr[0].ToString() && sPass == rdr[1].ToString())
                {
                    m_sMyAccount = rdr[0].ToString();
                    m_sMyUID = rdr[2].ToString();
                    return true;
                }
            }
            rdr.Close();
        }
        return false;
    }

    /// <summary>
    /// 계정이 존재하는지 체크
    /// </summary>
    /// <param name="sAccount"></param>
    /// <returns></returns>
    public bool GetUser_AccountCheck(string sAccount)
    {
        if (StartServerAliveCheck() == false) return false;

        int nCnt = 0;
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Format("select UACCOUNT from userinfo where UACCOUNT='{0}';", sAccount);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                nCnt++;
                if (nCnt >= 1)
                {
                    return true;
                }
            }


            rdr.Close();
        }


        return false;
    }

    public bool CreateUser(string sID, string sPass)
    {
        if (StartServerAliveCheck() == false) return false;

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        
        using (MySqlConnection pSqlCon = new MySqlConnection(m_sConnect))
        {
            pSqlCon.Open();
            string sql = string.Format("INSERT INTO userinfo values({0}, '{1}', '{2}', '{3}', 0, 0, 0, 0)", GetSearchLenght() + 1, sID, sPass, sID);

            MySqlCommand pCmd = new MySqlCommand(sql, pSqlCon);
            pCmd.ExecuteNonQuery();
        }
        return true;
    }

    public void SetUpdate_Rate(int nVal, eRate eDataRate, string sUID = "", string sAccount = "")
    {
        if (StartServerAliveCheck() == false) return;

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        string sID = sUID == "" ? m_sMyUID : sUID;
        string sAcc = sAccount == "" ? m_sMyAccount : sAccount;
        string sRate = string.Empty;

        switch (eDataRate)
        {
            case eRate.E_RATE_DRAW:
                sRate = "UDRAW";
                break;
            case eRate.E_RATE_WIN:
                sRate = "UWIN";
                break;
            case eRate.E_RATE_LOSE:
                sRate = "ULOSE";
                break;
        }

        using (MySqlConnection m_pSqlCon = new MySqlConnection(m_sConnect))
        {
            m_pSqlCon.Open();

            string sSql = string.Format("UPDATE userinfo SET {0}='{1}' where UID='{2}' and UACCOUNT='{3}'", sRate, nVal.ToString(), sID, sAcc);
            MySqlCommand pCmd = new MySqlCommand(sSql, m_pSqlCon);
            pCmd.ExecuteNonQuery();

        }
    }

    public string GetUser_Rate(eRate eDataRate, string sUID = "", string sAccount = "")
    {
        if (StartServerAliveCheck() == false) return string.Empty;
        string sString = string.Empty;
        string sID = sUID == "" ? m_sMyUID : sUID;
        string sAcc = sAccount == "" ? m_sMyAccount : sAccount;

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        string sRate = string.Empty;

        switch (eDataRate)
        {
            case eRate.E_RATE_DRAW:
                sRate = "UDRAW";
                break;
            case eRate.E_RATE_WIN:
                sRate = "UWIN";
                break;
            case eRate.E_RATE_LOSE:
                sRate = "ULOSE";
                break;
        }

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Format("SELECT {0} FROM userinfo where UID='{1}' and UACCOUNT='{2}'", sRate, sID, sAcc);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                sString = rdr[0].ToString();
            }
            rdr.Close();
        }
        return sString;

    }

    public void SetUpdate_LoginCheck(string sAccount = "", int nLogin = 0)
    {
        if (StartServerAliveCheck() == false) return;
        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        string sAcc = sAccount == "" ? m_sMyAccount : sAccount;

        using (MySqlConnection m_pSqlCon = new MySqlConnection(m_sConnect))
        {
            m_pSqlCon.Open();

            string sSql = string.Format("UPDATE userinfo SET ULOGIN='{0}' where UACCOUNT='{1}'", nLogin.ToString(), sAcc);
            MySqlCommand pCmd = new MySqlCommand(sSql, m_pSqlCon);
            pCmd.ExecuteNonQuery();
        }
    }

    public int GetUser_LoginCheck(string sAccount = "")
    {
        if (StartServerAliveCheck() == false) return -1;
        int nCheck = 0;
        string sAcc = sAccount == "" ? m_sMyAccount : sAccount;

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);

        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {

            conn.Open();
            string sql = string.Format("SELECT ULOGIN FROM userinfo where UACCOUNT='{0}'", sAcc);
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr[0].ToString() == "NULL")
                {
                    nCheck = 0;
                }
                else
                {
                    nCheck = int.Parse(rdr[0].ToString());
                }
            }
            rdr.Close();
        }
        return nCheck;
    }

    public List<string> GetUser_Ranking(eRanking eRankInfo = eRanking.E_RANK_ACCOUNT)
    {
        if (StartServerAliveCheck() == false) return null;
        List<string> pList = new List<string>();

        string m_sConnect = string.Format("Server={0};Database=ck_netgame;Uid={1};Pwd={2}", m_gServer, m_gID, m_gPass);
        using (MySqlConnection conn = new MySqlConnection(m_sConnect))
        {
            conn.Open();
            string sql = "select UACCOUNT, UWIN, ULOSE, UDRAW, ULOGIN from userinfo order by UWIN desc, ULOSE asc";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                pList.Add(rdr[(int)eRankInfo].ToString());
            }
            rdr.Close();
        }

        return pList;
    }
}
