using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class LifeView : MonoBehaviour
{
    [Tooltip("life表現用の丸")]
    [SerializeField] Image[] _lifes;
    [Tooltip("各ライフのマテリアル")]
    [SerializeField] Material[] _lifeMaterials;


    private void Start()
    {
        IngameManager.Instance.ViewModel.OnLife
            .Subscribe(LifeUpdate)
            .AddTo(this);
    }

    /// <summary>lifeの表示を更新</summary>
    public void LifeUpdate(int life)
    {

        //  indexが万が一にもはみ出さないように想定より大きな数値のときは余剰分を切る
        if (life > (_lifeMaterials.Length - 1) * _lifes.Length)
        {
            life = (_lifeMaterials.Length - 1) * _lifes.Length;
        }
        //  lifeが0未満なら0に
        else if (life < 0)
        {
            life = 0;
        }

        int index = life / _lifes.Length;


        //  lifeの右側(小さい方)を変更
        for (int i = _lifes.Length - 1; i >= life % _lifes.Length; i--)
        {
            _lifes[i].material = _lifeMaterials[index];
            Debug.Log($"{i}, {index}");
        }

        index++;

        //  lifeの左側(大きい方)を変更
        for (int i = 0; i < life % _lifes.Length; i++)
        {
            if (index >= 4) //  エラー報告
            {
                Debug.LogError(life + "" + _lifes.Length);
            }
            Debug.Log($"{i}, {index}");
            _lifes[i].material = _lifeMaterials[index];
        }
    }
}