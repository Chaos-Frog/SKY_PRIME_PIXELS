using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DanmakuPatern/Danmaku_Boss_Ex")]
public class DanmakuParameter_BossEx : BaseDanmakuParameter {
    public GameObject bulletPrefab_R;  // 弾プレハブ
    public GameObject bulletPrefab_B;  // 弾プレハブ
    public GameObject missile;         // ミサイルプレハブ

    [Space(10)]
    public float bossshot_startTime_1;
    [Space(5)]
    public float bossshot_speed_1;
    public float bossshot_size_1;
    public float bossshot_interval_1;

    [Space(10)]
    public float bossshot_startTime_2;
    [Space(5)]
    public int   bossshot_way_2;
    public float bossshot_speed_2;
    public float bossshot_size_2;
    public float bossshot_interval_2;
    public float bossshot_interval_2_burst;

    [Space(10)]
    public float bossshot_startTime_3;
    [Space(5)]
    public float bossshot_interval_3;

    public DanmakuParameter_BossEx() : base(14) { }
}
