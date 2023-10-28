using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusManager : MonoBehaviour
{
    [Tooltip("��������@")]
    [SerializeField] Lotus _lotus;
    [Tooltip("�@�̍Œᖇ��")]
    [SerializeField] int _minValue = 2;
    [Tooltip("��b�����m��")]
    [SerializeField] float _probability = 0.1f;
    [Tooltip("���Ԃɂ�鐶���m���̏㏸�W��")]
    [SerializeField] float _timeFactor = 1.0f;
    [Tooltip("�����ɂ�鐶���m���̏㏸�W��")]
    [SerializeField] float _numFactor = 1.0f;
    [Tooltip("�@�̐����\�͈�(����)")]
    [SerializeField] Vector2 _leftTop;
    [Tooltip("�@�̐����\�͈�(�E��)")]
    [SerializeField] Vector2 _rightBottom;
    [Tooltip("�@�̍Œ�X�s�[�h")]
    [SerializeField] float _lowerSpeed = 1.0f;
    [Tooltip("�@�̍ō��X�s�[�h")]
    [SerializeField] float _upperSpeed = 2.0f;

    List<Lotus> _lotusList = new List<Lotus>();
    float _time = 0.0f;

    /// <summary>���ݑ��݂���@�̖���</summary>
    public List<Lotus> LotusList { get => _lotusList; }
    /// <summary>��������@</summary>
    public Lotus Lotus { get => _lotus; set => _lotus = value; }
    /// <summary>�@�̍Œᖇ��</summary>
    public int MinValue { get => _minValue; set => _minValue = value; }
    /// <summary>��b�����m��</summary>
    public float Probability { get => _probability; set => _probability = value; }
    /// <summary>���Ԃɂ�鐶���m���̏㏸�W��</summary>
    public float TimeFactor { get => _timeFactor; set => _timeFactor = value; }
    /// <summary>�����ɂ�鐶���m���̏㏸�W��</summary>
    public float NumFactor { get => _numFactor; set => _numFactor = value; }
    /// <summary>�@�̐����͈�(�E��)</summary>
    public Vector2 RightBottom { get => _leftTop; set => _leftTop = value; }
    /// <summary>�@�̐����͈�(����)</summary>
    public Vector2 LeftTop { get => _rightBottom; set => _rightBottom = value; }


    public void RandomGenerate(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var lotus = Generate(_leftTop, _rightBottom);
        }
    }

    /// <summary>
    /// �������[�v�X�^�[�g
    /// </summary>
    public void GenerateStart()
    {
        StartCoroutine(GenerateLoop());
    }


    /// <summary>
    /// �@����
    /// �����͐����̒��I�͈�
    /// </summary>
    /// <param name="leftTop"></param>
    /// <param name="rightBottom"></param>
    /// <returns></returns>
    public Lotus Generate(Vector2 leftTop, Vector2 rightBottom)
    {
        Vector2 point;
        if (leftTop != rightBottom)
        {
            point = new Vector2(Random.Range(leftTop.x, rightBottom.x), Random.Range(leftTop.y, rightBottom.y));
        }
        else
        {
            point = leftTop;
        }

        var lotus = Instantiate(_lotus, point, Quaternion.identity);
        _lotusList.Add(lotus);

        var speed = Random.Range(_lowerSpeed, _upperSpeed);
        lotus.Speed = Vector2.down * speed;
        lotus.RunStart();
        Destroy(lotus.gameObject, Mathf.Abs(_leftTop.y - _rightBottom.y) / speed);

        lotus.OnDestroyAction += (_) =>
        {
            _lotusList.Remove(lotus);
        };

        return lotus;
    }


    IEnumerator GenerateLoop()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (Lottery())
            {
                _time = 0.0f;
                var lotus = Generate(new Vector2(_leftTop.x, _leftTop.y), new Vector2(_rightBottom.x, _leftTop.y));
            }
            else
            {
                _time += Time.fixedDeltaTime;
            }
        }
    }

    bool Lottery()
    {
        float success = _probability * _time * _timeFactor;
        float failure = (1 - _probability) * Mathf.Max((_lotusList.Count - (_minValue - 1)) * _numFactor, 0);
        float hit = Random.Range(0.0f, success + failure);

        return hit <= success;
    }




    private void OnValidate()
    {
        if (_minValue < 2)
        {
            _minValue = 2;
        }
    }
}
