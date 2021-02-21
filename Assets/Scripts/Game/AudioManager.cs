using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    static public AudioManager Instance;

    public AudioSource bgm_as;
    public AudioSource se_as;

    [Space(10)]
    public GameObject audioPlayer;

    [Space(10)]
    public AudioClip bgm_standby;
    public AudioClip bgm_stage;
    public AudioClip bgm_boss;

    [Space(10)]
    public AudioClip jingle_clear;
    public AudioClip jingle_over;

    [Space(10)]
    public AudioClip se_shot;
    public AudioClip se_break;

    public AudioClip se_exp1;
    public AudioClip se_exp2;
    public AudioClip se_exp3;
    public AudioClip se_exp4;
    public AudioClip se_expL;

    public AudioClip se_warning;

    public AudioClip se_select;
    public AudioClip se_cancel;
    public AudioClip se_cursor;

    void Start() {
        Instance = this;
    }

    public void AudioFadeIn(AudioSource audio, float time) {
        StartCoroutine(Audio_FadeIn(audio, time));
    }

    public void AudioFadeOut(AudioSource audio, float time) {
        StartCoroutine(Audio_FadeOut(audio, time));
    }

    public void PlayOneTimeAudio(AudioClip clip) {
        GameObject obj = Instantiate(audioPlayer);
        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("Audio");
        if(objects.Length > 5) {
            Destroy(objects[0]);
        }
    }

    private IEnumerator Audio_FadeIn(AudioSource audio, float t) {
        float time = 0.0f;
        while(time <= t) {
            audio.volume = time / t * 0.5f;
            time += Time.deltaTime;
            yield return null;
        }
        audio.volume = 0.5f;
    }

    private IEnumerator Audio_FadeOut(AudioSource audio, float t) {
        float time = 0.0f;
        while(time <= t) {
            audio.volume = (1.0f - time / t) * 0.5f;
            time += Time.deltaTime;
            yield return null;
        }
        audio.volume = 0.0f;
        audio.Stop();
    }
}
