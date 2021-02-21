using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rect : MonoBehaviour {
    private float base_size;  // 基準サイズ
    private float size;       // 現サイズ

    private float speed;

    void Start() {
        base_size = Random.Range(0.5f, 1.5f);
        size = base_size;
        this.transform.localScale = new Vector2(base_size, base_size);

        speed = Random.Range(0.2f, 0.6f);

        this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, Random.Range(0.2f, 0.4f));
    }

    void Update() {
        this.transform.position += new Vector3(0, speed * Time.deltaTime, 0);

        float size_tmp = size;
        size = base_size * (1.0f + TitleManager.Instance.volume * 5.0f);
        if(size_tmp > size) {
            size = size_tmp - 0.02f;
            if(size < base_size) size = base_size;
        }
        this.transform.localScale = new Vector2(size, size);

        if(transform.position.y >= 8.0f) Destroy(this.gameObject);
    }
}
