using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Boss_1")]
public class DanmakuParameter_Boss01 : BaseDanmakuParameter {
    public GameObject bulletPrefab_R;  // 弾プレハブ
    public GameObject bulletPrefab_B;  // 弾プレハブ

    [Space(15)]
    public float boss_shotTime_1_0;
    public float boss_shotTime_1_1;
    [Space(5)]
    public float boss_shotSpeed_1;
    public float boss_shotSize_1;
    public float boss_shotAngle_1;
    public float boss_shotInterval_1;

    [Space(15)]
    public float boss_shotTime_2_0;
    public float boss_shotTime_2_1;
    public float boss_shotTime_2_2;
    [Space(5)]
    public int   boss_shotNum_2_0;
    public int   boss_shotNum_2_1;
    public int   boss_shotNum_2_2;
    public float boss_shotSpeed_2;
    public float boss_shotSize_2;

    [Space(15)]
    public float bit_startTime;
    public float bit_time;
    public float bit_speed;
    public float bit_size;
    public float bit_interval;
    [Space(5)]
    public float bit_endStartTime;
    public float bit_endtime;
    public float bit_endspeed_min;
    public float bit_endspeed_inter;
    public float bit_endsize;
    public float bit_endinterval;

    [Space(10)]
    public float all_endtime;

    public DanmakuParameter_Boss01() : base(11) { }
}
