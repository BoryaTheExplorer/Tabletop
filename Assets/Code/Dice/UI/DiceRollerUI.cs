using UnityEngine;
using UnityEngine.UI;

public class DiceRollerUI : MonoBehaviour
{
    [SerializeField] private DiceRoller _roller;
    [SerializeField] private Button _rollButton;

    private void Awake()
    {
        _rollButton.onClick.AddListener(RollDice);
    }

    private void RollDice()
    {
        _roller.SpawnDice(DiceType.D6, new Vector3(32f, 40f, 32f), Quaternion.identity);
    }
}
