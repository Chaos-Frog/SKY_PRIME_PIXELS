using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    public Text text_score;
    public Text text_bonus_rank;
    public Text text_bonus_hp;

    public GameObject credit;

    void Start() {
        if(GameController.Instance.life == 0) {
            StartCoroutine(GameOverCoru());
        } else {
            StartCoroutine(GameClearCoru());
        }
    }

    private IEnumerator GameOverCoru() {
        AudioManager.Instance.bgm_as.clip = AudioManager.Instance.jingle_over;
        AudioManager.Instance.bgm_as.loop = false;
        AudioManager.Instance.bgm_as.volume = 0.5f;
        AudioManager.Instance.bgm_as.Play();

        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Failed");
        yield return new WaitForSeconds(4.0f);

        credit.SetActive(true);
        Credit cre = credit.GetComponent<Credit>();
        yield return StartCoroutine(cre.Credit_Coroutine());

        Image img = GameController.Instance.fadeCanvas.transform.GetChild(0).GetComponent<Image>();

        float time = 0.0f;
        while(time <= 0.5f) {
            float t = time / 0.5f;
            img.color = new Color(img.color.r, img.color.g, img.color.b, t);
            time += Time.deltaTime;
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Title");
    }

    private IEnumerator GameClearCoru() {
        AudioManager.Instance.bgm_as.clip = AudioManager.Instance.jingle_clear;
        AudioManager.Instance.bgm_as.loop = false;
        AudioManager.Instance.bgm_as.volume = 0.5f;
        AudioManager.Instance.bgm_as.Play();

        Animator anim = this.GetComponent<Animator>();
        anim.SetTrigger("Clear");
        yield return new WaitForSeconds(3.5f);

        uint scorePoint = GameController.Instance.score;
        int gameRank = GameController.Instance.gameRank;
        int life = GameController.Instance.life;

        float time = 0.0f;
        while(time <= 1.0f) {
            text_score.text = ((int)(scorePoint * (time / 1.0f))).ToString();
            time += Time.deltaTime;
            yield return null;
        }
        text_score.text = scorePoint.ToString();
        yield return new WaitForSeconds(0.5f);

        time = 0.0f;
        while(time <= 1.0f) {
            text_bonus_rank.text = ((int)(gameRank * (time / 1.0f))).ToString() + " x500";
            time += Time.deltaTime;
            yield return null;
        }
        text_bonus_rank.text = gameRank.ToString() + " x500";
        yield return new WaitForSeconds(0.5f);

        time = 0.0f;
        while(time <= 1.0f) {
            text_bonus_hp.text = ((int)(life * (time / 1.0f))).ToString() + " x2000";
            time += Time.deltaTime;
            yield return null;
        }
        text_bonus_hp.text = life.ToString() + " x2000";
        yield return new WaitForSeconds(0.5f);

        time = 0.0f;
        uint total_life = (uint)(scorePoint + gameRank * 500 + life * 2000);
        while(time <= 1.0f) {
            text_score.text = ((uint)((total_life - scorePoint) * (time / 1.0f)) + scorePoint).ToString();
            time += Time.deltaTime;
            yield return null;
        }
        text_score.text = total_life.ToString();
        yield return new WaitForSeconds(2.0f);

        credit.SetActive(true);
        Credit cre = credit.GetComponent<Credit>();
        yield return StartCoroutine(cre.Credit_Coroutine());

        Image img = GameController.Instance.fadeCanvas.transform.GetChild(0).GetComponent<Image>();

        time = 0.0f;
        while(time <= 0.5f) {
            float t = time / 0.5f;
            img.color = new Color(img.color.r, img.color.g, img.color.b, t);
            time += Time.deltaTime;
            yield return null;
        }
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1.0f);
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("Title");
    }
}