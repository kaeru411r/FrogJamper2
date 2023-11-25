using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UniRx;
using System;
using InputSystemAgent;
using Unity.VisualScripting;
using UnityEngine.UIElements;

/// <summary>
/// カエル
/// </summary>
[RequireComponent(typeof(Joint2D), typeof(Rigidbody2D))]
public class Frog : SingletonMono<Frog>
{

    [Tooltip("スピード")]
    [SerializeField] float _speed;
    [SerializeField] float _chargeSpeed;
    [SerializeField] PlayerInput _playerInput;

    Vector2 _targetPosition;
    ReactiveProperty<float> _distance = new ReactiveProperty<float>();
    List<Lotus> _touchingLotuses = new List<Lotus>();
    bool _isTouching = false;
    ReactiveProperty<FrogState> _frogState = new ReactiveProperty<FrogState>(FrogState.Stand);
    Joint2D _joint;
    Rigidbody2D _rigidBody;

    /// <summary>スピード</summary>
    public float Speed { get => _speed; set => _speed = value; }
    public float ChargeSpeed { get => _chargeSpeed; set => _chargeSpeed = value; }
    public IObservable<FrogState> StateSubject => _frogState;
    public IObservable<float> JumpDistance => _distance;


    void OnMousePosition(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Performed)
        {
            _targetPosition = (Vector2)Camera.main.ScreenToWorldPoint(callback.ReadValue<Vector2>());
        }
    }

    void OnTouch(InputAction.CallbackContext callback)
    {
        if (_frogState.Value == FrogState.Stand)
        {
            if (callback.phase == InputActionPhase.Started)
            {
                _isTouching = true;
                StartCoroutine(Targeting());
            }
            else if (callback.phase == InputActionPhase.Canceled)
            {
                _isTouching = false;
            }
        }
    }

    // Start is called before the first frame update
    public void PlayStart()
    {
        StartCoroutine(Targeting());
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        InputAgent2.Subscribe("Player", "MousePosition", OnMousePosition);
        InputAgent2.Subscribe("Player", "Touch", OnTouch);

        _joint = GetComponent<Joint2D>();
    }

    IEnumerator Targeting()
    {
        while (true)
        {
            yield return null;

            var direction = (_targetPosition - (Vector2)transform.position).normalized;
            transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2(direction.x, direction.y) / Mathf.PI * 180);
            _distance.Value += Time.deltaTime * _chargeSpeed;

            if (!_isTouching)
            {
                StartCoroutine(Jumping(direction));
                break;
            }
        }

    }

    IEnumerator Jumping(Vector2 direction)
    {
        _rigidBody.velocity = Vector2.zero;
        _joint.enabled = false;
        _frogState.Value = FrogState.Jump;
        while (_distance.Value > 0)
        {
            yield return new WaitForFixedUpdate();
            float dis = _speed * Time.fixedDeltaTime;
            dis = Mathf.Min(dis, _distance.Value);
            _rigidBody.MovePosition(((Vector2)transform.position + (direction * dis)));

            _distance.Value -= dis;
        }

        Landing();
    }

    private void Landing()
    {
        if (_touchingLotuses.Count > 0)
        {
            _frogState.Value = FrogState.Stand;
            _touchingLotuses = _touchingLotuses.OrderByDescending(
                lotus => Vector2.Distance(transform.position, lotus.transform.position))
                .ToList();

            var lotus = _touchingLotuses.FirstOrDefault();

            transform.localPosition = Vector3.zero;
            transform.position = lotus.transform.position;
            _joint.enabled = true;
            _joint.connectedBody = lotus.Rigidbody;
        }
        else
        {
            Drowing();
        }


    }

    private void Drowing()
    {
        _frogState.Value = FrogState.Drown;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Lotus>(out var lotus))
        {
            _touchingLotuses.Add(lotus);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Lotus>(out var lotus))
        {
            _touchingLotuses.Remove(lotus);
        }
    }
}
