using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class MessageConstructor
{
    public static NetworkMessageSender MessageSender;
    public static void BuildAndSendMessage(MessageRequest request)
    {
        
        switch (request.MessageType)
        {
            case MessageType.PlainMessage:
                MessageContent content;

                content = new MessageContent(request.Sender, 
                                             request.MessageType,
                                             new PlainMessageData(request.PlainMessageRequestData));
                MessageSender.SendChatMessageToClients(content);
                break;
            case MessageType.RollMessage:
                _ = SendRollMessage(request);
                break;
        }
    }

    private static async Task SendRollMessage(MessageRequest request)
    {
        await Awaitable.WaitForSecondsAsync(0);
        Debug.Log("Rolling");
        MessageContent content;

        Dictionary<DiceType, int> dice = DiceParser.ParseDiceFromString(request.RollMessageRequestData.Dice);
        List<Task<int[]>> rollTasks = new List<Task<int[]>>();

        foreach (var d in dice)
        {
            rollTasks.Add(DiceRoller.Instance.RollDice(d.Key, d.Value));
        }

        int[][] allResults = await Task.WhenAll(rollTasks);

        var final = allResults.SelectMany(r => r).ToArray();

        content = new MessageContent(request.Sender,
                                     request.MessageType,
                                     rollData: new RollMessageData(request.RollMessageRequestData, final));

        MessageSender.SendChatMessageToClients(content);
    }
}
