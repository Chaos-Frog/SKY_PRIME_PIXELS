using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_Boss01 : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameter_Boss01 param;

    private float time = 0.0f;

    public DanmakuPatern_Boss01(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameter_Boss01;
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

    private int bossCount_1 = 0;
    private int bossCount_2 = 0;

    // 本体攻撃
    private void BossShot() {
        switch(bossCount_1) {
            case 0:
                if(time >= param.boss_shotTime_1_0) {
                    Shot_1(3, 2);
                }
                break;

            case 1:
                if(time >= param.boss_shotTime_1_1) {
                    Shot_1(5, 4);
                }
                break;
        }

        switch(bossCount_2) {
            case 0:
                if(time >= param.boss_shotTime_2_0) {
                    Shot_2(param.boss_shotNum_2_0);
                }
                break;

            case 1:
                if(time >= param.boss_shotTime_2_1) {
                    Shot_2(param.boss_shotNum_2_1);
                }
                break;

            case 2:
                if(time >= param.boss_shotTime_2_2) {
                    Shot_2(param.boss_shotNum_2_2);
                }
                break;
        }
    }

    private int   shot1_Count    = 0;
    private float shot1_Interval = 0.0f;
    private float shot1_Angle    = 0.0f;

    // ショット1
    private void Shot_1(int count, int shot_min) {
        if(shot1_Count == 0) {
            shot1_Angle = LookPlayer();
        }

        if(shot1_Count < count) {
            if(shot1_Interval >= param.boss_shotInterval_1) {
                int num = shot_min + (shot1_Count % 2);
                NWayShot(num, shot1_Angle, param.boss_shotAngle_1, param.bulletPrefab_R, enemy.transform.position, param.boss_shotSpeed_1, param.boss_shotSize_1);
                shot1_Interval = 0.0f;
                shot1_Count++;

                if(shot1_Count >= count) {
                    shot1_Count = 0;
                    bossCount_1++;
                }
            }
            shot1_Interval += Time.deltaTime;
        }
    }

    // ショット2
    private void Shot_2(int num) {
        float between_a = 360.0f / num;
        NWayShot(num, 0.0f, between_a, param.bulletPrefab_B, enemy.transform.position, param.boss_shotSpeed_2, param.boss_shotSize_2);
        bossCount_2++;
    }

    private int bitcount = 0;

    // ビット攻撃
    private void BitShot() {
        switch(bitcount) {
            case 0:
                if(time >= param.bit_startTime) {
                    BitShot_1();
                }
                break;

            case 1:
                if(time >= param.bit_endStartTime) {
                    BitShot_2();
                }
                break;
        }
    }

    private float bit_time_1  = 0.0f;   // ビット時間
    private float bit_inter_1 = 0.0f;   // インターバルカウント
    private int   bit_count_1 = 0;      // ビットカウント

    private void BitShot_1() {
        if(bit_time_1 <= param.bit_time) {
            int active_count = 0;
            for(int i = 0; i < 4; i++) {
                if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                    active_count++;
                }
            }

            float interval = param.bit_interval * ((active_count + 4.0f) / 8.0f);
            if(bit_inter_1 >= interval) {
                Vector3 pos = Enemy_Boss.Instance.bitObjects[bit_count_1].transform.position;
                float angle = LookPlayer(pos);
                ShotBullet(param.bulletPrefab_B, pos, param.bit_speed, angle, param.bit_size, true);

                bit_count_1++;
                if(bit_count_1 >= 4) bit_count_1 = 0;

                bit_inter_1 -= interval;
            }

            if(!Enemy_Boss.Instance.bitObjects[bit_count_1].activeSelf) {
                bit_count_1++;
                if(bit_count_1 >= 4) bit_count_1 = 0;
            }

            bit_inter_1 += Time.deltaTime;
        } else {
            bitcount++;
        }

        bit_time_1 += Time.deltaTime;
    }

    private float bit_time_2  = 0.0f;  // ビット時間2
    private float bit_inter_2 = 0.0f;  // インターバルカウント2
    private int   bit_count_2 = 0;     // ビットカウント2

    private void BitShot_2() {
        if(bit_time_2 <= param.bit_endtime) {
            if(bit_inter_2 >= param.bit_endinterval) {
                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        Vector3 pos = Enemy_Boss.Instance.bitObjects[i].transform.position;
                        float angle = LookPlayer(pos);
                        float spd = param.bit_endspeed_min + param.bit_endspeed_inter * bit_count_2;
                        ShotBullet(param.bulletPrefab_R, pos, spd, angle, param.bit_endsize);
                    }
                }
                bit_inter_2 -= param.bit_endinterval;
                bit_count_2++;
            }
            bit_inter_2 += Time.deltaTime;
        } else {
            bitcount++;
        }

        bit_time_2 += Time.deltaTime;
    }
}
