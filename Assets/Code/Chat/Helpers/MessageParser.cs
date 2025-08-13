using UnityEngine;
using System.Collections.Generic;
using System;
public class MessageParser : MonoBehaviour
{
    [SerializeField] private NetworkMessageSender _networkMessageSender;
    private Dictionary<ChatCommands, Func<ulong, string, bool>> _commands = new Dictionary<ChatCommands, Func<ulong, string, bool>>();
    
    private void Awake()
    {
        _commands.Clear();

        _commands.Add(ChatCommands.r, SendRollMessage);
        _commands.Add(ChatCommands.roll, SendRollMessage);
    }
    public bool TryParseAndSendMessage(ulong clientId, string message)
    {
        if (message.Length < 1)
            return false;

        message = message.ToLower();

        if (message[0] == '/')
        {
            message = message.Substring(1);
            string[] splits = message.Split(' ');

            if (!Enum.TryParse<ChatCommands>(splits[0], out ChatCommands cmd))
                return false;

            message = message.Substring(splits[0].Length - 1);

            if (_commands.TryGetValue(cmd, out Func<ulong, string, bool> action))
                return action.Invoke(clientId, message);
        }

        MessageRequest request = new MessageRequest(clientId.ToString(), MessageType.PlainMessage, plainData: new PlainMessageRequestData(message));
        return false;
    }

    private bool SendRollMessage(ulong clientId, string message)
    {


        return false;
    }
}
