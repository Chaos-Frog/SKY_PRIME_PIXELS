using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_M : Enemy {
    private bool endDeath = false;

    protected override bool DeathFunc() {
        if(time <= 0.0f) {
            this.gameObject.GetComponent<Collider2D>().enabled = false;

            // スコア加算
            GameController.Instance.AddScore(scorePoint);

            // スコアテキスト表示
            Text text = Instantiate(scoreTextObj).GetComponent<Text>();
            ScoreText textScr = text.GetComponent<ScoreText>();
            textScr.SetScore(scorePoint);
            text.transform.SetParent(GameObject.Find("Canvas").transform, false);
            text.transform.localScale *= 1.5f;
            text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);

            // 死亡演出コルーチン開始
            StartCoroutine(nameof(DeathDirecting));
            time = 1.0f;
        }

        if(endDeath) {
            return true;
        } else {
            // 敵弾消去
            GameController.Instance.DeleteAllEBullets();
            return false;
        }
    }

    // Enemy_M 死亡演出コルーチン
    private IEnumerator DeathDirecting() {
        // 敵弾消去
        GameController.Instance.DeleteAllEBullets();

        for(int i = 0; i < 3; i++) {
            float angle = 180.0f + (120 * i) + Random.Range(-5.0f, 5.0f);
            Vector3 pos = Quaternion.Euler(0, 0, angle) * Vector3.up * Random.Range(0.6f, 1.0f);
            GameObject exp_s = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position + pos);
            exp_s.transform.localScale *= 0.6f;
            RandomExpSE_Small();
            yield return new WaitForSeconds(0.2f);
        }

        GameObject exp_l = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position);
        exp_l.transform.localScale *= 1.5f;
        RandomExpSE_Big();
        endDeath = true;
    }
}