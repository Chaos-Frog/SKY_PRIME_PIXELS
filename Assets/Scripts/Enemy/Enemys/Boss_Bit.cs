using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_Bit : Enemy {
    public bool death;

    void Start() {
        death = false;

        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        enemyColor = sr.color;
    }

    void FixedUpdate() {
        if(hp > 0) {
            DamageEffect();
            time += Time.deltaTime;
        } else {
            if(DeathFunc()) this.gameObject.SetActive(false);
        }
    }

    // 被弾演出
    protected override void DamageEffect() {
        if(damageCT > 0.0f) {
            SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
            sr.color = enemyColor * 0.8f;

            damageCT -= Time.deltaTime;

            if(damageCT <= 0.0f) {
                sr.color = enemyColor;
            }
        }
    }

    // 死亡時処理 消滅時に true を返す
    protected override bool DeathFunc() {
        if(time <= 0.0f) {
            // スコア加算
            GameController.Instance.AddScore(scorePoint);

            // スコアテキスト表示
            Text text = Instantiate(scoreTextObj).GetComponent<Text>();
            ScoreText textScr = text.GetComponent<ScoreText>();
            textScr.SetScore(scorePoint);
            text.transform.SetParent(GameObject.Find("Canvas").transform, false);
            text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);

            // 爆破エフェクト設置
            GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, this.transform.position);
            eff.transform.localScale = new Vector2(0.8f, 0.8f);

            // 効果音再生
            RandomExpSE_Big();
        }

        death = true;
        return true;
    }

    // 自爆（スコアなし）
    public void SelfDestroy() {
        // 爆破エフェクト設置
        GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, this.transform.position);
        eff.transform.localScale = new Vector2(0.8f, 0.8f);

        // 効果音再生
        RandomExpSE_Big();

        this.gameObject.SetActive(false);
    }
}
