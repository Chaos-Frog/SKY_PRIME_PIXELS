using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Boss : Enemy {
    public static Enemy_Boss Instance;

    private enum Phase {
        Standy,
        Phase_1,
        Phase_2,
        Phase_3,
        Phase_Ex,
    }

    [Space(10)]
    [SerializeField] private GameObject bitPrefabs;    // ビットプレハブ

    [HideInInspector] public GameObject[] bitObjects;  // ビット配列

    [HideInInspector] public bool endDeath;  // 死亡演出終了

    private Phase phase;  // フェーズ
    private int   maxHP;  // 最大HP

    void Start() {
        Instance = this;

        phase = Phase.Standy;
        endDeath = false;

        maxHP = hp;

        SpriteRenderer sr = this.gameObject.GetComponent<SpriteRenderer>();
        enemyColor = sr.color;

        bitObjects = new GameObject[4];
        for(int i = 0; i < 4; i++) {
            bitObjects[i] = Instantiate(bitPrefabs);
        }

        this.transform.position = new Vector3(0.0f, 12.0f, 0.0f);

        this.GetComponent<Collider2D>().enabled = false;
        for(int i = 0; i < 4; i++) {
            bitObjects[i].GetComponent<Collider2D>().enabled = false;
        }

        StartCoroutine(StandbyCorutine());
    }

    void FixedUpdate() {
        if(phase != Phase.Standy) {
            if(hp > 0) {
                // ビットが全て撃破された場合発狂
                bool bit_active = false;
                for(int i = 0; i < bitObjects.Length; i++) {
                    if(bitObjects[i].activeSelf) {
                        bit_active = true;
                    }
                }

                if(!bit_active && phase != Phase.Phase_Ex) {
                    ChangePhase(Phase.Phase_Ex);
                }

                // 移動と弾幕のサイクル
                bool do_move = MoveBoss();
                bool do_danmaku = DanmakuBoss();
                if(!do_move && !do_danmaku) {
                    switch(phase) {
                        case Phase.Phase_1:
                            ChangePhase(Phase.Phase_2);
                            break;
                        case Phase.Phase_2:
                            ChangePhase(Phase.Phase_3);
                            break;
                        case Phase.Phase_3:
                            ChangePhase(Phase.Phase_1);
                            break;
                    }
                }

                RectTransform gage_rt = GameController.Instance.boss_HPGage.GetComponent<RectTransform>();
                gage_rt.localScale = new Vector3((float)hp / maxHP, 1.0f, 1.0f);

                DamageEffect();
                time += Time.deltaTime;
            } else {
                GameController.Instance.DeleteAllEBullets();
                if(DeathFunc()) Destroy(this.gameObject);
            }
        }
    }

    // 移動関数 終了時falseを返す
    private bool MoveBoss() {
        movePaternClass.Moving();

        if(!movePaternClass.withdrawal) {
            return true;
        } else {
            return false;
        }
    }

    // 弾幕関数 終了時falseを返す
    private bool DanmakuBoss() {
        danmakuPaternClass.Shot();

        if(!danmakuPaternClass.end) {
            return true;
        } else {
            return false;
        }
    }

    // フェーズ切り替え
    private void ChangePhase(Phase next) {
        movePaternClass = Base_MovePatern.SetMovePaternClass(moveParameters[(int)next - 1]);
        movePaternClass.Init(this.gameObject);

        danmakuPaternClass = Base_DanmakuPatern.SetDanmakuPaternClass(danmakuParameters[(int)next - 1]); ;
        danmakuPaternClass.Init(this.gameObject);

        phase = next;
    }

    // 死亡時処理 消滅時に true を返す
    protected override bool DeathFunc() {
        if(time <= 0.0f) {
            this.gameObject.GetComponent<Collider2D>().enabled = false;

            // 残ったビット自爆
            for(int i = 0; i < 4; i++) {
                if(bitObjects[i].activeSelf) {
                    bitObjects[i].GetComponent<Boss_Bit>().SelfDestroy();
                }
            }

            // 残ったミサイル自爆
            GameObject[] missiles = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject obj in missiles) {
                Enemy_Missile scr = obj.GetComponent<Enemy_Missile>();
                if(scr != null) {
                    scr.SelfDestroy();
                }
            }

            // スコア加算
            GameController.Instance.AddScore(scorePoint);

            // スコアテキスト表示
            Text text = Instantiate(scoreTextObj).GetComponent<Text>();
            ScoreText textScr = text.GetComponent<ScoreText>();
            textScr.SetScore(scorePoint);
            text.transform.SetParent(GameObject.Find("Canvas").transform, false);
            text.transform.localScale *= 2.0f;
            text.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, this.transform.position);

            // 死亡演出コルーチン開始
            StartCoroutine(DeathDirecting());
            time = 1.0f;
        }

        if(endDeath) {
            return true;
        } else {
            return false;
        }
    }

    // スタンバイコルーチン
    private IEnumerator StandbyCorutine() {
        float time = 0.0f;
        Vector3 start = new Vector3(0.0f, 12.0f, 0.0f);
        Vector3 end = new Vector3(0.0f, 5.0f, 0.0f);
        while(time <= 2.0f) {
            Vector3 pos = Easing.Ease_Out_Quad(start, end, time / 2.0f);
            this.transform.position = pos;
            for(int i = 0; i < 4; i++) {
                bitObjects[i].transform.position = pos;
            }
            time += Time.deltaTime;
            yield return null;
        }
        this.transform.position = end;
        for(int i = 0; i < 4; i++) {
            bitObjects[i].transform.position = end;
        }
        yield return null;

        this.GetComponent<Collider2D>().enabled = true;
        for(int i = 0; i < 4; i++) {
            bitObjects[i].GetComponent<Collider2D>().enabled = true;
        }

        ChangePhase(Phase.Phase_1);
    }

    // 死亡演出コルーチン
    private IEnumerator DeathDirecting() {
        AudioManager.Instance.AudioFadeOut(AudioManager.Instance.bgm_as, 1.0f);

        RandomExpSE_Big();

        GameController.Instance.boss_HPGage.SetActive(false);
        StartCoroutine(SkyColorReset());
        var move_cor = StartCoroutine(DeathMoving());

        GameObject eff = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Exp_Charge, this.transform.position);
        eff.transform.SetParent(this.transform);
        eff.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        for(int i = 0; i < 12; i++) {
            float angle = 180.0f + (120 * i) + Random.Range(-5.0f, 5.0f);
            Vector3 pos = Quaternion.Euler(0, 0, angle) * Vector3.up * Random.Range(1.0f, 1.4f);
            GameObject exp_s = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position + pos);
            RandomExpSE_Small();
            exp_s.transform.localScale *= 1.0f;
            yield return new WaitForSeconds(0.2f);
        }

        for(int i = 0; i < 24; i++) {
            float angle = 180.0f + (120 * i) + Random.Range(-25.0f, 25.0f);
            Vector3 pos = Quaternion.Euler(0, 0, angle) * Vector3.up * Random.Range(1.2f, 1.8f);
            GameObject exp_s = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position + pos);
            RandomExpSE_Small();
            exp_s.transform.localScale *= 1.0f;
            yield return new WaitForSeconds(0.1f);
        }

        yield return move_cor;

        Time.timeScale = 0.2f;
        this.gameObject.GetComponent<Renderer>().enabled = false;
        GameObject exp_l = EffectManager.Instance.InstantiateEffect(EffectManager.Effects.Explosion, transform.position);
        AudioManager.Instance.PlayOneTimeAudio(AudioManager.Instance.se_expL);
        exp_l.transform.localScale *= 2.5f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1.0f;

        yield return new WaitForSeconds(1.0f);

        GameController.Instance.gameEnd = true;
        GameController.Instance.gameOverCanvas.SetActive(true);

        for(int i = 0; i < 4; i++) {
            Destroy(bitObjects[i]);
        }
        Destroy(this.gameObject);

        yield return null;
    }

    // 死亡演出コルーチン　移動
    private IEnumerator DeathMoving() {
        float time = 0.0f;
        while(time <= 6.0f) {
            float spd = Easing.Ease_In_Quad(0.0f, -3.0f, time / 6.0f);
            transform.position += Vector3.up * spd * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, 60.0f * (time / 6.0f) + 180.0f);
            time += Time.deltaTime;
            yield return null;
        }
    }

    // 空の色を元に
    private IEnumerator SkyColorReset() {
        float cor_time = 0.0f;
        while(cor_time <= 3.0f) {
            float t = cor_time / 3.0f;
            Camera.main.backgroundColor = (GameController.Instance.skyColor_N - GameController.Instance.skyColor_B) * t + GameController.Instance.skyColor_B;
            cor_time += Time.deltaTime;
            yield return null;
        }
        Camera.main.backgroundColor = GameController.Instance.skyColor_N;
    }
}
