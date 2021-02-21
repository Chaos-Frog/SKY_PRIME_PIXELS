using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_BossEx : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameter_BossEx param;

    private float time = 0.0f;

    public DanmakuPatern_BossEx(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameter_BossEx;
    }

    public override void Init(GameObject e) {
        base.Init(e);
    }

    public override void ShotDanmaku() {
        BossShot();

        time += Time.deltaTime;
    }

    // 本体攻撃
    private void BossShot() {
        if(time >= param.bossshot_startTime_1) {
            BossShot_1();
        }
        if(time >= param.bossshot_startTime_2) {
            BossShot_2();
        }
        if(time >= param.bossshot_startTime_3) {
            BossShot_3();
        }
    }

    private float interval_1 = 0.0f;

    // 本体攻撃1
    private void BossShot_1() {
        if(interval_1 <= 0.0f) {
            float rad = time * 360.0f * Mathf.Deg2Rad;
            float angle = Mathf.Sin(rad) * 90.0f + 180.0f;
            NWayShot(4,  angle, 90.0f, param.bulletPrefab_R, enemy.transform.position, param.bossshot_speed_1, param.bossshot_size_1);
            NWayShot(4, -angle, 90.0f, param.bulletPrefab_R, enemy.transform.position, param.bossshot_speed_1, param.bossshot_size_1);
            interval_1 += param.bossshot_interval_1;
        }
        interval_1 -= Time.deltaTime;
    }

    private float interval_2  = 0.0f;
    private float interval_2b = 0.0f;
    private int   count_2b    = 0;

    // 本体攻撃2
    private void BossShot_2() {
        if(interval_2 <= 0.0f) {
            if(interval_2b <= 0.0f) {
                float betw_a = 360.0f / param.bossshot_way_2;
                float base_a = LookPlayer();
                if(count_2b % 2 == 1) {
                    base_a -= betw_a / 2.0f;
                }
                NWayShot(param.bossshot_way_2, base_a, betw_a, param.bulletPrefab_B, enemy.transform.position, param.bossshot_speed_2, param.bossshot_size_2, true);

                count_2b++;
                interval_2b += param.bossshot_interval_2_burst;
                if(count_2b >= 5) {
                    interval_2  = param.bossshot_interval_2;
                    interval_2b = 0.0f;
                    count_2b    = 0;
                } 
            }
            interval_2b -= Time.deltaTime;
        }
        interval_2 -= Time.deltaTime;
    }

    private float interval_3 = 0.0f;
    private int   count_3    = 0;

    // 本体攻撃3
    private void BossShot_3() {
        if(interval_3 >= param.bossshot_interval_3) {
            GameObject e = UnityEngine.Object.Instantiate(param.missile);
            e.transform.position = enemy.transform.position;
            e.transform.rotation = Quaternion.Euler(0.0f, 0.0f, count_3 * -20.0f + 70.0f);
            Enemy es = e.GetComponent<Enemy>();
            es.SetEnemy(0, 0);

            count_3++;
            if(count_3 >= 8) count_3 = 0;
            interval_3 -= param.bossshot_interval_3;
        }
        interval_3 += Time.deltaTime;
    }
}
