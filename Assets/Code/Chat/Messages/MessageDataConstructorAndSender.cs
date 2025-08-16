using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public static class MessageDataConstructorAndSender
{
    public static void BuildAndSendMessage(MessageRequest request, NetworkMessageSender networkMessageSender)
    {
        
        switch (request.MessageType)
        {
            case MessageType.PlainMessage:
                MessageContent content;

                content = new MessageContent(PlayersData.Instance.PlayerNames[request.Sender], 
                                             request.MessageType,
                                             new PlainMessageData(request.PlainMessageRequestData));
                networkMessageSender.SendChatMessageToClients(content);
                break;
            case MessageType.RollMessage:
                _ = SendRollMessage(request, networkMessageSender);
                break;
        }
    }

    private static async Task SendRollMessage(MessageRequest request, NetworkMessageSender networkMessageSender)
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

        content = new MessageContent(PlayersData.Instance.PlayerNames[request.Sender],
                                     request.MessageType,
                                     rollData: new RollMessageData(request.RollMessageRequestData, final));

        networkMessageSender.SendChatMessageToClients(content);
    }
}
