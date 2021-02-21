using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoseMenu : MonoBehaviour {
    public static PoseMenu Instance;
    public static GameObject InstanceObject;

    enum Menu_State {
        Resume,
        Restart,
        ReturnTitle,
    }

    [SerializeField] private Animator animator;       // アニメータ
    [SerializeField] private GameObject[] menuObj;    // メニューオブジェクト
    [SerializeField] private GameObject menuCursor;   // メニューカーソル
    [SerializeField] private GameObject realyWindow;  // 確認画面

    [HideInInspector] public bool animated = false;   // アニメーション判定

    private Menu_State state;                         // メニュー選択状態

    void Start() {
        Instance = this;
        InstanceObject = this.gameObject;
        state = Menu_State.Resume;
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        InstanceObject.SetActive(false);
    }

    void Update() {
        if(!animated) {
            float vert = Input.GetAxisRaw("Vertical");
            switch(state) {
                case Menu_State.Resume:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                        PoseChange();
                    } else if(vert < 0.0f || vert > 0.0f) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                        if(vert > 0.0f) {
                            state = Menu_State.ReturnTitle;
                        } else if(vert < 0.0f) {
                            state = Menu_State.Restart;
                        }
                        StartCoroutine(CursorSlide());
                    }
                    break;

                case Menu_State.Restart:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                        StartCoroutine(RealyCheck("RESTART GAME ?"));
                    } else if(vert < 0.0f || vert > 0.0f) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                        if(vert > 0.0f) {
                            state = Menu_State.Resume;
                        } else if(vert < 0.0f) {
                            state = Menu_State.ReturnTitle;
                        }
                        StartCoroutine(CursorSlide());
                    }
                    break;

                case Menu_State.ReturnTitle:
                    if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                        StartCoroutine(RealyCheck("RETURN TO TITLE ?"));
                    } else if(vert < 0.0f || vert > 0.0f) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                        if(vert > 0.0f) {
                            state = Menu_State.Restart;
                        } else if(vert < 0.0f) {
                            state = Menu_State.Resume;
                        }
                        StartCoroutine(CursorSlide());
                    }
                    break;
            }
        }
    }

    // ポーズ状態遷移
    public void PoseChange() {
        if(!Instance.animated) {
            if(!GameController.Instance.posed) {
                InstanceObject.SetActive(true);
                StartCoroutine(PoseEnable());
            } else {
                StartCoroutine(PoseDisable());
            }
        }
    }

    // ポーズ状態有効化コルーチン
    private IEnumerator PoseEnable() {
        AudioManager.Instance.bgm_as.Pause();

        animated = true;
        Time.timeScale = 0.0f;

        float time = 0.0f;
        while(time <= 0.02f) {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        GameController.Instance.posed = true;
        animated = false;
    }

    // ポーズ状態無効化コルーチン
    private IEnumerator PoseDisable() {
        animated = true;
        animator.SetTrigger("Close");

        float time = 0.0f;
        while(time <= 0.02f) {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1.0f;
        GameController.Instance.posed = false;
        animated = false;

        AudioManager.Instance.bgm_as.Play();
        InstanceObject.SetActive(false);
    }

    // カーソルスライドコルーチン
    private IEnumerator CursorSlide() {
        animated = true;

        RectTransform rt = menuCursor.GetComponent<RectTransform>();
        float first = rt.anchoredPosition.y;
        float end = ((int)state) * -70.0f + 30.0f;

        float time = 0.0f;
        while(time <= 0.2f) {
            float y_pos = Easing.Ease_Out_Quad(first, end, time / 0.2f);
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y_pos);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, end);
        yield return null;

        animated = false;
    }

    // 確認画面コルーチン
    private IEnumerator RealyCheck(string menu) {
        animated = true;

        realyWindow.SetActive(true);
        bool realy = false;
        Text title = realyWindow.transform.GetChild(0).GetComponent<Text>();
        title.text = menu;

        RectTransform rt = realyWindow.transform.GetChild(3).GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(0.0f, -60.0f);

        float time = 0.0f;
        while(time <= 0.3f) {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        bool slided = false;
        while(true) {
            if(!slided) {
                if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) {
                    break;
                } else if(Input.GetKey(KeyCode.Escape) || ControllSetting.GetKey("Pose")) {
                    AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cancel);
                    realy = false;
                    break;
                } else {
                    float vert = Input.GetAxisRaw("Vertical");
                    if(vert > 0.0f || vert < 0.0f) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                        realy = !realy;
                        slided = true;
                    }
                }
            } else {
                float first = rt.anchoredPosition.y;
                float end = 0.0f;
                if(!realy) end = -60.0f;

                time = 0.0f;
                while(time <= 0.2f) {
                    float y_pos = Easing.Ease_Out_Quad(first, end, time / 0.2f);
                    rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, y_pos);
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }
                slided = false;
            }

            yield return null;
        }

        if(realy) {
            switch(state) {
                case Menu_State.Restart:
                    AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                    GameController.Instance.MoveScene("Game");
                    break;
                case Menu_State.ReturnTitle:
                    AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                    GameController.Instance.MoveScene("Title");
                    break;
            }
        } else {
            AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cancel);
            Animator anim = realyWindow.GetComponent<Animator>();
            anim.SetTrigger("Close");
            time = 0.0f;
            while(time <= 0.3f) {
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            animated = false;
            realyWindow.SetActive(false);
            yield return null;
        }
    }
}