using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingBullets : PlayerBullets {
    private float time;        // タイム
    private GameObject target; // ターゲットエネミー

    // 弾移動関数
    protected override void MoveBullet() {
        if(time <= 0.3f) {
            float dist = 0.0f;
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Enemy")) {
                float temp = Vector3.Distance(this.transform.position, obj.transform.position);
                if(dist == 0.0f || dist > temp) {
                    dist = temp;
                    target = obj;
                }
            }
        }
        
        if(time >= 0.3f) {
            if(target != null) {
                Vector3 dif = target.transform.position - this.transform.position;
                float toAngle = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg - 90.0f;
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, toAngle), 6.0f);
            }
        }

        float spd;
        if(time <= 0.3f) {
            spd = speed * Easing.Ease_Out_Sine(0.6f, 0.2f, time / 0.3f);
        } else {
            spd = speed * Easing.Ease_In_Sine(0.2f, 1.0f, (time - 0.3f) / 0.7f);
        }
        this.transform.position += transform.up * spd * Time.deltaTime;

        if(time >= 1.0f) withdrawal = true;
        if(time <= 1.0f) time += Time.deltaTime;
    }
}
