using Unity.Netcode;
using UnityEngine;

public class CameraLocomotion : NetworkBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _scrollStrength;
    private GameInput _gameInput;

    private Vector3 _focusPosition = Vector3.zero;
    private float _distance = 1f;
    private readonly float NEAR_LIMIT = 1;
    private readonly float FAR_LIMIT = 50f;
    private float _currentAlpha = 0f;
    private float _currentBeta = 0f;

    private bool _isTraversing = false;
    private bool _isMovingCenter = false;

    public override void OnNetworkSpawn()
    {  
        if (!IsOwner) 
        { 
            _camera.gameObject.SetActive(false);
            return; 
        }

        _gameInput = GameInput.Instance;
        _gameInput.OnMiddleTriggerStarted += GameInput_OnMiddleTriggerStarted;
        _gameInput.OnMiddleTriggerCanceled += GameInput_OnMiddleTriggerCanceled;

        _gameInput.OnAdditionalOptionStarted += GameInput_OnAdditionalOptionStarted;
        _gameInput.OnAdditionalOptionCanceled += GameInput_OnAdditionalOptionCanceled;

        _focusPosition = PlayersData.Instance.SpawnPoints[(int)OwnerClientId].position;

        Move();
    }

    private void GameInput_OnAdditionalOptionCanceled()
    {
        if (!IsOwner) return;

        _isMovingCenter = false;
    }

    private void GameInput_OnAdditionalOptionStarted()
    {
        if (!IsOwner) return;

        _isMovingCenter = true;
    }

    private void GameInput_OnMiddleTriggerCanceled()
    {
        if (!IsOwner) return;

        _isTraversing = false;
    }

    private void GameInput_OnMiddleTriggerStarted()
    {
        if (!IsOwner) return;

        _isTraversing = true;
    }

    private void Update()
    {
        if (!IsOwner) return;

        Traverse();
    }

    private void UpdateAngles()
    {
        if (!IsOwner) return;
        Vector2 delta = _gameInput.GetPointerDelta();

        _currentAlpha += delta.x * Time.deltaTime;
        _currentBeta += delta.y * Time.deltaTime;
    }

    private void Traverse()
    {
        if (_isMovingCenter && _isTraversing)
        {
            Move();
        }
        else if (_isTraversing)
        {
            UpdateAngles();
        }
        else
        {
            Scroll();
        }
        float x = _distance * Mathf.Cos(_currentAlpha) * Mathf.Sin(_currentBeta);
        float z = _distance * Mathf.Sin(_currentAlpha) * Mathf.Sin(_currentBeta);
        float y = _distance * Mathf.Cos(_currentBeta);


        transform.position = new Vector3(x, y, z) + _focusPosition;
        transform.LookAt(_focusPosition);
    }

    private void Move()
    {
        Vector3 delta = _gameInput.GetPointerDelta() * -1f;

        _focusPosition = _focusPosition + (delta.y * transform.up + delta.x * transform.right) * Time.deltaTime;
    }

    private void Scroll()
    {
        float scrollAmount = -_gameInput.GetScrollDelta() * Time.deltaTime * _scrollStrength;

        _distance = Mathf.Clamp(_distance + scrollAmount, NEAR_LIMIT, FAR_LIMIT);
    }

    private void OnDisable()
    {
        if (!IsOwner) return;
        _gameInput.OnClickCanceled -= GameInput_OnMiddleTriggerCanceled;
        _gameInput.OnClickStarted -= GameInput_OnMiddleTriggerStarted;
    }


}
