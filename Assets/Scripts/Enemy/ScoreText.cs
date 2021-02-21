using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    [SerializeField] private float move_y;

    private Text textObj;
    private uint score;
    private float time;
    private float firstPos_Y;

    void Start() {
        textObj = this.GetComponent<Text>();
        textObj.text = "" + score;
        firstPos_Y = textObj.rectTransform.position.y;
    }

    void FixedUpdate() {
        float py = Easing.Ease_Out_Sine(firstPos_Y, firstPos_Y + move_y, time);
        textObj.rectTransform.position = new Vector2(textObj.rectTransform.position.x, py);
        time += Time.deltaTime;

        if(time >= 1.0f) {
            Destroy(this.gameObject);
        }
    }

    public void SetScore(uint sc) {
        score = sc;
    }
}
