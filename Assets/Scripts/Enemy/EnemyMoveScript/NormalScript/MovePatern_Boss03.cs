using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_Boss03 : Base_MovePatern {
    // パラメータ
    public MoveParameter_Boss03 paramB03;

    private Vector3[] firstBitPos = new Vector3[4];  // ビット初期位置

    public MovePatern_Boss03(Base_MoveParameters parameter) : base(parameter) {
        paramB03 = parameter as MoveParameter_Boss03;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
        for(int i = 0; i < 4; i++) {
            firstBitPos[i] = Enemy_Boss.Instance.bitObjects[i].transform.position;
        }
    }

    protected override void Moving_In(float time) {
        float t = time / paramB03.time_in;
        for(int i = 0; i < 4; i++) {
            GameObject bit = Enemy_Boss.Instance.bitObjects[i];
            if(bit.activeSelf) {
                Vector3 pos = Easing.Ease_Out_Quad(firstBitPos[i], paramB03.bit_Pos[i], t);
                bit.transform.position = pos;
            }

            enemy.transform.position = (paramB03.firstBossPos - firstPos) * time / paramB03.time_in + firstPos;
        }
    }

    protected override void Moving_Wait(float time) {
        float t = Easing.Ease_InOut_Quad(0.0f, 1.0f, time / paramB03.time_wait);
        float rad = 360.0f * paramB03.moveCycle * t * Mathf.Deg2Rad;
        float px = Mathf.Sin(rad) * paramB03.moveVec2.x;
        float py = Mathf.Sin(rad * 2) * paramB03.moveVec2.y;
        enemy.transform.position = new Vector3(px, py, 0.0f) + paramB03.firstBossPos;
    }

    protected override void Moving_Out(float time) {
    }

    protected override void Moving_Always(float time) {
        float limit;
        if(time <= paramB03.time_in) {
            limit = 2.0f;
        } else {
            limit = paramB03.bit_angleLimit;
        }

        for(int i = 0; i < 4; i++) {
            GameObject bit = Enemy_Boss.Instance.bitObjects[i];
            if(bit.activeSelf) {
                if(time >= param.time_in) {
                    float my = (Mathf.Cos(360.0f * ((time - param.time_in) / paramB03.bit_moveCycle) * Mathf.Deg2Rad) - 1.0f) * paramB03.bit_moveY;
                    bit.transform.position = paramB03.bit_Pos[i] + new Vector3(0.0f, my, 0.0f);
                }

                Quaternion angle = LookPlayerQuat(bit.transform.position);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, angle, limit);
            }
        }
    }
}
