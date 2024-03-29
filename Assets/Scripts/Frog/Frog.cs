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
[RequireComponent(/*typeof(Joint2D), */typeof(Rigidbody2D))]
public class Frog : SingletonMono<Frog>
{

    [Tooltip("スピード")]
    [SerializeField] float _speed;
    [Tooltip("ジャンプ距離のチャージ速度")]
    [SerializeField] float _chargeSpeed;
    [Tooltip("高さ制限")]
    [SerializeField] float _highest;

    Vector2 _targetPosition;
    ReactiveProperty<float> _distance = new ReactiveProperty<float>();
    bool _isTouching = false;
    ReactiveProperty<FrogState> _frogState = new ReactiveProperty<FrogState>(FrogState.Stand);
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
        InputAgent2.Subscribe("Player", "MousePosition", OnMousePosition).AddTo(this);
        InputAgent2.Subscribe("Player", "Touch", OnTouch).AddTo(this);
        if (TryGetComponent(out _collider))
        {
            _type = _collider.GetType();
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

        _frogState.Value = FrogState.Jump;
        while (_distance.Value > 0)
        {
            yield return null;
            Debug.DrawRay(transform.position, direction * 100);
            float dis = _speed * Time.deltaTime;
            dis = Mathf.Min(dis, _distance.Value);
            transform.Translate(direction * dis, Space.World);

            float pos = transform.position.y - Field.Instance.Center.y;

            if (pos > _highest)
            {
                Field.Instance.Position += pos - _highest;
            }
            _distance.Value -= dis;
        }

        Land();
    }

    private void Land()
    {
        if (TryGetRideables(out IRideable[] rideables))
        {
            var rideObject = rideables
                .OrderByDescending(rideable => Vector2.Distance(transform.position, rideable.Position))
                .FirstOrDefault();

            _frogState.Value = FrogState.Stand;

            var onPosition = rideObject.Ride()
                .Subscribe(Landing)
                .AddTo(this);

            var onLotusDestroy = rideObject.OnDestroyed
                .Subscribe(_ => Drowing())
                .AddTo(this);

            _frogState
                .Where(state => state == FrogState.Jump)
                .Subscribe(_ =>
                {
                    onLotusDestroy.Dispose();
                    onPosition.Dispose();
                })
                .AddTo(this);
        }
        else
        {
            StartCoroutine(Drowing());
        }


    }

    private void Landing(Vector2 position)
    {
        transform.position = position;
    }


    private IEnumerator Drowing()
    {
        _frogState.Value = FrogState.Drown;
        IngameManager.Instance.Drow();
        yield return _frogState;
    }

    IRideable[] GetRideables()
    {
        RaycastHit2D[] hits = null;
        var pos = (Vector2)_collider.transform.position + _collider.offset;

        if (_type == typeof(BoxCollider2D))
        {
            var box = (BoxCollider2D)_collider;
            hits = Physics2D.BoxCastAll(pos, box.size, box.transform.eulerAngles.z, Vector2.zero, 1.0f);
        }
        else if (_type == typeof(CircleCollider2D))
        {
            var circle = (CircleCollider2D)_collider;
            hits = Physics2D.CircleCastAll(pos, circle.radius, Vector2.zero);
        }
        else if (_type == typeof(CapsuleCollider2D))
        {
            var capsule = (CapsuleCollider2D)_collider;
            hits = Physics2D.CapsuleCastAll(pos, capsule.size, capsule.direction, capsule.transform.eulerAngles.z, Vector2.zero);
        }

        var result = hits?.Select(value => value.collider.gameObject.GetComponent<IRideable>())
                            .Where(value => value != null)
                            .ToArray();

        return result.Length > 0 ? result : null;
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
