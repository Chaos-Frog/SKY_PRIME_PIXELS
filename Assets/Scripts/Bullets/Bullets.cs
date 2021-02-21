using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour {
    protected Vector2 bulletSize; // 弾画像サイズ
    protected bool withdrawal;    // 離脱判定

    void Start() {
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        bulletSize = sr.sprite.rect.size / sr.sprite.pixelsPerUnit;
        bulletSize *= this.transform.localScale;
    }

    void FixedUpdate() {
        MoveBullet();
        DeleteBullet();
    }

    // 弾移動関数 Virtual
    protected virtual void MoveBullet() {
    }

    // 範囲外の弾消去関数
    protected void DeleteBullet() {
        if(withdrawal) {
            Vector3 pos = this.transform.position;
            // 画面範囲外に出たら消去
            if(pos.x < GameController.window_LUPos.x - bulletSize.x / 2 || pos.x > GameController.window_RBPos.x + bulletSize.x / 2 ||
               pos.y > GameController.window_LUPos.y + bulletSize.y / 2 || pos.y < GameController.window_RBPos.y - bulletSize.y / 2) {
                Destroy(this.gameObject);
            }
        }
    }
}
