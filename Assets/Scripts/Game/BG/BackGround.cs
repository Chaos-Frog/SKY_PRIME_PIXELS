using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
    public static BackGround Instance;

    [SerializeField] private GameObject cloud;        // 雲プレハブ
    public float scrollSpeed;                         // スクロール速度

    [Space(10)]
    [SerializeField] private float spawnInter_front;  // 前面 雲生成間隔
    [SerializeField] private float spawnInter_back;   // 後面 雲生成間隔
    private float spawnTime_front;                    // 前面 雲生成時間
    private float spawnTime_back;                     // 後面 雲生成時間

    private int before_front = 0;
    private int before_back = 0;

    void Start() {
        Instance = this;
    }

    void FixedUpdate() {
        spawnTime_front += Time.deltaTime * (scrollSpeed / 2);
        if(spawnTime_front >= spawnInter_front) {
            InstantiateCloud(true);
            spawnTime_front = 0.0f;
        }

        spawnTime_back += Time.deltaTime * (scrollSpeed / 2);
        if(spawnTime_back >= spawnInter_back) {
            InstantiateCloud(false);
            spawnTime_back = 0.0f;
        }
    }

    private void InstantiateCloud(bool front) {
        GameObject cloudObj = Instantiate(cloud);
        Cloud cloudScr = cloudObj.GetComponent<Cloud>();
        cloudScr.front = front;

        float xs, ys;
        if(front) {
            xs = Random.Range(1.8f, 2.4f);
            ys = Random.Range(1.8f, 2.4f);
        } else {
            xs = Random.Range(1.0f, 1.5f);
            ys = Random.Range(1.0f, 1.5f);
        }
        cloudObj.transform.localScale = new Vector3(xs, ys, 1.0f);

        float xp, yp;
        if(front) {
            if(before_front == 0) {
                xp = Random.Range(2.0f, 6.0f);
                before_front = 1;
            } else {
                xp = Random.Range(-6.0f, -2.0f);
                before_front = 0;
            }
        } else {
            int num;
            do {
                num = Random.Range(0, 3) - 1;
            } while(num == before_back);
            before_back = num;
            xp = Random.Range(-2.0f, 2.0f) + 4.0f * num;
        }
        yp = 10.0f + Random.Range(1.0f, 2.0f) * ys;
        cloudObj.transform.position = new Vector3(xp, yp, 0.0f);
    }
}
