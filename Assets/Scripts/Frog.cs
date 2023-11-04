using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using Unity.VisualScripting.FullSerializer;

public class Frog : FieldFollowUpObject
{
    [Tooltip("スピード")]
    [SerializeField] float _speed;

    Vector2 _targetPosition;
    float _distance;
    List<Lotus> _touchingLotuses = new List<Lotus>();
    bool _isTouching = false;
    FrogState _frogState = FrogState.Stand;

    /// <summary>スピード</summary>
    public float Speed { get => _speed; set => _speed = value; }


    public void OnMousePosition(InputAction.CallbackContext callback)
    {
        if (callback.phase == InputActionPhase.Performed)
        {
            _targetPosition = (Vector2)Camera.main.ScreenToWorldPoint(callback.ReadValue<Vector2>());
        }
    }

    public void OnTouch(InputAction.CallbackContext callback)
    {
        if (_frogState == FrogState.Stand)
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

    IEnumerator Targeting()
    {
        while (_isTouching)
        {
            yield return null;

            _distance += Time.deltaTime;
        }

        StartCoroutine(Jumping(_targetPosition - (Vector2)transform.position));
    }

    IEnumerator Jumping(Vector2 direction)
    {
        _frogState = FrogState.Jump;
        while (_distance > 0)
        {
            yield return new WaitForFixedUpdate();
            float dis = _speed * Time.deltaTime;
            dis = Mathf.Min(dis, _distance);
            _rigidbody.MovePosition((Vector2)transform.position + (direction * dis));

            _distance -= dis;
        }

        Landing();
    }

    private void Landing()
    {
        if (_touchingLotuses.Count > 0)
        {
            _frogState = FrogState.Stand;
            _touchingLotuses = _touchingLotuses.OrderByDescending(
                lotus => Vector2.Distance(transform.position, lotus.transform.position))
                .ToList();

            var lotus = _touchingLotuses.FirstOrDefault();
            transform.parent = lotus.transform;
            transform.localPosition = Vector3.zero;
            _rigidbody.simulated = false;
        }
        else
        {
            Drowing();
        }


    }

    private void Drowing()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Lotus>(out  var lotus))
        {
            Debug.Log($"Enter {lotus}");
            _touchingLotuses.Add(lotus);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Lotus>(out var lotus))
        {
            Debug.Log($"Exit {lotus}");
            _touchingLotuses.Remove(lotus);
        }
    }
}
