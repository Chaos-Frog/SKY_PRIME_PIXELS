using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_Boss03")]
public class MoveParameter_Boss03 : Base_MoveParameters {
    [Space(20)]
    public Vector3 firstBossPos;                // 本体初期位置
    public Vector3[] bit_Pos = new Vector3[4];  // ビット 待機位置

    [Space(20)]
    public Vector2 moveVec2;                    // ボス移動範囲
    public float moveCycle;                     // ボス移動サイクル

    [Space(20)]
    public float bit_angleLimit;                // ビット回転角制限
    public float bit_moveY;                     // ビット上下移動
    public float bit_moveCycle;                 // ビット移動サイクル

    public MoveParameter_Boss03() : base(13) { }
}
