using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_MoveParameters : ScriptableObject {
    // パターン番号
    [HideInInspector] public int paternNumber;

    public float time_in;    // 入場時間
    public float time_wait;  // 待機時間
    public float time_out;   // 退場時間

    public Base_MoveParameters(int pn) {
        paternNumber = pn;
    }
}