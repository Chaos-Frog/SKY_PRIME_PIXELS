using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_Rotate_NWay : Base_DanmakuPatern {
    // パラメータ
    private DanmakuParameterRotateNWay param;

    private float first_Angle;     // 初弾発射角度
    private float rotate_Angle;    // 回転角度

    private float time = 0.0f;     // 経過時間

    private int   burst_c    = 0;  // バーストカウント
    private float burst_time = 0;  // バーストタイムカウント
    private int   loop_c     = 0;  // ループカウント
    private float loop_time  = 0;  // ループタイムカウント

    public DanmakuPatern_Rotate_NWay(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameterRotateNWay;
    }

    public override void Init(GameObject e) {
        base.Init(e);

        first_Angle = param.firstAngle;
        switch(param.targetMode) {
            case DanmakuParameterRotateNWay.TargetMode.First_LookPlayer:
                first_Angle += LookPlayer();
                break;
            case DanmakuParameterRotateNWay.TargetMode.Enemy_Look:
                first_Angle += enemy.transform.rotation.eulerAngles.z;
                break;
        }

        rotate_Angle = 0.0f;
    }

    public override void ShotDanmaku() {
        if(param.rotateMode == DanmakuParameterRotateNWay.RotateMode.Per_Time) {
            if(time >= param.rotateTime) {
                rotate_Angle += param.rotateAngle;
                rotate_Angle %= 360.0f;
                time = 0.0f;
            } else {
                time += Time.deltaTime;
            }
        }
        Loop_Task();
    }

    // NWayショット処理
    private void NWay_Task() {
        float angle = first_Angle + rotate_Angle;

        Vector3 shotPos = enemy.transform.position + param.shotPos;
        NWayShot(param.shotNum, angle, param.betweenAngle, param.bulletPrefab, shotPos, param.bulletSpeed, param.bulletSize, true);

        if(param.rotateMode == DanmakuParameterRotateNWay.RotateMode.Per_Shot) {
            rotate_Angle += param.rotateAngle;
            rotate_Angle %= 360.0f;
        }
    }

    // バースト処理
    private bool Burst_Task() {
        if(burst_c < param.count_Burst) {
            if(burst_c == 0 || burst_time >= param.inter_Burst) {
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
                if(param.resetRotate) {
                    rotate_Angle = 0.0f;
                    time = 0.0f;
                }
            }
        } else {
            end = true;
        }
    }
}
