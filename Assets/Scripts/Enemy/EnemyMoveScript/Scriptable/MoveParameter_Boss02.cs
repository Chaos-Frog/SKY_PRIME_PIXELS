using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_Boss02")]
public class MoveParameter_Boss02 : Base_MoveParameters {
    [Space(20)]
    public Vector3 firstBossPos;  // 本体初期位置
    public float radius;          // ビット位置 半径
    public float bit_rotate;      // ビット回転角度（総合）

    [Space(20)]
    public float move_y;          // 上下の揺れ幅
    public float cycleTime;       // 時間周期

    public MoveParameter_Boss02() : base(12) { }
}
