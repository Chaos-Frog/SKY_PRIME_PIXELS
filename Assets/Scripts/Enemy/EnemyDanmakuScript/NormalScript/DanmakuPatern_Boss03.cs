using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_Boss03 : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameter_Boss03 param;

    private float time = 0.0f;

    public DanmakuPatern_Boss03(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameter_Boss03;
    }

    public override void Init(GameObject e) {
        base.Init(e);
    }

    public override void ShotDanmaku() {
        BossShot();
        BitShot();

        if(time >= param.all_endtime) {
            end = true;
        }

        time += Time.deltaTime;
    }

    private int bossCount = 0;
    private int bossCount_2 = 0;

    // 本体攻撃
    private void BossShot() {
        switch(bossCount) {
            case 0:
                if(time >= param.bossshot_startTime) {
                    BossShot_1();
                }
                break;
        }

        switch(bossCount_2) {
            case 0:
                if(time >= param.bossshot_startTime_2_0) {
                    BossShot_2(param.bossshot_way_2_0);
                }
                break;
            case 1:
                if(time >= param.bossshot_startTime_2_1) {
                    BossShot_2(param.bossshot_way_2_1);
                }
                break;
            case 2:
                if(time >= param.bossshot_startTime_2_2) {
                    BossShot_2(param.bossshot_way_2_2);
                }
                break;
        }
    }

    private float boss_shotTime_1 = 0.0f;
    private float boss_shotInterval_1 = 0.0f;

    // 本体攻撃1
    private void BossShot_1() {
        if(boss_shotTime_1 <= param.bossshot_time) {
            if(boss_shotInterval_1 >= param.bossshot_interval_max) {
                Vector3 pos_L = new Vector3(-1.8f, 0.0f, 0.0f) + enemy.transform.position;
                Vector3 pos_R = new Vector3( 1.8f, 0.0f, 0.0f) + enemy.transform.position;
                ShotBullet(param.bulletPrefab_R, pos_L, param.bossshot_speed, 180.0f, param.bossshot_size, true);
                ShotBullet(param.bulletPrefab_R, pos_R, param.bossshot_speed, 180.0f, param.bossshot_size, true);
                boss_shotInterval_1 = Random.Range(0.0f, param.bossshot_interval_randomDif);
            }
            boss_shotInterval_1 += Time.deltaTime;
        }
        boss_shotTime_1 += Time.deltaTime;
    }


    // 本体攻撃1
    private void BossShot_2(int way) {
        for(int i = 0; i < way; i++) {
            float ba = 360.0f / way;
            float angle = (ba * i) + Random.Range(-ba / 2.0f, ba / 2.0f);
            float speed = Random.Range(param.bossshot_speed_2min, param.bossshot_speed_2max);
            float size = Random.Range(param.bossshot_size_2min, param.bossshot_size_2max);
            ShotBullet(param.bulletPrefab_B, enemy.transform.position, speed, angle, size);
        }
        bossCount_2++;
    }


    private int bitcount = 0;

    // ビット攻撃
    private void BitShot() {
        switch(bitcount) {
            case 0:
                if(time >= param.bitshot_startTime) {
                    BitShot_1();
                }
                break;
        }
    }

    private float bit_shotTime = 0.0f;
    private float bit_shotInterval = 0.0f;

    // ビットショット1
    private void BitShot_1() {
        if(bit_shotTime <= param.bitshot_time) {
            if(bit_shotInterval >= param.bitshot_interval) {
                int num = 0;
                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        num++;
                    }
                }

                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        GameObject bit = Enemy_Boss.Instance.bitObjects[i];
                        int shotNum = 2 * (5 - num);
                        float base_a = bit.transform.rotation.eulerAngles.z;
                        float betw_a = param.bitshot_betweenAngle * ((float)(num + 4.0f) / 8.0f);
                        Vector3 spos = bit.transform.position;
                        NWayShot(shotNum, base_a, betw_a, param.bulletPrefab_B, spos, param.bitshot_speed, param.bitshot_size);
                    }
                }

                bit_shotInterval = 0.0f;
            }
            bit_shotInterval += Time.deltaTime;
        } else {
            bitcount++;
        }
        bit_shotTime += Time.deltaTime;
    }
}
