using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class MessageData
{
    public string SenderId { get; set; }
    public string SenderName { get; set; }
    public string SenderServer {  get; set; }
    public string Content { get; set; }
    public string TimeStamp { get; set; }

    public MessageData(string senderId, string senderName, string senderServer, string content)
    {
        SenderId = senderId;
        SenderName = senderName;
        SenderServer = senderServer;
        Content = content;
        TimeStamp = DateTime.UtcNow.ToString("o");
    }
}
