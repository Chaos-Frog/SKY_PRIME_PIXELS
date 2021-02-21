using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Player 弾クラス */
public class PlayerBullets : Bullets {
    [SerializeField] protected float speed; // 弾移動スピード
    [SerializeField] protected int damage;  // 弾が与えるダメージ量

    // スピード変更
    public void SetSpeed(float spd) {
        speed = spd;
    }

    // ダメージ値渡し
    public int GetDamage() {
        return damage;
    }
}
