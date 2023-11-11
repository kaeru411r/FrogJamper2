using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UniRx;
using System;
using Unity.VisualScripting;
using UnityEngine.UIElements;

/// <summary>
/// カエル
/// </summary>
[RequireComponent(typeof(Joint2D))]
public class Frog : FieldFollowUpObject
{
    [Tooltip("スピード")]
    [SerializeField] float _speed;
    [SerializeField] PlayerInput _playerInput;

    Vector2 _targetPosition;
    float _distance;
    List<Lotus> _touchingLotuses = new List<Lotus>();
    bool _isTouching = false;
    ReactiveProperty<FrogState> _frogState = new ReactiveProperty<FrogState>(FrogState.Stand);
    Joint2D _joint;

    /// <summary>スピード</summary>
    public float Speed { get => _speed; set => _speed = value; }
    public IObservable<FrogState> StateSubject => _frogState;

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

    private new void Start()
    {
        base.Start();
        _joint = GetComponent<Joint2D>();
        _playerInput.AddListener("Player", "Touch", OnTouch);
        _playerInput.AddListener("Player", "MousePosition", OnMousePosition);
    }

    IEnumerator Targeting()
    {
        while (true)
        {
            yield return null;

            var direction = _targetPosition - (Vector2)transform.position;
            transform.eulerAngles = new Vector3(0, 0, -Mathf.Atan2(direction.x, direction.y) / Mathf.PI * 180);
            _distance += Time.deltaTime;

            if (!_isTouching)
            {
                StartCoroutine(Jumping(direction));
                break;
            }
        }

    }

    IEnumerator Jumping(Vector2 direction)
    {
        //transform.parent = null;
        Rigidbody.velocity = Vector2.zero;
        Rigidbody.simulated = true;
        _joint.enabled = false;
        _frogState.Value = FrogState.Jump;
        while (_distance > 0)
        {
            yield return new WaitForFixedUpdate();
            float dis = _speed * Time.fixedDeltaTime;
            dis = Mathf.Min(dis, _distance);
            Rigidbody.MovePosition(((Vector2)transform.position + (direction * dis)));

            _distance -= dis;
        }

        Landing();
        Debug.Log(9);
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
            //Rigidbody.simulated = false;
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
