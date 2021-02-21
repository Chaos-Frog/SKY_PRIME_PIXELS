using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Boss_2")]
public class DanmakuParameter_Boss02 : BaseDanmakuParameter {
    public GameObject bulletPrefab_R;  // 弾プレハブ
    public GameObject bulletPrefab_B;  // 弾プレハブ

    [Space(10)]
    public float bossshot_startTime;
    [Space(5)]
    public float bossshot_time;
    public float bossshot_speed;
    public float bossshot_size;
    public float bossshot_interval;
    public float bossshot_cycle;

    [Space(10)]
    public float bitshot_startTime_0;
    public float bitshot_startTime_1;
    public float bitshot_startTime_2;
    [Space(5)]
    public float bitshot_speed;
    public float bitshot_size;
    public float bitshot_interval;
    public float bitshot_betweenAngle;

    [Space(10)]
    public float all_endtime;

    public DanmakuParameter_Boss02() : base(12) { }
}
