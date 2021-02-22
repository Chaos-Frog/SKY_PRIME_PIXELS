using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {
    public static TitleManager Instance;

    public static bool atFirst = true;  // 初回起動時判定

    private enum Menu_State {
        GameStart,
        SettingMenu,
        Credit,
        ShotKey,
        BombKey,
        SlowKey,
        PoseKey,
    }

    [SerializeField] private GameObject startLogo;     // 初回起動画面 ロゴ
    [SerializeField] private GameObject mainMenu;      // メニューオブジェクト
    [SerializeField] private GameObject menuCursor;    // メニューカーソル
    [SerializeField] private Image      fadePanel;     // フェードパネル
    [SerializeField] private Animator   menuAnimator;  // メニューアニメータ
    [SerializeField] private Text[]     configText;    // キーコンフィグテキスト
    [SerializeField] private Credit     creditScr;     // クレジット表記

    [Space(10)]
    [SerializeField] private GameObject rectObj;       // 背景オブジェクト
    [SerializeField] private Spectrum bgm_spec;        // BGMスペクトラム
    public float volume = 0.0f;                        // ボリュームデータ
    private float time = 0.0f;                         // 時間計測
    private int count = 0;                             // カウント

    [Space(10)]
    private AudioSource audioSource;                   // オーディオソース
    [SerializeField] private AudioClip select_SE;      // セレクト効果音
    [SerializeField] private AudioClip cancel_SE;      // キャンセル効果音
    [SerializeField] private AudioClip cursor_SE;      // カーソル効果音

    private bool animated = false;                     // アニメーション判定
    private bool keySetting = false;                   // キー入力待ち判定

    private Menu_State state = Menu_State.GameStart;   // カーソル位置

    void Start() {
        Instance = this;
        animated = true;

        ControllSetting.LoadKeyConfig();
        SetKeyConfigText();

        audioSource = this.GetComponent<AudioSource>();

        if(atFirst) {
            Camera.main.gameObject.layer = 0;
            StartCoroutine(nameof(StartUpAnimation));
        } else {
            bgm_spec.audio.Play();
            startLogo.SetActive(false);
            StartCoroutine(nameof(SceneStartAnimation));
        }
    }

    private void Update() {
        if(!atFirst) {
            if((int)state <= 2) {
                MenuControll();
            } else if(!keySetting) {
                ConfigMenuControll();
            }

            if(time >= 2.0f) {
                float xp = UnityEngine.Random.Range(-1.0f, 1.0f) + ((count - 1) * 2);
                Instantiate(rectObj, new Vector3(xp, -7.0f, 0), Quaternion.identity);
                time = UnityEngine.Random.Range(0.0f, 1.0f);
                count = (count + 1) % 3;
            }
            time += Time.deltaTime;
            volume = bgm_spec.spectrum.Select(x => x * 40).Sum() / bgm_spec.spectrum.Length;
        }
    }

    // メニュー操作
    private void MenuControll() {
        if(!animated) {
            float vert = Input.GetAxisRaw("Vertical");
            switch(state) {
                case Menu_State.GameStart:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        PlaySE(select_SE);
                        StartCoroutine(nameof(MoveGameScene));
                    } else {
                        if(vert < 0.0f || vert > 0.0f) {
                            if(vert < 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.SettingMenu;
                                StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                            } else if(vert > 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.Credit;
                                StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                            }
                        }
                    }
                    break;

                case Menu_State.SettingMenu:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        PlaySE(select_SE);
                        StartCoroutine(nameof(SlideKeyConfig), true);
                    } else if(vert < 0.0f || vert > 0.0f) {
                        if(vert < 0.0f) {
                            PlaySE(cursor_SE);
                            state = Menu_State.Credit;
                            StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                        } else if(vert > 0.0f) {
                            PlaySE(cursor_SE);
                            state = Menu_State.GameStart;
                            StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                        }
                    }
                    break;

                case Menu_State.Credit:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        PlaySE(select_SE);
                        StartCoroutine(CreditView());
                    } else if(vert < 0.0f || vert > 0.0f) {
                        if(vert < 0.0f) {
                            PlaySE(cursor_SE);
                            state = Menu_State.GameStart;
                            StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                        } else if(vert > 0.0f) {
                            PlaySE(cursor_SE);
                            state = Menu_State.SettingMenu;
                            StartCoroutine(nameof(CursorSlide), (int)state * -100.0f - 80.0f);
                        }
                    }
                    break;
            }
        }
    }

    // キーコンフィグ操作
    private void ConfigMenuControll() {
        if(!animated) {
            float vert = Input.GetAxisRaw("Vertical");
            if(Input.GetKey(KeyCode.Backspace) || ControllSetting.GetKey("Bomb")) {
                PlaySE(cancel_SE);
                StartCoroutine(nameof(SlideKeyConfig), false);
            } else {
                switch(state) {
                    case Menu_State.ShotKey:
                        if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                            PlaySE(select_SE);
                            StartCoroutine(nameof(SetKeyConfig), "Shot");
                        } else {
                            if(vert < 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.BombKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            } else if(vert > 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.PoseKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            }
                        }
                        break;

                    case Menu_State.BombKey:
                        if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                            PlaySE(select_SE);
                            StartCoroutine(nameof(SetKeyConfig), "Bomb");
                        } else {
                            if(vert < 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.SlowKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            } else if(vert > 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.ShotKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            }
                        }
                        break;

                    case Menu_State.SlowKey:
                        if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                            PlaySE(select_SE);
                            StartCoroutine(nameof(SetKeyConfig), "Slow");
                        } else {
                            if(vert < 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.PoseKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            } else if(vert > 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.BombKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            }
                        }
                        break;

                    case Menu_State.PoseKey:
                        if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                            PlaySE(select_SE);
                            StartCoroutine(nameof(SetKeyConfig), "Pose");
                        } else {
                            if(vert < 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.ShotKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            } else if(vert > 0.0f) {
                                PlaySE(cursor_SE);
                                state = Menu_State.SlowKey;
                                StartCoroutine(nameof(CursorSlide), ((int)state - 3) * -100.0f + 40.0f);
                            }
                        }
                        break;
                }
            }
        }
    }

    // キー設定反映
    private void SetKeyConfigText() {
        configText[0].text = ControllSetting.keyConfig["Shot"].ToString();
        configText[1].text = ControllSetting.keyConfig["Bomb"].ToString();
        configText[2].text = ControllSetting.keyConfig["Slow"].ToString();
        configText[3].text = ControllSetting.keyConfig["Pose"].ToString();
    }

    // SE再生
    private void PlaySE(AudioClip se) {
        audioSource.clip = se;
        audioSource.Play();
    }

    // キー登録コルーチン
    private IEnumerator SetKeyConfig(string key) {
        StartCoroutine(CursorSelect());

        keySetting = true;
        configText[(int)state - 3].text = "Press Use Key...";
        yield return new WaitForSeconds(0.5f);

        while(true) {
            if(Input.anyKey) {
                foreach(KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                    if(Input.GetKey(code)) {
                        PlaySE(select_SE);
                        ControllSetting.keyConfig[key] = code;
                        Debug.Log(ControllSetting.keyConfig[key].ToString());
                        ControllSetting.SaveKeyConfig();
                        SetKeyConfigText();
                        break;
                    }
                }
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        keySetting = false;
    }

    // 初回起動時アニメーションコルーチン
    private IEnumerator StartUpAnimation() {
        yield return new WaitForSeconds(1);
        startLogo.SetActive(true);
        yield return new WaitForSeconds(4);

        float time = 0.0f;
        while(time <= 0.5f) {
            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, time / 0.5f);
            time += Time.deltaTime;
            yield return null;
        }
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 1.0f);

        startLogo.SetActive(false);
        atFirst = false;

        yield return new WaitForSeconds(1);
        bgm_spec.audio.Play();
        Camera.main.gameObject.layer = 12;
        StartCoroutine(nameof(SceneStartAnimation));
    }

    // シーン開始時アニメーションコルーチン
    private IEnumerator SceneStartAnimation() {
        var fadeCor = StartCoroutine(nameof(FadeScreen), false);
        yield return fadeCor;

        mainMenu.SetActive(true);
        yield return new WaitForSeconds(1.4f);

        fadeCor = StartCoroutine(nameof(CursorFade), true);
        yield return fadeCor;

        animated = false;
    }

    // コンフィグメニュー遷移コルーチン
    private IEnumerator SlideKeyConfig(bool on) {
        animated = true;

        if(on) {
            var cor = StartCoroutine(CursorSelect());
            yield return cor;
        }

        var fadeCor = StartCoroutine(CursorFade(false));
        yield return fadeCor;

        menuAnimator.SetBool("KeyConfig", on);
        yield return new WaitForSeconds(0.4f);

        RectTransform rt = menuCursor.GetComponent<RectTransform>();
        if(on) {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 40.0f);
            state = Menu_State.ShotKey;
        } else {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -180.0f);
            state = Menu_State.SettingMenu;
        }

        fadeCor = StartCoroutine(nameof(CursorFade), true);
        yield return fadeCor;

        animated = false;
    }

    // クレジット表記コルーチン
    private IEnumerator CreditView() {
        animated = true;
        yield return StartCoroutine(creditScr.Credit_Coroutine(false));
        animated = false;
    }

    // メニューカーソルスライドコルーチン
    private IEnumerator CursorSlide(float end_y) {
        animated = true;

        RectTransform rt = menuCursor.GetComponent<RectTransform>();
        float start_y = rt.anchoredPosition.y;

        float time = 0.0f;
        while(time <= 0.2f) {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, Easing.Ease_Out_Quad(start_y, end_y, time / 0.2f));
            time += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, end_y);

        animated = false;
    }

    // メニューカーソルフェードコルーチン
    private IEnumerator CursorFade(bool fade_in) {
        float time = 0.0f;
        Image img_bar = menuCursor.GetComponent<Image>();
        Image img = menuCursor.transform.GetChild(0).GetComponent<Image>();

        while(time <= 0.2f) {
            float t = time / 0.2f;
            if(!fade_in) t = 1.0f - t;

            img_bar.color = new Color(img_bar.color.r, img_bar.color.g, img_bar.color.b, t * 0.4f);
            img.color = new Color(img.color.r, img.color.g, img.color.b, t);
            time += Time.deltaTime;
            yield return null;
        }

        float a = 1.0f;
        if(!fade_in) a = 0.0f;
        img_bar.color = new Color(img_bar.color.r, img_bar.color.g, img_bar.color.b, a * 0.4f);
        img.color = new Color(img.color.r, img.color.g, img.color.b, a);
        yield return null;
    }

    // メニューカーソル選択コルーチン
    private IEnumerator CursorSelect() {
        Image img_bar = menuCursor.GetComponent<Image>();
        img_bar.color = new Color(img_bar.color.r, img_bar.color.g, img_bar.color.b, 1.0f);
        yield return null;

        float time = 0.0f;
        while(time <= 0.2f) {
            float t = time / 0.2f;
            img_bar.color = new Color(img_bar.color.r, img_bar.color.g, img_bar.color.b, -0.6f * t + 1.0f);
            time += Time.deltaTime;
            yield return null;
        }

        img_bar.color = new Color(img_bar.color.r, img_bar.color.g, img_bar.color.b, 0.4f);
        yield return null;
    }

    // 画面フェードコルーチン
    private IEnumerator FadeScreen(bool fade_in) {
        float time = 0.0f;
        while(time <= 0.5f) {
            float t = time / 0.5f;
            if(!fade_in) t = 1.0f - t;

            fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, t);
            time += Time.deltaTime;
            yield return null;
        }

        float a = 0.0f;
        if(fade_in) a = 1.0f;
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, a);
        yield return null;
    }

    // 画面遷移コルーチン
    private IEnumerator MoveGameScene() {
        animated = true;
        
        var cor = StartCoroutine(CursorSelect());
        yield return cor;

        var fadeCor = StartCoroutine(nameof(CursorFade), false);
        yield return fadeCor;

        fadeCor = StartCoroutine(nameof(FadeScreen), true);
        yield return fadeCor;

        float time = 0.0f;
        while(time <= 0.5f) {
            float t = 1.0f - time / 0.5f;
            bgm_spec.audio.volume = t;

            time += Time.deltaTime;
            yield return null;
        }
        bgm_spec.audio.Stop();

        SceneManager.LoadScene("Game");
    }
}
