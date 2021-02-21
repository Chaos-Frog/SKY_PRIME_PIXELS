using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_Boss02 : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameter_Boss02 param;

    private float time = 0.0f;

    public DanmakuPatern_Boss02(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameter_Boss02;
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

    // 本体攻撃
    private void BossShot() {
        switch(bossCount) {
            case 0:
                if(time >= param.bossshot_startTime) {
                    BossShot_1();
                }
                break;
        }
    }

    private float boss_shotTime = 0.0f;
    private float boss_shotInterval = 0.0f;

    // 本体攻撃1
    private void BossShot_1() {
        if(boss_shotTime <= param.bossshot_time) {
            if(boss_shotInterval >= param.bossshot_interval) {
                int num = 7;
                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        num--;
                    }
                }
                float angle = 360.0f / num;
                float base_a = angle / 2.0f + 90.0f;
                float t = boss_shotTime / param.bossshot_time;
                base_a += (Mathf.Cos(360.0f * param.bossshot_cycle * t * Mathf.Deg2Rad) - 1.0f) * 180.0f;
                NWayShot(num, base_a, angle, param.bulletPrefab_R, enemy.transform.position, param.bossshot_speed, param.bossshot_size);

                boss_shotInterval -= param.bossshot_interval;

                if(boss_shotTime + Time.deltaTime > param.bossshot_time) {
                    bossCount++;
                }
            }
            boss_shotInterval += Time.deltaTime;
        }
        boss_shotTime += Time.deltaTime;
    }

    private int bitcount = 0;

    // ビット攻撃
    private void BitShot() {
        switch(bitcount) {
            case 0:
                if(time >= param.bitshot_startTime_0) {
                    BitShot_1(3);
                }
                break;

            case 1:
                if(time >= param.bitshot_startTime_1) {
                    BitShot_1(5);
                }
                break;

            case 2:
                if(time >= param.bitshot_startTime_2) {
                    BitShot_1(7);
                }
                break;
        }
    }

    private int   bit_shotCount;
    private float bit_shotInterval;

    // ビットショット1
    private void BitShot_1(int count) {
        if(bit_shotCount < count) {
            if(bit_shotInterval >= param.bitshot_interval) {
                int num = 5;
                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        num--;
                    }
                }

                for(int i = 0; i < 4; i++) {
                    if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                        float sa = LookPlayer(enemy.transform.position);
                        Vector3 pos = Enemy_Boss.Instance.bitObjects[i].transform.position;
                        NWayShot(num, sa, param.bitshot_betweenAngle, param.bulletPrefab_B, pos, param.bitshot_speed, param.bitshot_size, true);
                    }
                }
                bit_shotCount++;
                bit_shotInterval -= param.bitshot_interval;

                if(bit_shotCount >= count) {
                    bit_shotCount = 0;
                    bit_shotInterval = 0.0f;
                    bitcount++;
                }
            }

            bit_shotInterval += Time.deltaTime;
        }
    }
}
