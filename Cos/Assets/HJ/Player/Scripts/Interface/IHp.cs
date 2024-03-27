using System;
using UnityEngine;

namespace HJ
{
    internal interface IHp
    {
        float hp { get; set; }
        float hpMax { get; }
        event Action<float> onHpChanged; // 변화된 후의 값
        event Action<float> onHpDepleted; // 고갈된 양
        event Action<float> onHpRecovered; // 회복된 양
        event Action onHpMin;
        event Action onHpMax;
        void DepleteHp(float amount);
        void RecoverHp(float amount);

        void Hit(float damage, bool powerAttack, Quaternion hitRotation);
        void Hit(float damage);
    }
}