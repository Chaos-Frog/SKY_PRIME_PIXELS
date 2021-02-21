using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_NWay : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameterNWay param;

    private float burst_Angle = 0;  // バーストショット角度
    private int   burst_c     = 0;  // バーストカウント
    private float burst_time  = 0;  // バーストタイムカウント
    private int   loop_c      = 0;  // ループカウント
    private float loop_time   = 0;  // ループタイムカウント

    public DanmakuPatern_NWay(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameterNWay;
    }

    public override void Init(GameObject e) {
        base.Init(e);
    }

    public override void ShotDanmaku() {
        Loop_Task();
    }

    // NWayショット処理
    private void NWay_Task() {
        float baseAngle = param.baseAngle;
        switch(param.targetMode) {
            case DanmakuParameterNWay.TargetMode.LookPlayer:
                baseAngle += LookPlayer();
                break;
            case DanmakuParameterNWay.TargetMode.Enemy_Look:
                baseAngle = enemy.transform.rotation.eulerAngles.z;
                break;
            case DanmakuParameterNWay.TargetMode.First_LookPlayer:
                baseAngle = burst_Angle;
                break;
        }

        Vector3 shotPos = enemy.transform.position + param.shotPos;
        NWayShot(param.shotNum, baseAngle, param.betweenAngle, param.bulletPrefab, shotPos, param.bulletSpeed, param.bulletSize, true);
    }

    // バースト処理
    private bool Burst_Task() {
        if(burst_c < param.count_Burst) {
            if(burst_c == 0 || burst_time >= param.inter_Burst) {
                if(burst_c == 0 && param.targetMode == DanmakuParameterNWay.TargetMode.First_LookPlayer) {
                    burst_Angle = param.baseAngle + LookPlayer();
                }
                NWay_Task();
                burst_c++;
                burst_time = 0.0f;
            } else {
                burst_time += Time.deltaTime;
            }
            return false;
        } else {
            burst_c = 0;
            return true;
        }
    }

    // ループ処理
    private void Loop_Task() {
        if(param.count_Loop == 0 || loop_c < param.count_Loop) {
            if(loop_c == 0 || loop_time >= param.inter_Loop) {
                if(Burst_Task()) {
                    loop_c++;
                    loop_time = 0.0f;
                }
            } else {
                loop_time += Time.deltaTime;
            }
        } else {
            end = true;
        }
    }
}
