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
    [Tooltip("高さ制限")]
    [SerializeField] float _highest;

    Vector2 _targetPosition;
    ReactiveProperty<float> _distance = new ReactiveProperty<float>();
    bool _isTouching = false;
    ReactiveProperty<FrogState> _frogState = new ReactiveProperty<FrogState>(FrogState.Stand);
    Joint2D _joint;
    Rigidbody2D _rigidBody;
    Type _type;
    Collider2D _collider;

    /// <summary>スピード</summary>
    public float Speed { get => _speed; set => _speed = value; }
    public float ChargeSpeed { get => _chargeSpeed; set => _chargeSpeed = value; }
    public IObservable<FrogState> OnState => _frogState;
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
        if (TryGetComponent(out _collider))
        {
            _type = _collider.GetType();
            Debug.Log(_type);
        }
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
            yield return null;
            Debug.DrawRay(transform.position, direction * 100);
            float dis = _speed * Time.deltaTime;
            dis = Mathf.Min(dis, _distance.Value);
            transform.Translate(direction * dis, Space.World);

            if (transform.position.y > _highest)
            {
                Field.Instance.Position += transform.position.y - _highest;
            }
            _distance.Value -= dis;
        }

        Landing();
    }

    private void Landing()
    {
        if (TryGetRideables(out IRideable[] rideables))
        {
            var rideable = rideables
                .OrderByDescending(rideable => Vector2.Distance(transform.position, rideable.Transform.position))
                .FirstOrDefault();
            _frogState.Value = FrogState.Stand;


            transform.localPosition = Vector3.zero;
            transform.position = rideable.Transform.position;
            _joint.enabled = true;
            _joint.connectedBody = rideable.Rigidbody;

            rideable.Ride();

            var onLotusDestroy = rideable.OnDestroyed
                .Subscribe(_ => Drowing())
                .AddTo(this);

            _frogState
                .Where(state => state == FrogState.Jump)
                .Subscribe(_ => onLotusDestroy.Dispose());
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

    IRideable[] GetRideables()
    {
        if (_type == typeof(BoxCollider2D))
        {
            var box = (BoxCollider2D)_collider;
            var pos = (Vector2)box.transform.position + box.offset;
            var hits = Physics2D.BoxCastAll(pos, box.size, box.transform.eulerAngles.z, Vector2.zero, 1.0f);
            var result = hits.Select(value => value.collider.gameObject.GetComponent<IRideable>())
                            .Where(value => value != null)
                            .ToArray();

            return result.Length > 0 ? result : null;
        }
        return null;
    }

    bool TryGetRideables(out IRideable[] rideables)
    {
        rideables = GetRideables();
        return rideables != null;
    }
    private void OnDrawGizmosSelected()
    {
        var pos1 = new Vector2(Field.Instance.Vertex1.x, _highest);
        var pos2 = new Vector2(Field.Instance.Vertex3.x, _highest);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos1, pos2);
    }
}
