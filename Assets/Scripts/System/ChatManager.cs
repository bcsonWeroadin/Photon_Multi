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

    public ChatClient chatClient;
    public string UserName { get; set; }

    private string currentChannelName;

    //protected internal ChatAppSettings chatAppSettings;

    private void Start()
    {
        DontDestroyOnLoad(this);
        UserName = "User" + Random.Range(1, 101);
        
        //bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

        currentChannelName = "abc";

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
        AddLine(string.Format("����õ�", UserName));
        Debug.Log("�����̸�: "+ UserName);
        // ������ ä�θ����� ����

        /*        
                 this.chatClient.ConnectUsingSettings(this.chatAppSettings);

                this.ChannelToggleToInstantiate.gameObject.SetActive(false);
                Debug.Log("Connecting as: " + this.UserName);

                this.ConnectingLabel.SetActive(true);
        */
    }

    // ���� ä�� ���¸� ������� UI.Text
    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    // ������ ��ư Ŭ��
    public void OnClickSendButton()
    {
        Debug.Log("������ ��ư Ŭ��");
            //this.chatClient.PublishMessage(currentChannelName, m_InputChat.text.ToString());

        if ( !string.IsNullOrEmpty(m_InputChat.text) )
        {
            this.chatClient.PublishMessage(currentChannelName, m_InputChat.text.ToString());
            Debug.Log("ä�� ����");

        }


        m_InputChat.text = "";
    }


    #region IChatClientListener implementation
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        //throw new System.NotImplementedException();
    }

    // �������� ������ ������

    public void OnDisconnected()
    {
        AddLine("������ ������ ���������ϴ�.");
        Debug.Log("ä�� ���� ���� ������");

        //Connect();
        //Debug.Log("�翬��");

        //throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        AddLine("������ ���� �Ǿ����ϴ�.");

        Debug.Log("ä�� ���� ���� �����");

        // ä�� ä�� ����
        chatClient.Subscribe(new string[] { currentChannelName }, 10);

        //throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        AddLine("OnChatStateChange = " + state);
        //Debug.Log("ä�� ���� �����");

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
            // ä�ο� ���� �� ��� ä�� �޼����� �ҷ��´�.
            // ���� �̸��� ä�� ������ �Ѳ����� �ҷ�������.
            this.outputText.text = channel.ToStringMessages();

            Debug.Log("senders: " + senders[senders.Length - 1]);           // ���������� ���� ���
            Debug.Log("messages: " + messages[messages.Length - 1]);        // ���������� ���� �޽���

            if ( UserName.Equals(senders[senders.Length - 1]) )
            {

            }



        }
        //throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
        //throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
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
