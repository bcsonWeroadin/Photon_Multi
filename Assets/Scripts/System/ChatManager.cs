using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using AuthenticationValues = Photon.Chat.AuthenticationValues;

public class ChatManager : MonoBehaviour , IChatClientListener
{
    [SerializeField] private TMP_InputField m_InputChat;
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private MultiManager multi;
    [SerializeField] private PhotonView PV;

    public ChatClient chatClient;
    public string UserName { get; set; }

    private string currentChannelName;
    private TMP_Text PlayerBubbleText;
    private GameObject chatView;
    private string m_message;
    private GameObject PlayerInstance;
    //protected internal ChatAppSettings chatAppSettings;


    private void Start()
    {
        DontDestroyOnLoad(this);
        UserName = "User" + Random.Range(1, 101);
        
        //bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        currentChannelName = "abc";
        PV = GetComponent<PhotonView>();

    }

    public void Init()
    {

    }

    void Update()
    {
        if (this.chatClient != null)
        {
            chatClient.Service();

            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                if (!(string.IsNullOrEmpty(m_InputChat.text)))
                {
                    this.chatClient.PublishMessage(currentChannelName, m_InputChat.text.ToString());
                }


                m_InputChat.text = "";
            }
        }
    }

    public void Connect()
    {
        //Debug.Log(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat);

        this.chatClient = new ChatClient(this);

    #if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
    #endif
        //chatClient.UseBackgroundWorkerForSending = true;

        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(UserName));
        AddLine(string.Format("????????", UserName));
        Debug.Log("????????: "+ UserName);

        // ?????? ?????????? ????

        /*        
                 this.chatClient.ConnectUsingSettings(this.chatAppSettings);

                this.ChannelToggleToInstantiate.gameObject.SetActive(false);
                Debug.Log("Connecting as: " + this.UserName);

                this.ConnectingLabel.SetActive(true);
        */



        // ?????? ????????
        chatView = multi?.PlayerInstance.transform.Find("ChatView").gameObject;
        //chatView.SetActive(false);
        PlayerBubbleText = chatView?.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
        PlayerBubbleText.text = "";
    }

    // ???? ???? ?????? ???????? UI.Text
    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    // ?????? ???? ????
    public void OnClickSendButton()
    {
        Debug.Log("?????? ???? ????");
            //this.chatClient.PublishMessage(currentChannelName, m_InputChat.text.ToString());

        if ( !string.IsNullOrEmpty(m_InputChat.text) )
        {
            this.chatClient.PublishMessage(currentChannelName, m_InputChat.text.ToString());
            Debug.Log("???? ????");

        }


        m_InputChat.text = "";
    }


    #region IChatClientListener implementation
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    // ???????? ?????? ??????

    public void OnDisconnected()
    {
        AddLine("?????? ?????? ????????????.");
        Debug.Log("???? ???? ???? ??????");

        //Connect();
        //Debug.Log("??????");

        //throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        AddLine("?????? ???? ??????????.");

        Debug.Log("???? ???? ???? ??????");

        // ???? ???? ????
        chatClient.Subscribe(new string[] { currentChannelName }, 10);


        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        AddLine("OnChatStateChange = " + state);
        //Debug.Log("???? ???? ??????");

        //throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(currentChannelName))
        {
            // update text
            if (string.IsNullOrEmpty(channelName))
            {
                return;
            }

            ChatChannel channel = null;
            bool found = this.chatClient.TryGetChannel(channelName, out channel);
            if (!found)
            {
                Debug.Log("ShowChannel failed to find channel: " + channelName);
                return;
            }

            //this.currentChannelName = channelName;
            // ?????? ???? ?? ???? ???? ???????? ????????.
            // ???? ?????? ???? ?????? ???????? ??????????.
            this.outputText.text = channel.ToStringMessages();

/*            Debug.Log("senders: " + senders[senders.Length - 1]);           // ?????????? ???? ????
            Debug.Log("messages: " + messages[messages.Length - 1]);        // ?????????? ???? ??????*/

            if ( UserName.Equals(senders[senders.Length - 1]) )
            {
                m_message = messages[messages.Length - 1].ToString();
                multi?.PlayerInstance.GetComponent<PlayerController>().ShowSpeechBubble(m_message);
                //PV.RPC("ShowSpeech", RpcTarget.All);

            }



        }
        //throw new System.NotImplementedException();
    }
/*
    [PunRPC]
    private void ShowSpeech()
    {
        StopCoroutine("ShowSpeechBubble");
        PlayerBubbleText.text = m_message;
        StartCoroutine("ShowSpeechBubble");
    }

    IEnumerator ShowSpeechBubble()
    {
        chatView.SetActive(true);

        yield return new WaitForSeconds(5f);

        chatView.SetActive(false);

    }*/

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("???? ???? ({0})", string.Join(",", channels)));
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("???? ???? ({0})", string.Join(",", channels)));
        //throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();

    }

    /// <summary>
    /// A user has unsubscribed from a public chat channel
    /// </summary>
    /// <param name="channel">Name of the chat channel</param>
    /// <param name="user">UserId of the user who unsubscribed</param>
    public void OnUserUnsubscribed(string channel, string user)
    {
       // throw new System.NotImplementedException();

    }

    #endregion
}
