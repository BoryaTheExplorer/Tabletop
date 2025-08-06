using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
using System;
public class Dice : NetworkBehaviour
{
    [SerializeField] private List<Transform> _sides = new List<Transform>();
    [SerializeField] private Rigidbody _rigidBody;

    public event Action<int> OnDiceRolled;

    private bool _rolling = false;
    private float _haltVelocity = .1f;

    private float _countdown = 1.5f;
    private float _timer = 0f;
    private void FixedUpdate()
    {
        if (!IsServer)
            return;

        if (!_rolling)
            return;

        if (_rigidBody.linearVelocity.magnitude < _haltVelocity && _rigidBody.angularVelocity.magnitude < _haltVelocity)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                int result = GetRollOutcome();
                _rolling = false;

                Debug.Log(result);
                OnDiceRolled?.Invoke(result);
            }
        }
    }

    private int GetRollOutcome()
    {
        float dot = -1f;
        float dotMax = -1f;

        int face = 0;

        for (int i = 0; i < _sides.Count; i++)
        {
            dot = Vector3.Dot(Vector3.up, _sides[i].up);

            if (dot > dotMax)
            {
                dotMax = dot;
                face = i;
            }
        }

        return face + 1;
    }

    public void Roll(Vector3 force, Vector3 torque)
    {
        if (!IsServer)
            return;

        _rigidBody.linearVelocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;

        _rigidBody.AddForce(force, ForceMode.Impulse);
        _rigidBody.AddForce(torque, ForceMode.Impulse);

        _rolling = true;
        _timer = _countdown;
    }
}
