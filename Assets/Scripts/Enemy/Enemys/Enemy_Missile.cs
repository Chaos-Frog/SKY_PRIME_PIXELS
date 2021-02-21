using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Missile : Enemy {
    public override void SetEnemy(int mp, int dp) {
        movePaternClass = Base_MovePatern.SetMovePaternClass(moveParameters[mp]);
        movePaternClass.Init(this.gameObject);

        danmakuPaternClass = null;
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
            GameController.Instance.AddScore(scorePoint, false);

            // 爆破エフェクト設置
            GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, this.transform.position);
            eff.transform.localScale = new Vector2(0.4f, 0.4f);

            // 効果音再生
            RandomExpSE_Small();
        }
        return true;
    }

    // 自爆（スコアなし）
    public void SelfDestroy() {
        // 爆破エフェクト設置
        GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, this.transform.position);
        eff.transform.localScale = new Vector2(0.8f, 0.8f);

        Destroy(this.gameObject);
    }
}
