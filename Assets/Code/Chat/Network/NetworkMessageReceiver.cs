using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkMessageReceiver : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ReceiveChatMessage", HandleChatMessage);
    }

    public void HandleChatMessage(ulong senderId, FastBufferReader reader)
    {
        MessageContent content;
        reader.ReadValueSafe(out content);


        switch (content.MessageType)
        {
            case MessageType.PlainMessage:
                MessageContainer.Instance.SpawnPlainMessage(content.Sender, content.PlainMessage.Message);
                break;
            case MessageType.RollMessage:
                Dictionary<DiceType, int> dice = DiceParser.ParseDiceFromString(content.RollMessage.Dice);
                Dictionary<DiceType, int[]> outcomes = new Dictionary<DiceType, int[]>();
                
                int index = 0;
                
                foreach (var die in dice)
                {
                    int[] nums = new int[die.Value];
                    for (int i = 0; i < nums.Length; i++)
                    {
                        nums[i] = content.RollMessage.Outcomes[i + index];
                    }
                    outcomes.Add(die.Key, (int[])nums.Clone());
                    index += nums.Length;
                }

                int[] mods = new int[content.RollMessage.Modifiers.Length];

                for (int i = 0; i < mods.Length; i++)
                {
                    mods[i] = content.RollMessage.Modifiers[i];
                }

                MessageContainer.Instance.SpawnRollMessage(outcomes, content.Sender, content.RollMessage.RollType, mods);
                break;
        }
        //

    }
}
