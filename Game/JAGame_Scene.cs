using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class JAGame_Scene : HL_Singleton<JAGame_Scene>
{
    public enum eState
    {
        E_STATE_NONE = 0,
        E_STATE_READY,
        E_STATE_GAME,
        E_STATE_RESULT,
        E_STATE_GAMEOVER,
        E_STATE_DISCONNECT
    };

    public enum eTurn
    {
        E_TURN_MY = 0,
        E_TURN_YOU
    };

    public JAGame_PopupMng m_pPopup_Mng = null;
    public JAGame_Result m_pResult_Mng = null;

    public JAGame_TurnUI m_pTurnUI = null;
    public JAGame_ItemsMng m_pItem_Mng = null;
    public JAGame_UI_Mng m_pUI_Mng = null;

    public GameObject m_pReadyObj = null;

    public bool m_bGameOver;
    public float m_fTime = 10f;
    private float m_fCurrTime = 0f;
    private bool m_bCurrTime = false;

    public eState m_eState = eState.E_STATE_NONE;
    public eTurn m_eTurn;

    public eTurn m_eTurn_Remote;
    public eTurn m_eTurn_Local;

    public string m_sClickName = string.Empty;
    public bool m_bMyTurn = false;

    public int m_nMyCount = 0;
    public int m_nYouCount = 0;

    float m_fAmbTimeDt = 0f;
    int m_nAmbRand = 0;

    private void Awake()
    {
        if (TransportTCP.I == null)
        {
            Application.LoadLevel("Init");
            return;
        }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    void Start()
    {
        TransportTCP.I.RegisterEventHandler(EventCallback);

        Reset();
        GameStart();
        m_bGameOver = false;
    }

    IEnumerator Cor_LoadScene(float fTime)
    {
        yield return new WaitForSeconds(fTime);
        AutoFade.LoadLevel("Menu", 0.3f, 0.3f, Color.black);
    }

    /// <summary>
    /// 업데이트
    /// </summary>
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            HL_SoundMng.I.FadeOut("BGM", "bgm1");
            HL_SoundMng.I.FadeOut("BGM", "bgm2");

            switch (JAManager.I.m_eHost)
            {
                case JAManager.eHost.E_HOST_CLIENT:
                    TransportTCP.I.Disconnect();
                    break;
                case JAManager.eHost.E_HOST_SERVER:
                    TransportTCP.I.StopServer();
                    break;
            }

            //AutoFade.LoadLevel("Menu", 0.3f, 0.3f, Color.black);
        }

        m_fAmbTimeDt += Time.deltaTime;
        if (m_fAmbTimeDt >= NGUITools.RandomRange(5, 9))
        {
            m_nAmbRand = NGUITools.RandomRange(1, 4);
            HL_SoundMng.I.Play("AMB", "amb" + m_nAmbRand);
            HL_SoundMng.I.SetVolue("AMB", "amb" + m_nAmbRand, 0.6f);
            m_fAmbTimeDt = 0f;
        }

        switch (m_eState)
        {
            case eState.E_STATE_NONE:
                //DrawTurnMark();
                break;
            case eState.E_STATE_READY:
                Game_State_Ready();
                DrawTurnMark();


                break;
            case eState.E_STATE_GAME:
                TurnUpdate();
                DrawTurnMark();
                if (m_pUI_Mng.GetWorldTimerState() == false)
                {
                    GameResult();
                }

                break;
            case eState.E_STATE_RESULT:
                GameOver();
                m_eState = eState.E_STATE_NONE;
                break;
            case eState.E_STATE_DISCONNECT:
                DisConnection();
                break;
        }

    }

    public void DrawTurnMark()
    {
        if (m_eTurn_Local == m_eTurn)
        {
            m_pTurnUI.SetTurnObjectUpt(JAGame_TurnUI.eTurnState.E_TURN_YOUR);
        }
        else
        {
            m_pTurnUI.SetTurnObjectUpt(JAGame_TurnUI.eTurnState.E_TURN_MY);
        }
    }

    /// <summary>
    /// 게임시작
    /// </summary>
    public void GameStart()
    {
        m_eState = eState.E_STATE_READY;
        m_eTurn = eTurn.E_TURN_MY;



        HL_SoundMng.I.FadeOut("BGM", "bgm3");
        HL_SoundMng.I.Play("SFX", "stinger");
        HL_SoundMng.I.SetVolue("SFX", "stinger", 0.7f);

        HL_SoundMng.I.Play("SFX", "connect");
        HL_SoundMng.I.SetVolue("SFX", "connect", 1.2f);

        HL_SoundMng.I.Play("AMB", "amb_bgm", true);
        HL_SoundMng.I.SetVolue("AMB", "amb_bgm", 0.35f);

        HL_SoundMng.I.Play("AMB", "amb" + NGUITools.RandomRange(1, 4));
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        Debug.Log("Client Send : " + JAManager.I.m_sMyAccount);
        byte[] buffers = Encoding.UTF8.GetBytes(JAManager.I.m_sMyAccount);
        TransportTCP.I.Send(buffers, buffers.Length);

        if (TransportTCP.I.IsServer() == true)
        {
            m_eTurn_Local = eTurn.E_TURN_MY;
            m_eTurn_Remote = eTurn.E_TURN_YOU;
        }
        else
        {
            byte[] buffer = new byte[1024];
            int nRecvSize = TransportTCP.I.Receive(ref buffer, buffer.Length);

            Debug.Log(Encoding.UTF8.GetString(buffer));
            JAManager.I.m_sYouAccount = Encoding.UTF8.GetString(buffer).ToString();

            m_eTurn_Local = eTurn.E_TURN_YOU;
            m_eTurn_Remote = eTurn.E_TURN_MY;

        }
        m_pTurnUI.SetNameSet(JAManager.I.m_sMyAccount, JAManager.I.m_sYouAccount);
        m_pTurnUI.SetRateSet(JAManager.I.m_sMyAccount, JAManager.I.m_sYouAccount);

        m_nMyCount = 0;
        m_nYouCount = 0;
        m_bMyTurn = false;
        m_bCurrTime = false;
        StartCoroutine(Cor_Ready());

        m_bGameOver = false;
        Debug.Log("GameStart");
    }

    IEnumerator Cor_Ready()
    {
        yield return new WaitForSeconds(0.3f);
        HL_SoundMng.I.Play("SFX", "ready");
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
    }

    /// <summary>
    /// 게임 리스타트
    /// </summary>
    public void Reset()
    {
        m_eTurn = eTurn.E_TURN_MY;
        m_eState = eState.E_STATE_NONE;

        m_nMyCount = 0;
        m_nYouCount = 0;
        m_bMyTurn = false;

        Debug.Log("GameRestart");
    }

    /// <summary>
    /// 게임 준비
    /// </summary>
    void Game_State_Ready()
    {
        m_pReadyObj.gameObject.SetActive(true);
        m_fCurrTime += Time.deltaTime;


        if (m_bCurrTime == false)
        {
            if (m_fCurrTime >= 2f)
            {
                HL_SoundMng.I.Play("SFX", "start");
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
                m_bCurrTime = true;
            }
        }

        if (m_fCurrTime > 3f)
        {
            m_pUI_Mng.SetGameWorldTime(60);
            m_pUI_Mng.SetTimeStart(6);

            m_pReadyObj.gameObject.SetActive(false);
            if (TransportTCP.I.IsServer() == true)
            {
                HL_SoundMng.I.Play("BGM", "bgm1", true);
                JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
                m_pUI_Mng.SetBlackScreen(false);
            }
            else
            {
                HL_SoundMng.I.Play("BGM", "bgm2", true);
                JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
                JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);
                m_pUI_Mng.SetBlackScreen(true);
            }

            m_eState = eState.E_STATE_GAME;
        }
    }

    /// <summary>
    /// 턴 체크
    /// </summary>
    void TurnUpdate()
    {
        bool bSetMark = false;

        if (m_eTurn == m_eTurn_Local)
        {
            bSetMark = GetTurn_My();
        }
        else
        {
            bSetMark = GetTurn_Your();
        }

        if (bSetMark == false)
        {
            return;
        }
        else
        {
            GameResult();
        }
        m_eTurn = (m_eTurn == eTurn.E_TURN_MY) ? eTurn.E_TURN_YOU : eTurn.E_TURN_MY;

    }

    void FindCount()
    {
        int nCnt = 0;
        for (int i = 0; i < m_pItem_Mng.m_pItems.Length; i++)
        {
            if (m_pItem_Mng.m_pItems[i].m_bFinded == false)
            {
                nCnt += 1;
            }
        }
        m_pUI_Mng.SetCount(nCnt, m_pItem_Mng.m_pItems.Length);

    }

    void MyYouFightCount(int nMy, int nYou)
    {
        m_pUI_Mng.SetFightCount(nMy, nYou);
    }

    /// <summary>
    /// 내 턴
    /// </summary>
    /// <returns></returns>
    bool GetTurn_My()
    {
        string sData;

        if (m_bMyTurn == false)
        {
            StartCoroutine(Cor_BlackShow(false));
            m_pTurnUI.SetTurnRefresh();
            m_pUI_Mng.SetTimeStart(6);

            FindCount();
        }
        m_bMyTurn = true;

        if (m_pUI_Mng.GetTimerState() == false)
        {
            string sDa = "-100,-100," + JAManager.I.m_sMyAccount + ",empty";
            byte[] buffers = Encoding.UTF8.GetBytes(sDa);
            TransportTCP.I.Send(buffers, buffers.Length);
            m_pUI_Mng.SetTimeStart(6);

            StartCoroutine(Cor_BlackShow(true));
            return true;
        }

        bool bClicked = Input.GetMouseButtonUp(0);
        if (bClicked == false)
        {
            return false;
        }

        Vector2 stMouse = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        sData = Input.mousePosition.x.ToString() + "," + Input.mousePosition.y.ToString();
        sData += "," + JAManager.I.m_sMyAccount;

        if (Input.GetMouseButtonUp(0))
        {

            if (m_sClickName == "")
            {
                sData += "," + "empty";
                m_pPopup_Mng.Create_Corss(stMouse, JAManager.I.m_sMyAccount);
            }
            else
            {
                sData += "," + m_sClickName;

                for (int i = 0; i < m_pItem_Mng.m_pItems.Length; i++)
                {
                    if (m_pItem_Mng.m_sItemName[i] == m_sClickName)
                    {
                        m_nMyCount += 1;
                        MyYouFightCount(m_nMyCount, m_nYouCount);
                        sData += "," + m_pItem_Mng.m_pItems[i].transform.localPosition.x.ToString() + "," + m_pItem_Mng.m_pItems[i].transform.localPosition.y.ToString();
                        m_pItem_Mng.m_pItems[i].SetCreateCircle(JAManager.I.m_sMyAccount);
                    }
                }

                m_sClickName = "";
                FindCount();
                m_pItem_Mng.SetItemNameUdt();
            }

            StartCoroutine(Cor_BlackShow(true));
        }
        m_pUI_Mng.SetTimeStart(6);

        byte[] buffer = Encoding.UTF8.GetBytes(sData);
        TransportTCP.I.Send(buffer, buffer.Length);

        return true;
    }

    /// <summary>
    /// 상대방 턴
    /// </summary>
    /// <returns></returns>
    bool GetTurn_Your()
    {

        byte[] buffer = new byte[1024];
        int nRecvSize = TransportTCP.I.Receive(ref buffer, buffer.Length);

        if (nRecvSize <= 0)
        {
            return false;
        }

        if (m_bMyTurn == true)
        {
            m_pTurnUI.SetTurnRefresh();
        }

        m_bMyTurn = false;

        string sText = Encoding.UTF8.GetString(buffer);
        string[] sSplit = sText.Split(',');

        float fX = Convert.ToSingle(sSplit[0].ToString());
        float fY = Convert.ToSingle(sSplit[1].ToString());
        Vector2 stPos = new Vector2(fX, fY);
        Vector2 stMouse = UICamera.mainCamera.ScreenToWorldPoint(stPos);

        JAManager.I.m_sYouAccount = sSplit[2];
        m_pTurnUI.SetNameSet(JAManager.I.m_sMyAccount, JAManager.I.m_sYouAccount);
        m_pTurnUI.SetRateSet(JAManager.I.m_sMyAccount, JAManager.I.m_sYouAccount);

        if (sSplit[3].Contains("empty"))
        {
            m_pPopup_Mng.Create_Corss(stMouse, sSplit[2]);
        }
        else
        {
            Vector2 stCirPos = new Vector2(Convert.ToSingle(sSplit[4]), Convert.ToSingle(sSplit[5]));
            m_pPopup_Mng.Create_Circle(stCirPos, sSplit[2]);

            m_nYouCount += 1;
            MyYouFightCount(m_nMyCount, m_nYouCount);


            for (int i = 0; i < m_pItem_Mng.m_pItems.Length; i++)
            {
                if (sSplit[3].Contains(m_pItem_Mng.m_sItemName[i]))
                {
                    m_pItem_Mng.m_pItems[i].m_bFinded = true;
                }
            }
            FindCount();
            m_pItem_Mng.SetItemNameUdt();

        }

        return true;
    }

    bool GameResult()
    {

        int nCnt = 0;
        for (int i = 0; i < m_pItem_Mng.m_pItems.Length; i++)
        {
            if (m_pItem_Mng.m_pItems[i].m_bFinded == false)
            {
                nCnt += 1;
            }
        }

        if (nCnt <= 0)
        {
            Debug.Log("결과");
            m_pUI_Mng.SetBlackScreen(false);
            StartCoroutine(Cor_Result());
            m_eState = eState.E_STATE_RESULT;

            return false;
        }

        return true;
    }

    IEnumerator Cor_Result()
    {
        JAPopupManager.I.Create_Notice("게임 종료!", 2f);
        yield return new WaitForSeconds(1.3f);

        HL_SoundMng.I.FadeOut("BGM", "bgm1");
        HL_SoundMng.I.FadeOut("BGM", "bgm2");
        HL_SoundMng.I.FadeOut("AMB", "amb_bgm");

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        yield return new WaitForSeconds(0.2f);

        HL_SoundMng.I.StopTheme("BGM");
        HL_SoundMng.I.StopTheme("SFX");
        HL_SoundMng.I.StopTheme("AMB");

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        yield return new WaitForSeconds(0.5f);

        HL_SoundMng.I.Play("SFX", "stinger");
        HL_SoundMng.I.SetVolue("SFX", "stinger", 0.45f);

        HL_SoundMng.I.Play("SFX", "result1");
        HL_SoundMng.I.Play("SFX", "result2");
        HL_SoundMng.I.SetVolue("SFX", "result1", 0.45f);
        HL_SoundMng.I.SetVolue("SFX", "result2", 0.55f);
        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        ResultCheck();

        yield return null;
    }

    bool ResultCheck()
    {

        if (m_nMyCount == m_nYouCount)
        {
            m_pResult_Mng.Enter("상대방과 비겼습니다.. 무승부!");
            HL_SoundMng.I.Play("BGM", "draw");
            JAManager.I.SetUpdate_Rate(int.Parse(JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_DRAW, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount)) + 1,
                                       JAManager.eRate.E_RATE_DRAW, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount);

            m_eState = eState.E_STATE_RESULT;

            return false;
        }

        if (m_nMyCount >= m_nYouCount)
        {
            m_pResult_Mng.Enter("상대방을 이겼습니다!");
            HL_SoundMng.I.Play("BGM", "win");
            JAManager.I.SetUpdate_Rate(int.Parse(JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_WIN, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount)) + 1,
                                       JAManager.eRate.E_RATE_WIN, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount);
            m_eState = eState.E_STATE_RESULT;

            return false;
        }
        else
        {
            m_pResult_Mng.Enter("상대방에게 졌습니다..");
            HL_SoundMng.I.Play("BGM", "lose");
            JAManager.I.SetUpdate_Rate(int.Parse(JAManager.I.GetUser_Rate(JAManager.eRate.E_RATE_LOSE, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount)) + 1,
                                       JAManager.eRate.E_RATE_LOSE, JAManager.I.GetUser_UID(JAManager.I.m_sMyAccount), JAManager.I.m_sMyAccount);

            m_eState = eState.E_STATE_RESULT;
            return false;
        }
    }

    void GameOver()
    {
        m_pUI_Mng.SetTimerState(false);
        m_pUI_Mng.SetWorldTimerState(false);
        m_bGameOver = true;
    }

    IEnumerator Cor_BlackShow(bool bShow)
    {
        //HL_SoundMng.I.ChangePlayWithFadeOut("BGM", "bgm1", "bgm2", true);
        HL_SoundMng.I.FadeOut("BGM", "bgm1");
        HL_SoundMng.I.Play("BGM", "bgm2", true);

        HL_SoundMng.I.Play("SFX", "stinger");
        HL_SoundMng.I.SetVolue("SFX", "stinger", 0.35f);

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        yield return new WaitForSeconds(0.5f);

        m_pUI_Mng.SetBlackScreen(bShow);
    }

    /// <summary>
    /// 연결 끊기고나서 후처리
    /// </summary>
    private void DisConnection()
    {
        if (m_bGameOver == true) return;
        m_bGameOver = true;

        m_eState = eState.E_STATE_NONE;

        switch (JAManager.I.m_eHost)
        {
            case JAManager.eHost.E_HOST_CLIENT:
                TransportTCP.I.Disconnect();
                break;
            case JAManager.eHost.E_HOST_SERVER:
                TransportTCP.I.StopServer();
                break;
        }

        JAPopupManager.I.Create_Notice("상대방의 연결이 끊기거나 나갔습니다.", 1.8f);


        StartCoroutine(Cor_Disconnect());

    }

    IEnumerator Cor_Disconnect()
    {
        yield return new WaitForSeconds(2f);
        HL_SoundMng.I.Play("SFX", "disconnect");
        HL_SoundMng.I.FadeOut("BGM", "bgm1");
        HL_SoundMng.I.FadeOut("BGM", "bgm2");
        HL_SoundMng.I.FadeOut("AMB", "amb_bgm");

        JAManager.I.SoundBGMMute(JAManager.I.m_bSoundBGMMute);
        JAManager.I.SoundSFXMute(JAManager.I.m_bSoundSFXMute);

        Reset();
        m_bGameOver = true;
        AutoFade.LoadLevel("Menu", 0.3f, 0.3f, Color.black);
    }

    /// <summary>
    /// 연결 상태인지 확인하는 콜백함수
    /// </summary>
    /// <param name="state"></param>
    public void EventCallback(NetEventState state)
    {
        switch (state.type)
        {
            case NetEventType.Disconnect:
                if (m_eState < eState.E_STATE_RESULT && m_bGameOver == false)
                    m_eState = eState.E_STATE_DISCONNECT;
                break;
        }
    }
}
