using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Rotate_NWay")]
public class DanmakuParameterRotateNWay : BaseDanmakuParameter {
    // 初弾発射角モード
    public enum TargetMode {
        None,             // 固定角度
        Enemy_Look,       // 敵角度依存
        First_LookPlayer  // 初弾自機狙い
    }

    // 発射角回転モード
    public enum RotateMode {
        Per_Time,  // 指定時間経過後
        Per_Shot,  // 発射時
    }

    public GameObject bulletPrefab;  // 弾プレハブ
    public float      bulletSpeed;   // 弾 速度
    public float      bulletSize;    // 弾 サイズ

    [Space(10)]
    public int        shotNum;       // 弾数
    public Vector3    shotPos;       // 発射相対座標
    public TargetMode targetMode;    // 初弾発射角モード
    public float      firstAngle;    // 基準の角度（中心）
    public float      betweenAngle;  // 間の角度
    public RotateMode rotateMode;    // 発射角回転モード
    public float      rotateAngle;   // 回転角度
    public float      rotateTime;    // 回転時間

    [Space(10)]
    public int     count_Burst = 1;  // バースト数
    public float   inter_Burst;      // バースト間隔
    [Space(10)]
    public int     count_Loop = 1;   // ループ数
    public float   inter_Loop;       // ループ間隔
    public bool    resetRotate;      // 回転リセット

    public DanmakuParameterRotateNWay() : base(2) {}
}