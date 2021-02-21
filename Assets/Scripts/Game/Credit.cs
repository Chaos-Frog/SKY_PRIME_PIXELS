using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour {
    private CanvasGroup credit_base;
    [SerializeField] private CanvasGroup credit_1;
    [SerializeField] private CanvasGroup credit_2;
    [SerializeField] private CanvasGroup credit_3;

    void Start() {
    }

    private IEnumerator CanvasFadeIn(CanvasGroup cg, float t) {
        float time = 0.0f;
        while(time <= t) {
            cg.alpha = time / t;
            time += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1.0f;
        yield return null;
    }

    private IEnumerator CanvasFadeOut(CanvasGroup cg, float t) {
        float time = 0.0f;
        while(time <= t) {
            cg.alpha = 1.0f - time / t;
            time += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0.0f;
        yield return null;
    }

    public IEnumerator Credit_Coroutine(bool thanks = true) {
        credit_base = this.GetComponent<CanvasGroup>();
        yield return StartCoroutine(CanvasFadeIn(credit_base, 0.5f));

        float time = 0.0f;
        while(time <= 4.0f) {
            if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) break;
            time += Time.deltaTime;
            yield return null;
        }

        var f_out = StartCoroutine(CanvasFadeOut(credit_1, 0.5f));
        var f_in = StartCoroutine(CanvasFadeIn(credit_2, 0.5f));
        yield return f_in;
        yield return f_out;

        time = 0.0f;
        while(time <= 4.0f) {
            if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) break;
            time += Time.deltaTime;
            yield return null;
        }

        if(thanks) {
            f_out = StartCoroutine(CanvasFadeOut(credit_2, 0.5f));
            f_in = StartCoroutine(CanvasFadeIn(credit_3, 0.5f));
            yield return f_in;
            yield return f_out;

            time = 0.0f;
            while(time <= 4.0f) {
                if(Input.GetKey(KeyCode.Return) || ControllSetting.GetKey("Shot")) break;
                time += Time.deltaTime;
                yield return null;
            }
        } else {
            yield return CanvasFadeOut(credit_2, 0.5f);
            yield return CanvasFadeOut(credit_base, 0.5f);
            credit_1.alpha = 1.0f;
            credit_2.alpha = 0.0f;
            credit_3.alpha = 0.0f;
            yield return null;
        }
    }
}
