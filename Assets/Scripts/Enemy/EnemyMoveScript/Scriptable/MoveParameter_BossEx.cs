using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_BossEx")]
public class MoveParameter_BossEx : Base_MoveParameters {
    [Space(20)]
    public Vector3 firstBossPos;  // 本体初期位置

    [Space(20)]
    public float move_y;          // 上下の揺れ幅
    public float cycleTime;       // 時間周期

    public MoveParameter_BossEx() : base(14) { }
}
