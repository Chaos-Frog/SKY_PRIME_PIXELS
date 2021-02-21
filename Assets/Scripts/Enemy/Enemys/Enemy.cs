using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [SerializeField] protected int hp;                                     // 体力
    [SerializeField] protected uint scorePoint;                            // スコア
    [SerializeField] protected GameObject scoreTextObj;                    // スコアテキストプレハブ
    [SerializeField] protected Base_MoveParameters[] moveParameters;       // 移動パラメータ配列
    [SerializeField] protected BaseDanmakuParameter[] danmakuParameters;  // 弾幕パラメータ配列

    protected Base_MovePatern    movePaternClass;     // 移動パターンクラス
    protected Base_DanmakuPatern danmakuPaternClass;  // 弾幕パターンクラス

    protected Vector2 enemySize;                      // 敵画像サイズ
    protected Color enemyColor;                       // 敵カラー
    protected float time;                             // 生存時間
    protected float damageCT;                         // 被弾演出クールタイム

    void Start() {
        // 画像サイズ取得
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        enemySize = sr.sprite.rect.size / sr.sprite.pixelsPerUnit;
        enemySize *= this.transform.localScale;
        enemyColor = sr.color;
    }

    void FixedUpdate() {
        if(hp > 0) {
            if(movePaternClass    != null) movePaternClass.Moving();
            if(danmakuPaternClass != null) danmakuPaternClass.Shot();
            DamageEffect();
            CheckOutArea();
            time += Time.deltaTime;
        } else {
            if(DeathFunc()) Destroy(this.gameObject);
        }
    }

    // 敵設定
    virtual public void SetEnemy(int mp, int dp) {
        movePaternClass = Base_MovePatern.SetMovePaternClass(moveParameters[mp]);
        movePaternClass.Init(this.gameObject);

        danmakuPaternClass = Base_DanmakuPatern.SetDanmakuPaternClass(danmakuParameters[dp]);
        danmakuPaternClass.Init(this.gameObject);
    }

    // 被弾演出
    virtual protected void DamageEffect() {
    }

    // 死亡時処理 消滅時に true を返す
    virtual protected bool DeathFunc() {
        return true;
    }

    // 離脱時、画面外にいったら削除
    protected void CheckOutArea() {
        if(movePaternClass.withdrawal || movePaternClass == null) {
            if(!CheckInScreen()) Destroy(this.gameObject);
        }
    }

    // 画面内外判定
    public bool CheckInScreen() {
        Vector3 pos = this.transform.position;
        if(pos.x < GameController.window_LUPos.x - enemySize.x / 2 || pos.x > GameController.window_RBPos.x + enemySize.x / 2 ||
           pos.y > GameController.window_LUPos.y + enemySize.y / 2 || pos.y < GameController.window_RBPos.y - enemySize.y / 2) {
            return false;
        } else {
            return true;
        }
    }

    // ダメージ関数
    public void Damage(int dmg) {
        damageCT = 0.05f;
        hp -= dmg;

        if(hp <= 0) {
            GameController.Instance.GameRankUp();
            time = 0.0f;
        }
    }

    public int GetHP_Value() {
        return hp;
    }

    // 爆発効果音のランダム再生
    protected void RandomExpSE_Small() {
        AudioClip se;
        if(Random.Range(0, 2) == 0) se = AudioManager.Instance.se_exp1;
        else                        se = AudioManager.Instance.se_exp2;
        AudioManager.Instance.PlayOneTimeAudio(se);
    }
    protected void RandomExpSE_Big() {
        AudioClip se;
        if(Random.Range(0, 2) == 0) se = AudioManager.Instance.se_exp3;
        else                        se = AudioManager.Instance.se_exp4;
        AudioManager.Instance.PlayOneTimeAudio(se);
    }

    // 衝突判定
    protected void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBullet" && this.hp > 0) {
            PlayerBullets pb = collision.GetComponent<PlayerBullets>();
            if(pb != null) {
                Damage(pb.GetDamage());
                GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Hit, collision.transform.position);
                eff.transform.localScale = new Vector2(0.5f, 0.5f);
                Destroy(collision.gameObject);
            }
        }
    }
}
