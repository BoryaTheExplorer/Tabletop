using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GridObject : NetworkBehaviour
{
    [SerializeField] private int _id;
    private const float MOVE_DURATION = 2f;
    public int ID { get { return _id; } }
    private Vector3Int _gridPosition;
    public Vector3Int GridPosition { get { return _gridPosition; } }

    public void Select()
    {
        transform.position += Vector3.up;
    }
    public void Deselect()
    {
        transform.position -= Vector3.up;
    }
    public void Move(Vector3Int newPosition)
    {

        transform.DOMove(newPosition, MOVE_DURATION);
        _gridPosition = newPosition;
    }
}
