using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalBullets : PlayerBullets {

    private void Start() {
        withdrawal = true;
    }

    // 弾移動関数
    protected override void MoveBullet() {
        this.transform.position += transform.up * speed * Time.deltaTime;
    }
}
