using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusManager : MonoBehaviour
{
    [Tooltip("生成する蓮")]
    [SerializeField] Lotus _lotus;
    [Tooltip("蓮の最低枚数")]
    [SerializeField] int _minValue = 2;
    [Tooltip("基礎生成確立")]
    [SerializeField] float _probability = 0.1f;
    [Tooltip("時間による生成確立の上昇係数")]
    [SerializeField] float _timeFactor = 1.0f;
    [Tooltip("枚数による生成確立の上昇係数")]
    [SerializeField] float _numFactor = 1.0f;
    [Tooltip("蓮の生存可能範囲(左上)")]
    [SerializeField] Vector2 _leftTop;
    [Tooltip("蓮の生存可能範囲(右下)")]
    [SerializeField] Vector2 _rightBottom;
    [Tooltip("蓮の最低スピード")]
    [SerializeField] float _lowerSpeed = 1.0f;
    [Tooltip("蓮の最高スピード")]
    [SerializeField] float _upperSpeed = 2.0f;

    List<Lotus> _lotusList = new List<Lotus>();
    float _time = 0.0f;

    /// <summary>現在存在する蓮の枚数</summary>
    public List<Lotus> LotusList { get => _lotusList; }
    /// <summary>生成する蓮</summary>
    public Lotus Lotus { get => _lotus; set => _lotus = value; }
    /// <summary>蓮の最低枚数</summary>
    public int MinValue { get => _minValue; set => _minValue = value; }
    /// <summary>基礎生成確立</summary>
    public float Probability { get => _probability; set => _probability = value; }
    /// <summary>時間による生成確立の上昇係数</summary>
    public float TimeFactor { get => _timeFactor; set => _timeFactor = value; }
    /// <summary>枚数による生成確立の上昇係数</summary>
    public float NumFactor { get => _numFactor; set => _numFactor = value; }
    /// <summary>蓮の生成範囲(右下)</summary>
    public Vector2 RightBottom { get => _leftTop; set => _leftTop = value; }
    /// <summary>蓮の生成範囲(左上)</summary>
    public Vector2 LeftTop { get => _rightBottom; set => _rightBottom = value; }


    public void RandomGenerate(int num)
    {
        for (var i = 0; i < num; i++)
        {
            var lotus = Generate(_leftTop, _rightBottom);
        }
    }

    /// <summary>
    /// 生成ループスタート
    /// </summary>
    public void GenerateStart()
    {
        StartCoroutine(GenerateLoop());
    }


    /// <summary>
    /// 蓮生成
    /// 引数は生成の抽選範囲
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
