using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Boss_3")]
public class DanmakuParameter_Boss03 : BaseDanmakuParameter {
    public GameObject bulletPrefab_R;  // 弾プレハブ
    public GameObject bulletPrefab_B;  // 弾プレハブ

    [Space(10)]
    public float bossshot_startTime;
    [Space(5)]
    public float bossshot_time;
    public float bossshot_speed;
    public float bossshot_size;
    public float bossshot_interval_max;
    public float bossshot_interval_randomDif;

    [Space(10)]
    public float bossshot_startTime_2_0;
    public float bossshot_startTime_2_1;
    public float bossshot_startTime_2_2;
    [Space(5)]
    public int   bossshot_way_2_0;
    public int   bossshot_way_2_1;
    public int   bossshot_way_2_2;
    [Space(5)]
    public float bossshot_speed_2max;
    public float bossshot_speed_2min;
    public float bossshot_size_2max;
    public float bossshot_size_2min;

    [Space(10)]
    public float bitshot_startTime;
    [Space(5)]
    public float bitshot_time;
    public float bitshot_speed;
    public float bitshot_size;
    public float bitshot_interval;
    public float bitshot_betweenAngle;

    [Space(10)]
    public float all_endtime;

    public DanmakuParameter_Boss03() : base(13) { }
}
