using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour {
    [SerializeField] private GameObject hexaObj;
    [SerializeField] private GameObject text;

    void Start() {
        StartCoroutine(WarningAnimation());
    }

    private void InstantiateHexa(int hori, int vert) {
        float yp = vert * 90.0f;
        if(vert >= 0) {
            yp += 110.0f;
        } else {
            yp -= 110.0f;
        }
        float xp;
        if(vert % 2 == 0) {
            xp = hori * 310.0f;
            if(hori >= 0) {
                xp -= 155.0f;
            } else {
                xp += 155.0f;
            }
        } else {
            xp = hori * 310.0f;
        }
        Vector2 pos = new Vector2(xp, yp);
        GameObject hexa = Instantiate(hexaObj);
        hexa.transform.SetParent(this.transform, false);
        hexa.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    private IEnumerator WarningAnimation() {
        InstantiateHexa( 0,  1);
        InstantiateHexa( 0, -1);
        yield return new WaitForSeconds(0.15f);

        InstantiateHexa( 1,  2);
        InstantiateHexa(-1,  2);
        InstantiateHexa( 1, -2);
        InstantiateHexa(-1, -2);
        yield return new WaitForSeconds(0.15f);

        InstantiateHexa( 1,  1);
        InstantiateHexa(-1,  1);
        InstantiateHexa( 0,  3);
        InstantiateHexa( 1, -1);
        InstantiateHexa(-1, -1);
        InstantiateHexa( 0, -3);
        yield return new WaitForSeconds(0.15f);

        InstantiateHexa( 1,  3);
        InstantiateHexa(-1,  3);
        InstantiateHexa( 1,  4);
        InstantiateHexa(-1,  4);
        InstantiateHexa( 1, -3);
        InstantiateHexa(-1, -3);
        InstantiateHexa( 1, -4);
        InstantiateHexa(-1, -4);

        yield return new WaitForSeconds(3.00f);

        yield return new WaitForSeconds(3.00f);

        this.gameObject.SetActive(false);
    }
}
