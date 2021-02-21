using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShotType {
    FourWay,
    Homing,
    Spread
}

public class Player : MonoBehaviour {
    [SerializeField] private PlayerParameter param;       // プレイヤーパラメータ

    [SerializeField] public ShotType shotType_Primary;    // プレイヤーショットタイプ_1
    [SerializeField] public ShotType shotType_Secondary;  // プレイヤーショットタイプ_2
    [SerializeField] private GameObject bulletObj;        // 弾オブジェクト
    [SerializeField] private GameObject bulletObj_s;      // 弾オブジェクト_Spread
    [SerializeField] private GameObject homingObj;        // ホーミング弾オブジェクト
    [SerializeField] private ParticleSystem boost;        // ブーストエフェクト

    private bool slowMove;                                // 低速移動判定

    private Vector2 playerSize;                           // プレイヤー画像サイズ
    private const float correctionValue = 0.7071f;        // 斜め移動係数
    private float shotCT;                                 // ショットクールタイム
    private float damageCT;                               // ダメージクールタイム
    private ParticleSystem barrier;                       // バリアエフェクト

    private AudioSource audio;

    void Start() {
        // ブースト停止
        boost.Stop();

        // 画像サイズ取得
        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        playerSize = sr.sprite.rect.size / sr.sprite.pixelsPerUnit;
        playerSize *= this.transform.localScale;

        // 当たり判定サイズ設定
        CircleCollider2D col = this.gameObject.GetComponent<CircleCollider2D>();
        col.radius = param.coreRadius;
        Transform core = this.transform.GetChild(0);
        core.localScale *= param.coreRadius * 10.0f + 0.2f;

        audio = this.GetComponent<AudioSource>();
    }

    void FixedUpdate() {
        if(!StartMenu.Instance.gameObject.activeSelf) {
            if(damageCT <= 2.0f && GameController.Instance.life > 0 && !GameController.Instance.gameEnd) {
                Moving();
                MovingLimit();
                ShotBullets();
                UseBomber();
            }
            DamageEffect();
        }
    }

    // 移動関数
    void Moving() {
        Vector3 moveVector = new Vector3(0, 0, 0);

        // 低速移動
        float spd = param.moveSpeed;
        if(ControllSetting.GetKey("Slow")) {
            spd *= 0.7f;
            slowMove = true;
        } else {
            slowMove = false;
        }

        // 横軸入力
        float hori = Input.GetAxisRaw("Horizontal");
        if(hori > 0.0f)      hori = 1.0f;
        else if(hori < 0.0f) hori = -1.0f;

        // 縦軸入力
        float vert = Input.GetAxisRaw("Vertical");
        if(vert > 0.0f)      vert = 1.0f;
        else if(vert < 0.0f) vert = -1.0f;

        moveVector.x = spd * Time.deltaTime * hori;
        moveVector.y = spd * Time.deltaTime * vert;

        // 斜め移動時に補正をかける
        if(!(hori == 0 ^ vert == 0)) moveVector *= correctionValue;

        this.transform.position += moveVector;
    }

    // 移動制限関数
    void MovingLimit() {
        // 画面外に移動した場合、画面内に戻す
        Vector3 pos = this.transform.position;
        if(pos.x < GameController.window_LUPos.x + playerSize.x / 2) pos.x = GameController.window_LUPos.x + playerSize.x / 2;
        if(pos.x > GameController.window_RBPos.x - playerSize.x / 2) pos.x = GameController.window_RBPos.x - playerSize.x / 2;
        if(pos.y > GameController.window_LUPos.y - playerSize.y / 2 - 1.0f) pos.y = GameController.window_LUPos.y - playerSize.y / 2 - 1.0f;
        if(pos.y < GameController.window_RBPos.y + playerSize.y / 2 + 1.0f) pos.y = GameController.window_RBPos.y + playerSize.y / 2 + 1.0f;
        this.transform.position = pos;
    }

