using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController Instance;

    [SerializeField] private GameObject[] enemys;   // 敵プレハブ
    [SerializeField] private GameObject boss;       // ボスプレハブ
    [SerializeField] private GameObject lifeGage;   // ライフゲージプレハブ
    [SerializeField] private GameObject bombGage;   // ボンバーゲージプレハブ

    [Space(15)]
    public Color skyColor_N;                        // 背景色（通常）
    public Color skyColor_B;                        // 背景色（ボス）

    [Space(15)]
    [SerializeField] private TextAsset stageJson;   // ステージJsonデータ

    [Space(15)]
    [SerializeField] private float time;            // 経過時間
    [SerializeField] private int   wave;            // 経過Wave
    [SerializeField] private float bossTime;        // ボス出現時間

    [Space(15)]
    public GameObject playerObj;                    // プレイヤーオブジェクト
    [SerializeField] public int maxLife;            // プレイヤー最大ライフ（残機）
    [SerializeField] public int maxBomb;            // プレイヤー最大ボンバー

    [Space(15)]
    [SerializeField] public uint score;             // スコア
    [SerializeField] private Text scoreText;        // スコアテキストオブジェクト

    [Space(15)]
    [SerializeField] private GameObject warning;    // ワーニングオブジェクト
    public GameObject boss_HPGage;                  // ボスHPゲージ

    [Space(15)]
    public int gameRank;                            // ゲームランク
    public int gameRank_Max;                        // Maxゲームランク
    [SerializeField] private float rankUpTime;      // ゲームランク上昇時間
    [SerializeField] private Text gameRankText;     // ランクテキストオブジェクト
    private float rankTimer;                        // ランク上昇タイマー

    [Space(15)]
    public GameObject uiCanvas;                     // UIキャンバス
    public GameObject fadeCanvas;                   // フェード用キャンバス
    public GameObject gameOverCanvas;               // ゲームオーバーキャンバス

    [Space(15)]
    public bool DebugMode;                          // デバッグモード
    [SerializeField] private StageDataItem test_E;  // テスト用敵データ

    private StageData stage;                        // ステージデータ

    public static Vector3 window_LUPos;             // 画面左上の座標
    public static Vector3 window_RBPos;             // 画面右下の座標

    [HideInInspector] public float bomber_time;     // ボンバータイム
    [HideInInspector] public int life;              // プレイヤーライフ
    [HideInInspector] public int bomb;              // プレイヤーボンバー

    [HideInInspector] public bool posed;            // ポーズ判定
    [HideInInspector] public bool bossPhase;        // ボス戦判定
    [HideInInspector] public bool gameEnd;          // ゲーム終了判定

    private GameObject[] life_Gages;                // ライフゲージ配列
    private GameObject[] bomb_Gages;                // ボムゲージ配列

    void Start() {
        Instance = this;
        ControllSetting.LoadKeyConfig();

        GameObject under = uiCanvas.transform.GetChild(2).gameObject;

        // ライフセット
        life = maxLife;
        life_Gages = new GameObject[maxLife];
        for(int i = 0; i < maxLife; i++) {
            life_Gages[i] = Instantiate(lifeGage);
            life_Gages[i].transform.SetParent(under.transform, false);
            Image img =life_Gages[i].GetComponent<Image>();
            img.rectTransform.anchoredPosition = new Vector2(i * 60.0f + 160.0f, 12.0f);
        }

        // ボンバーセット
        bomb = maxBomb;
        bomb_Gages = new GameObject[maxBomb];
        for(int i = 0; i < maxBomb; i++) {
            bomb_Gages[i] = Instantiate(bombGage);
            bomb_Gages[i].transform.SetParent(under.transform, false);
            Image img = bomb_Gages[i].GetComponent<Image>();
            img.rectTransform.anchoredPosition = new Vector2(i * -60.0f - 160.0f, 12.0f);
        }

        // キャンバス非アクティブ
        uiCanvas.SetActive(false);

        // スコアセット
        scoreText.text = score.ToString();
        gameRankText.text = "Rank:" + gameRank.ToString("D3");

        // フェードイン
        StartCoroutine(FadeScreen(false));

        // メインカメラ取得
        Camera mainCamera = Camera.main;
        // 画面左上の座標取得
        window_LUPos = mainCamera.ViewportToWorldPoint(Vector3.zero);
        window_LUPos.Scale(new Vector3(1f, -1f, 1f));
        // 画面右下の座標取得
        window_RBPos = mainCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));
        window_RBPos.Scale(new Vector3(1f, -1f, 1f));

        Debug.Log("Window_LUPosition:" + window_LUPos);
        Debug.Log("Window_RBPosition:" + window_RBPos);

        score = 0;

        if(!DebugMode) {
            // ステージデータセット
            SetStageData(stageJson.ToString());
        }
    }

    private void Update() {
        if(!gameEnd) {
            // ポーズ設定
            if(Input.GetKeyDown(KeyCode.Escape) || ControllSetting.GetKeyDown("Pose")) {
                PoseMenu.Instance.PoseChange();
            }
        }
    }

    void FixedUpdate() {
        if(!StartMenu.Instance.setting) {
            if(!gameEnd) {
                StageUpdate();
                BomberUpdate();

                time += Time.deltaTime;
            }


            // 時間経過 ランクアップ
            if(time >= 0.0f && !gameEnd) {
                rankTimer += Time.deltaTime;
                if(rankTimer >= rankUpTime) {
                    GameRankUp();
                    rankTimer -= rankUpTime;
                }
            }

            // 背景スクロール
            if(time >= 0.0f && time <= 3.0f) {
                BackGround.Instance.scrollSpeed = Easing.Ease_In_Sine(20.0f, 6.0f, time / 3.0f);
            }
        }
    }

    // ステージ処理
    private void StageUpdate() {
        if(!bossPhase) {
            // ボス戦突入
            if(time >= bossTime && GameObject.FindGameObjectsWithTag("Enemy").Length == 0) {
                bossPhase = true;
                StartCoroutine(AttackBoss());

                //gameEnd = true;
                //gameOverCanvas.SetActive(true);

                // 敵弾消去
                DeleteAllEBullets();
            } else {
                if(!DebugMode) {
                    if(wave < stage.items.Length) {
                        while(stage.items[wave].spawnTime <= time) {
                            GameObject e = Instantiate(
                                enemys[stage.items[wave].enemyType],
                                new Vector3(stage.items[wave].position_X, stage.items[wave].position_Y, 0.0f),
                                Quaternion.Euler(0.0f, 0.0f, 180.0f)
                            );
                            Enemy es = e.GetComponent<Enemy>();
                            es.SetEnemy(stage.items[wave].movePatern, stage.items[wave].danmakuPatern);
                            wave++;

                            if(wave >= stage.items.Length) break;
                        }
                    }
                } else {
                    if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && time >= 0.0f) {
                        GameObject e = Instantiate(
                            enemys[test_E.enemyType],
                            new Vector3(test_E.position_X, test_E.position_Y, 0.0f),
                            Quaternion.Euler(0.0f, 0.0f, 180.0f)
                        );
                        Enemy es = e.GetComponent<Enemy>();
                        es.SetEnemy(test_E.movePatern, test_E.danmakuPatern);
                    }
                }
            }
        }
    }

    // ボンバー中処理
    private void BomberUpdate() {
        if(bomber_time > 0.0f) {
            // 敵弾消去
            DeleteAllEBullets();

            // 敵ダメージ
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject e in enemys) {
                Enemy e_scr = e.GetComponent<Enemy>();
                if(e_scr.CheckInScreen() && e_scr.GetHP_Value() > 0) {
                    e_scr.Damage(1);
                }
            }

            bomber_time -= Time.deltaTime;
        }
    }

    // ステージデータ設定
    private void SetStageData(string json) {
        stage = JsonUtility.FromJson<StageData>(json);
    }

    // スコア加算
    public void AddScore(uint sc, bool rank = true) {
        if(bomber_time <= 0.0f && rank) {
            sc = (uint)(sc * (gameRank / 10.0f + 1.0f));
        }
        score += sc;
        scoreText.text = score.ToString();
    }

    // ライフ（残機）減少
    public void LifeDamage() {
        life--;
        gameRank /= 3;
        gameRankText.text = "Rank:" + gameRank.ToString("D3");
        Image img = life_Gages[life].GetComponent<Image>();
        img.enabled = false;

        if(life == 0) {
            gameEnd = true;
        }
    }

    // ボンバー消費
    public void UseBomber() {
        bomb--;
        gameRank /= 2;
        gameRankText.text = "Rank:" + gameRank.ToString("D3");
        bomber_time = 3.0f;
        Image img = bomb_Gages[bomb].GetComponent<Image>();
        img.enabled = false;
    }

    // ボンバーリセット
    public void ResetBomber() {
        bomb = maxBomb;
        foreach(GameObject obj in bomb_Gages) {
            Image img = obj.GetComponent<Image>();
            img.enabled = true;
        }
    }

    // ゲームランク上昇
    public void GameRankUp() {
        if(gameRank < gameRank_Max) {
            gameRank++;
            gameRankText.text = "Rank:" + gameRank.ToString("D3");
        }
    }

    // 全敵弾消去
    public void DeleteAllEBullets() {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach(GameObject b in bullets) {
            if(b.GetComponent<Collider2D>().enabled) {
                b.GetComponent<EnemyBullets>().DestroyBullet();
            }
        }
    }

    // シーン遷移
    public void MoveScene(string scene) {
        StartCoroutine(MoveSceneCorutine(scene));
    }

    // シーン遷移コルーチン
    private IEnumerator MoveSceneCorutine(string scene) {
        var cor = StartCoroutine(FadeScreen(true));
        yield return cor;

        float time = 0.0f;
        while(time <= 0.5f) {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        posed = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(scene);
    }

    // フェードスクリーン
    private IEnumerator FadeScreen(bool fade_in) {
        Image img = fadeCanvas.transform.GetChild(0).GetComponent<Image>();

        float time = 0.0f;
        while (time <= 0.5f) {
            float t = time / 0.5f;
            if(!fade_in) t = 1.0f - t;
            img.color = new Color(img.color.r, img.color.g, img.color.b, t);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        float a = 1.0f;
        if(!fade_in) a = 0.0f;
        img.color = new Color(img.color.r, img.color.g, img.color.b, a);
        yield return null;
    }

    // ボス戦突入コルーチン
    private IEnumerator AttackBoss() {
        AudioManager.Instance.AudioFadeOut(AudioManager.Instance.bgm_as, 0.2f);
        yield return new WaitForSeconds(0.2f);

        warning.SetActive(true);
        AudioManager.Instance.bgm_as.clip = AudioManager.Instance.se_warning;
        AudioManager.Instance.bgm_as.loop = false;
        AudioManager.Instance.bgm_as.volume = 1.0f;
        AudioManager.Instance.bgm_as.Play();
        yield return new WaitForSeconds(4.0f);

        float cor_time = 0.0f;
        while(cor_time <= 3.0f) {
            float t = cor_time / 3.0f;
            Camera.main.backgroundColor = (skyColor_B - skyColor_N) * t + skyColor_N;
            BackGround.Instance.scrollSpeed = Easing.Ease_In_Sine(6.0f, 20.0f, t);
            cor_time += Time.deltaTime;
            yield return null;
        }
        Camera.main.backgroundColor = skyColor_B;
        BackGround.Instance.scrollSpeed = 20.0f;
        Instantiate(boss);

        AudioManager.Instance.bgm_as.clip = AudioManager.Instance.bgm_boss;
        AudioManager.Instance.bgm_as.loop = true;
        AudioManager.Instance.bgm_as.volume = 0.5f;
        AudioManager.Instance.bgm_as.Play();

        cor_time = 0.0f;
        while(cor_time <= 2.0f) {
            float t = cor_time / 2.0f;
            boss_HPGage.SetActive(true);
            RectTransform rt = boss_HPGage.GetComponent<RectTransform>();
            rt.localScale = new Vector3(t, 1.0f, 1.0f);
            cor_time += Time.deltaTime;
            yield return null;
        }
    }
}