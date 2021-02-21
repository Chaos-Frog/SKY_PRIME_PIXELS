using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : Bullets {
    private float bulletSpeed; // 弾速度

    void Start() {
        withdrawal = true;
    }

    // 弾移動関数
    protected override void MoveBullet() {
        this.transform.position += this.transform.rotation * new Vector3(0, bulletSpeed, 0) * Time.deltaTime;
    }

    // 弾設定関数
    public void SetUp(float speed, float angle, float size) {
        bulletSpeed = speed;
        this.transform.rotation = Quaternion.Euler(0, 0, angle);
        this.transform.localScale = new Vector3(size, size, 0.0f);
    }

    // 弾消去関数
    public void DestroyBullet() {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        StartCoroutine(nameof(DestroyBulletCor));
    }

    // 弾消滅コルーチン
    private IEnumerator DestroyBulletCor() {
        float time = 0.0f;

        while(time <= 0.2f) {
            this.transform.localScale *= 1.0f - (time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
