using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDanmakuParameter : ScriptableObject {
    // パターン番号
    [HideInInspector] public int paternNumber;

    public float time_start;         // 開始時間

    public BaseDanmakuParameter(int num) {
        paternNumber = num;
    }
}