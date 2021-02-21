using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
    public static StartMenu Instance;

    [SerializeField] private GameObject dummy;      // ダミー敵
    [SerializeField] private GameObject cursor;     // カーソル
    [SerializeField] private GameObject primary;    // プライマリーテキスト
    [SerializeField] private GameObject secondary;  // セカンダリーテキスト
    [SerializeField] private Text       message;    // メッセージテキスト

    private ShotType prim_shot;  // primaryショット
    private ShotType seco_shot;  // secondaryショット
    private bool select_prim;    // カーソル選択

    private float demoCT;        // デモ用クールタイム

    private bool animated;       // アニメーション判定
    public bool setting;         // セッティング判定

    private Player player;

    void Start() {
        Instance = this;
        setting = true;

        prim_shot = ShotType.FourWay;
        seco_shot = ShotType.FourWay;
        select_prim = true;

        primary.GetComponent<Text>().text = prim_shot.ToString();
        secondary.GetComponent<Text>().text = seco_shot.ToString();
    }

    void FixedUpdate() {
        if(!player) {
            player = GameController.Instance.playerObj.GetComponent<Player>();
            player.shotType_Primary = prim_shot;
            player.shotType_Secondary = seco_shot;

            AudioManager.Instance.bgm_as.clip = AudioManager.Instance.bgm_standby;
            AudioManager.Instance.bgm_as.volume = 0;
            AudioManager.Instance.bgm_as.Play();
            AudioManager.Instance.AudioFadeIn(AudioManager.Instance.bgm_as, 0.5f);
        }

        if(!animated) {
            if(ControllSetting.GetKeyDown("Shot")) {
                AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_select);
                StartCoroutine(GameStart());
            } else {
                float vert = Input.GetAxisRaw("Vertical");
                if(vert < 0.0f || vert > 0.0f) {
                    AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                    StartCoroutine(SlideCursor());
                } else {
                    float hori = Input.GetAxisRaw("Horizontal");
                    if(hori < 0.0f || hori > 0.0f) {
                        AudioManager.Instance.se_as.PlayOneShot(AudioManager.Instance.se_cursor);
                        if(hori < 0.0f) {
                            ShotChange(-1);
                        } else {
                            ShotChange(1);
                        }
                        primary.GetComponent<Text>().text = prim_shot.ToString();
                        secondary.GetComponent<Text>().text = seco_shot.ToString();

                        player.shotType_Primary = prim_shot;
                        player.shotType_Secondary = seco_shot;
                    }
                }
            }
        }

        if(setting) {
            if(demoCT >= 0.5f) {
                player.ShotBullets_Demo(select_prim);
            }
            demoCT += Time.deltaTime;
            if(demoCT >= 1.5f) {
                demoCT = 0.0f;
            }
        }
    }

    void ShotChange(int plus) {
        if(select_prim) {
            prim_shot += plus;
            if(prim_shot < ShotType.FourWay) {
                prim_shot = ShotType.Spread;
            } else if(prim_shot > ShotType.Spread) {
                prim_shot = ShotType.FourWay;
            }
        } else {
            seco_shot += plus;
            if(seco_shot < ShotType.FourWay) {
                seco_shot = ShotType.Spread;
            } else if(seco_shot > ShotType.Spread) {
                seco_shot = ShotType.FourWay;
            }
        }

        StartCoroutine(CoolTime());
    }

    // チェンジクールタイム
    private IEnumerator CoolTime() {
        animated = true;
        yield return new WaitForSeconds(0.2f);
        animated = false;
    }

    // カーソルスライド
    private IEnumerator SlideCursor() {
        animated = true;
        RectTransform rt = cursor.GetComponent<RectTransform>();

        select_prim = !select_prim;

        float time = 0.0f;
        float start_y = rt.anchoredPosition.y;
        float end_y;
        if(select_prim) end_y = primary.GetComponent<RectTransform>().anchoredPosition.y;
        else            end_y = secondary.GetComponent<RectTransform>().anchoredPosition.y;

        while(time <= 0.2f) {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, Easing.Ease_Out_Quad(start_y, end_y, time / 0.2f));
            time += Time.deltaTime;
            yield return null;
        }
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, end_y);
        yield return null;

        animated = false;
    }

    // ゲーム開始コルーチン
    private IEnumerator GameStart() {
        animated = true;
        setting = false;

        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("EndSetting");

        AudioManager.Instance.AudioFadeOut(AudioManager.Instance.bgm_as, 1.0f);
        yield return new WaitForSeconds(0.5f);

        player.StandbyPlayer();

        int count = 0;
        message.text = "";
        string message_str = "GET READY\n";
        while(message_str.Length > count) {
            message.text += message_str[count];
            count++;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.5f);

        count = 0;
        message_str = "GOOD LUCK!";
        while(message_str.Length > count) {
            message.text += message_str[count];
            count++;
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(1.0f);

        AudioManager.Instance.bgm_as.clip = AudioManager.Instance.bgm_stage;
        AudioManager.Instance.bgm_as.volume = 0.5f;
        AudioManager.Instance.bgm_as.Play();

        message.text = "";
        anim.SetTrigger("Open");
        yield return new WaitForSeconds(1.0f);
        GameController.Instance.uiCanvas.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
