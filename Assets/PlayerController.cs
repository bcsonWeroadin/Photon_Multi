using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.AI;
using UniRx.Triggers;
using UniRx;
using UnityEngine.EventSystems;
using TMPro;

public class PlayerController : MonoBehaviour//, IPunObservable
{
    [SerializeField] private PhotonView PV;
    private TMP_Text PlayerBubbleText;
    private GameObject chatView;

    private string m_message;
    private void Start()
    {
        PV = GetComponent<PhotonView>();

        // 말풍선 가져오기
        chatView = transform.Find("ChatView").gameObject;
        chatView.SetActive(false);
        PlayerBubbleText = chatView?.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        PlayerBubbleText.text = "";
    }

    public void ShowSpeechBubble(string message)
    {
        m_message = message;
        Debug.Log("message 수정: " + m_message);

        PV.RPC("ShowSpeechRPC", RpcTarget.All, message);
    }

    [PunRPC]
    private void ShowSpeechRPC(string message)
    {
        StopCoroutine("ActiveSpeechBubble");
        
        //PlayerBubbleText.text = m_message;
        PlayerBubbleText.text = message;

        StartCoroutine("ActiveSpeechBubble");
    }

    IEnumerator ActiveSpeechBubble()
    {
        chatView.SetActive(true);

        yield return new WaitForSeconds(3f);

        chatView.SetActive(false);
    }

    // 지속적인 데이터 업데이트
/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(m_message);
            Debug.Log("SendNext message");

        }
        else
        {
            // Network player, receive data
            m_message = (string)stream.ReceiveNext();
            Debug.Log("ReceiveNext message");
        }

        ShowSpeech();

        //PV.RPC("ShowSpeechRPC", RpcTarget.All);

        *//*        stream.SendNext(m_message);
                m_message = (string)stream.ReceiveNext();
        *//*

    }*/
}
