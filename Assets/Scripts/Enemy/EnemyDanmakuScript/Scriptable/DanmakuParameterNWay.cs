using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_NWay")]
public class DanmakuParameterNWay : BaseDanmakuParameter {
    // 発射角モード
    public enum TargetMode {
        None,             // 固定角度
        Enemy_Look,       // 敵角度依存
        LookPlayer,       // 自機狙い
        First_LookPlayer  // 自機狙い（バースト初弾固定）
    }

    public GameObject bulletPrefab;  // 弾プレハブ
    public float      bulletSpeed;   // 弾 速度
    public float      bulletSize;    // 弾 サイズ

    [Space(10)]
    public int        shotNum;       // 弾数
    public Vector3    shotPos;       // 発射相対座標
    public TargetMode targetMode;    // 狙い角度
    public float      baseAngle;     // 基準の角度（中心）
    public float      betweenAngle;  // 間の角度

    [Space(10)]
    public int     count_Burst = 1;  // バースト数
    public float   inter_Burst;      // バースト間隔
    [Space(10)]
    public int     count_Loop = 1;   // ループ数
    public float   inter_Loop;       // ループ間隔

    public DanmakuParameterNWay() : base(1) {}
}