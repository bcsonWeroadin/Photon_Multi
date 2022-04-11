using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using TMPro;

public class ChatManager : MonoBehaviour , IChatClientListener
{
    [SerializeField] private TMP_InputField m_InputChat;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    private void init()
    {
        
    }


    public void OnClickSendButton()
    {
        
    }


    #region IChatClientListener implementation
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();

    }

    /// <summary>
    /// A user has unsubscribed from a public chat channel
    /// </summary>
    /// <param name="channel">Name of the chat channel</param>
    /// <param name="user">UserId of the user who unsubscribed</param>
    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();

    }

    #endregion
}
