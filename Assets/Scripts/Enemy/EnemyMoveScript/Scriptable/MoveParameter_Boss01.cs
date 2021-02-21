using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_Boss01")]
public class MoveParameter_Boss01 : Base_MoveParameters {
    [Space(20)]
    public Vector3 firstBossPos;                // 本体初期位置
    public Vector3[] bit_Pos = new Vector3[4];  // ビット 待機位置

    [Space(20)]
    public float move_y;     // 上下の揺れ幅
    public float cycleTime;  // 時間周期

    public MoveParameter_Boss01() : base(11) { }
}
