using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {
    public bool front;  // 正面側

    void FixedUpdate() {
        float scroll_y;
        if(front) {
            scroll_y = -BackGround.Instance.scrollSpeed * 2.0f;
        } else {
            scroll_y = -BackGround.Instance.scrollSpeed;
        }

        transform.position += new Vector3(0.0f, scroll_y, 0.0f) * Time.deltaTime;

        if(transform.position.y < -transform.localScale.y * 1.0f - 10.0f) {
            Destroy(this.gameObject);
        }
    }
}
