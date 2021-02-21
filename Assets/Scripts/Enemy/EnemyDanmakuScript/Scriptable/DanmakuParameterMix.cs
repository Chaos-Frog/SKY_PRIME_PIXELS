using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Mix")]
public class DanmakuParameterMix : BaseDanmakuParameter {
    public BaseDanmakuParameter[] danmaku;  // 弾幕配列


    public DanmakuParameterMix() : base(4) { }
}