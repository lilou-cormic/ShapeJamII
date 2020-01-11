using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float Speed { get; set; }

    private float _timeLeft;

    public event Action<Vector3> OnPlayerTryMove;

    public int Col => Mathf.RoundToInt(transform.position.x);
    public int Row => Mathf.RoundToInt(transform.position.y);

    private void Start()
    {
        Speed = 0.1f;
        _timeLeft = 0f;
    }

    private void Update()
    {
        if (_timeLeft <= 0)
        {
            _timeLeft = Speed;

            if (Input.GetAxisRaw("Vertical") > 0)
                TryMove(Vector3.up);

            else if (Input.GetAxisRaw("Horizontal") > 0)
                TryMove(Vector3.right);

            else if (Input.GetAxisRaw("Horizontal") < 0)
                TryMove(Vector3.left);

            else if (Input.GetAxisRaw("Vertical") < 0)
                TryMove(Vector3.down);
        }
        else
        {
            _timeLeft -= Time.deltaTime;
        }
    }

    private void TryMove(Vector3 direction)
    {
        var newPosition = transform.position + direction * 3;

        if (newPosition.y > 9 || newPosition.y < -3)
            return;

        OnPlayerTryMove?.Invoke(newPosition);
    }
}
