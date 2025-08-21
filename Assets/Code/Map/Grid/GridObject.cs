using DG.Tweening;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private Transform _visuals;
    private const float MOVE_DURATION = 2f;
    public int ID { get { return _id; } }
    private Vector3Int _gridPosition;
    public Vector3Int GridPosition { get { return _gridPosition; } }
    public void Init(Vector3Int position)
    {
        _gridPosition = position;
    }
    public void Select()
    {
        _visuals.position += Vector3.up;
    }
    public void Deselect()
    {
        _visuals.position -= Vector3.up;
    }
    public void Move(Vector3Int newPosition)
    {
        transform.DOMove(newPosition, MOVE_DURATION);
        _gridPosition = newPosition;
    }
}