    // ボム発射関数
    void UseBomber() {
        if(GameController.Instance.bomb > 0 && GameController.Instance.bomber_time <= 0.0f) {
            if(ControllSetting.GetKey("Bomb")) {
                audio.PlayOneShot(AudioManager.Instance.se_expL);
                EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Bomber, this.transform.position);
                GameController.Instance.UseBomber();
            }
        }
    }

    // 弾発射関数
    void ShotBullets() {
        if(shotCT <= 0) {
            if(ControllSetting.GetKey("Shot")) {
                audio.PlayOneShot(AudioManager.Instance.se_shot);
                // 共通ショット
                Vector3[] shotPos = new Vector3[2];
                shotPos[0] = this.transform.position + new Vector3(0.4f, 0, 0);
                shotPos[1] = this.transform.position + new Vector3(-0.4f, 0, 0);
                for(int i = 0; i < 2; i++) {
                    GameObject bullet = Instantiate(bulletObj, shotPos[i], Quaternion.identity);
                    PlayerBullets pb = bullet.GetComponent<PlayerBullets>();
                    pb.SetSpeed(param.defaultShot_speed);
                }

                if(!slowMove) {
                    TypeShot(shotType_Primary, slowMove);
                } else {
                    TypeShot(shotType_Secondary, slowMove);
                }

                shotCT = param.shotInterval;
            }
        } else {
            shotCT -= Time.deltaTime;
        }
    }

    // デモ発射関数
    public void ShotBullets_Demo(bool prim) {
        if(shotCT <= 0) {
            // 共通ショット
            Vector3[] shotPos = new Vector3[2];
            shotPos[0] = this.transform.position + new Vector3(0.4f, 0, 0);
            shotPos[1] = this.transform.position + new Vector3(-0.4f, 0, 0);
            for(int i = 0; i < 2; i++) {
                GameObject bullet = Instantiate(bulletObj, shotPos[i], Quaternion.identity);
                PlayerBullets pb = bullet.GetComponent<PlayerBullets>();
                pb.SetSpeed(param.defaultShot_speed);
            }

            if(prim) {
                TypeShot(shotType_Primary, !prim);
            } else {
                TypeShot(shotType_Secondary, !prim);
            }

            shotCT = param.shotInterval;
        } else {
            shotCT -= Time.deltaTime;
        }
    }

    // タイプ別ショット
    private void TypeShot(ShotType type, bool slow) {
        switch(type) {
            case ShotType.FourWay:
                if(!slow) {
                    NWayShot(4, 0.0f, param.fourWay_betweenAngle, param.fourWay_speed);
                } else {
                    NWayShot(4, 0.0f, param.fourWay_betweenAngle_slow, param.fourWay_speed);
                }
                break;

            case ShotType.Homing:
                for(int i = 0; i < 4; i++) {
                    if(!slow) {
                        Instantiate(homingObj, this.transform.position, Quaternion.Euler(0.0f, 0.0f, (i - 2) * 30.0f - 180.0f + 15.0f));
                    } else {
                        Instantiate(homingObj, this.transform.position, Quaternion.Euler(0.0f, 0.0f, (i - 2) * 20.0f - 180.0f + 10.0f));
                    }
                }
                break;

            case ShotType.Spread:
                for(int i = 0; i < 4; i++) {
                    float a;
                    if(!slow) {
                        a = param.spread_betweenAngle;
                    } else {
                        a = param.spread_betweenAngle_slow;
                    }
                    float zAngle = (i - 2) * a + (a / 2.0f) + Random.Range(-(a / 2.0f), (a / 2.0f));
                    GameObject bullet = Instantiate(bulletObj_s, this.transform.position, Quaternion.Euler(0.0f, 0.0f, zAngle));
                    PlayerBullets pb = bullet.GetComponent<PlayerBullets>();
                    pb.SetSpeed(Random.Range(param.spread_minSpeed, param.spread_maxSpeed));
                }
                break;
        }
    }

    // N Way ショット
    private void NWayShot(int num, float baseAngle, float betweenAngle, float speed) {
		for (int i = 0; i < num; i++) {
            float zAngle = (i * betweenAngle) + baseAngle - ((num / 2) * betweenAngle);
			if (num % 2 == 0) zAngle += betweenAngle / 2.0f;

            Quaternion quat = Quaternion.Euler(0.0f, 0.0f, zAngle);
            GameObject bullet = Instantiate(bulletObj, this.transform.position, quat);
			PlayerBullets pb = bullet.GetComponent<PlayerBullets>();
			pb.SetSpeed(speed);
		}
	}

    // ダメージ表現
    private void DamageEffect() {
        if(damageCT > 0.0f) {
            SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
            float t = (Mathf.Cos(360.0f * damageCT * Mathf.Deg2Rad) + 1.0f) / 2.0f;
            sr.color = new Color(1.0f, t, t);

            damageCT -= Time.deltaTime;

            if(damageCT <= 0.5f) {
                if(barrier != null) barrier.Stop();
            } else {
                GameController.Instance.DeleteAllEBullets();
            }

            if(damageCT <= 0.0f) {
                sr.color = new Color(1.0f, 1.0f, 1.0f);
            }
        }
    }

    // 被弾処理
    void DamageFunction() {
        audio.PlayOneShot(AudioManager.Instance.se_break);
        damageCT = 3.0f;

        EffectManager.Instance.InstantiateEffect(EffectManager.Effects.PlayerHit, transform.position);

        GameController.Instance.LifeDamage();
        GameController.Instance.ResetBomber();

        if(GameController.Instance.life == 0) {
            StartCoroutine(nameof(ShootDown), 2.0f);
        } else {
            GameObject b_eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.PlayerBarrier, transform.position);
            b_eff.transform.parent = transform;
            barrier = b_eff.GetComponent<ParticleSystem>();
        }
    }

    // スタンバイ
    public void StandbyPlayer() {
        boost.Play();
    }

    // 被弾演出コルーチン
    private IEnumerator DamageDirecting(float anim_t) {
        boost.Stop();
        float time = 0.0f;
        while(time <= anim_t) {
            float spd = Easing.Ease_Out_Quad(-4.0f, 0.0f, time / anim_t);
            transform.position += Vector3.up * spd * Time.deltaTime;
            MovingLimit();
            time += Time.deltaTime;
            yield return null;
        }
        if(GameController.Instance.life != 0) {
            boost.Play();
        }
        yield return null;
    }

    // 撃墜演出コルーチン
    private IEnumerator ShootDown(float anim_t) {
        AudioManager.Instance.AudioFadeOut(AudioManager.Instance.bgm_as, 2.0f);
        
        SpriteRenderer core_sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        core_sr.enabled = false;

        boost.Stop();

        float time = 0.0f;
        while(time <= anim_t) {
            float spd = Easing.Ease_In_Quad(0.0f, -3.0f, time / anim_t);
            transform.position += Vector3.up * spd * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 60.0f * time / anim_t);
            time += Time.deltaTime;
            yield return null;
        }

        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;

        Time.timeScale = 0.25f;
        GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position);
        audio.PlayOneShot(AudioManager.Instance.se_expL);
        eff.transform.localScale = new Vector2(2.0f, 2.0f);
        

        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;
        GameController.Instance.gameOverCanvas.SetActive(true);
    }

    // 衝突判定
    private void OnTriggerEnter2D(Collider2D collision) {
        if(GameController.Instance.life > 0) {
            if(damageCT <= 0.0f && GameController.Instance.bomber_time <= 0.0f) {
                switch(collision.tag) {
                    // Enemy 衝突
                    case "Enemy":
                        StartCoroutine(nameof(DamageDirecting), 1.0f);
                        DamageFunction();
                        break;

                    // EnemyBullet 衝突
                    case "EnemyBullet":
                        StartCoroutine(nameof(DamageDirecting), 1.0f);
                        DamageFunction();
                        Destroy(collision.gameObject);
                        break;
                }
            }
        }
    }
}
