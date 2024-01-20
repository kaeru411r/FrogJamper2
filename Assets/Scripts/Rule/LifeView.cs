using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class LifeView : MonoBehaviour
{
    [Tooltip("life�\���p�̊�")]
    [SerializeField] Image[] _lifes;
    [Tooltip("�e���C�t�̃}�e���A��")]
    [SerializeField] Material[] _lifeMaterials;


    private void Start()
    {
        IngameManager.Instance.ViewModel.OnLife
            .Subscribe(LifeUpdate)
            .AddTo(this);
    }

    /// <summary>life�̕\�����X�V</summary>
    public void LifeUpdate(int life)
    {

        //  index��������ɂ��͂ݏo���Ȃ��悤�ɑz����傫�Ȑ��l�̂Ƃ��͗]�蕪��؂�
        if (life > (_lifeMaterials.Length - 1) * _lifes.Length)
        {
            life = (_lifeMaterials.Length - 1) * _lifes.Length;
        }
        //  life��0�����Ȃ�0��
        else if (life < 0)
        {
            life = 0;
        }

        int index = life / _lifes.Length;


        //  life�̉E��(��������)��ύX
        for (int i = _lifes.Length - 1; i >= life % _lifes.Length; i--)
        {
            _lifes[i].material = _lifeMaterials[index];
            Debug.Log($"{i}, {index}");
        }

        index++;

        //  life�̍���(�傫����)��ύX
        for (int i = 0; i < life % _lifes.Length; i++)
        {
            if (index >= 4) //  �G���[��
            {
                Debug.LogError(life + "" + _lifes.Length);
            }
            Debug.Log($"{i}, {index}");
            _lifes[i].material = _lifeMaterials[index];
        }
    }
}