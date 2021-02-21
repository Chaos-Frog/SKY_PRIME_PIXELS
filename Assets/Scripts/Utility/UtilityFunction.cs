using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class UtilityFunction {
    // 2点間の角度を求める（ラジアン）
    static public float GetToAngle(Vector3 self, Vector3 target) {
        Vector3 dif = target - self;
        return Mathf.Atan2(dif.y, dif.x);
    }
}
