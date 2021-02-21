using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_Boss01 : Base_MovePatern {
    // パラメータ
    public MoveParameter_Boss01 paramB01;

    private Vector3[] firstBitPos = new Vector3[4];  // ビット初期位置

    public MovePatern_Boss01(Base_MoveParameters parameter) : base(parameter) {
        paramB01 = parameter as MoveParameter_Boss01;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
        for(int i = 0; i < 4; i++) {
            firstBitPos[i] = Enemy_Boss.Instance.bitObjects[i].transform.position;
        }
    }

    protected override void Moving_In(float time) {
        float t = time / paramB01.time_in;
        for(int i = 0; i < 4; i++) {
            GameObject bit = Enemy_Boss.Instance.bitObjects[i];
            if(bit.activeSelf) {
                Vector3 pos = Easing.Ease_Out_Quad(firstBitPos[i], paramB01.bit_Pos[i], t);
                bit.transform.position = pos;

                Quaternion angle = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, angle, 4);
            }

            enemy.transform.position = (paramB01.firstBossPos - firstPos) * time / paramB01.time_in + firstPos;
        }
    }

    protected override void Moving_Wait(float time) {
        for(int i = 0; i < 4; i++) {
            GameObject bit = Enemy_Boss.Instance.bitObjects[i];
            if(bit.activeSelf) {
                Quaternion angle = LookPlayerQuat(bit.transform.position);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, angle, 4);
            }
        }
    }

    protected override void Moving_Out(float time) {
        for(int i = 0; i < 4; i++) {
            GameObject bit = Enemy_Boss.Instance.bitObjects[i];
            if(bit.activeSelf) {
                Quaternion angle = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, angle, 1);
            }
        }
    }

    protected override void Moving_Always(float time) {
        if(time >= paramB01.time_in) {
            float my = (Mathf.Cos(360.0f * ((time - param.time_in) / paramB01.cycleTime) * Mathf.Deg2Rad) - 1.0f) * paramB01.move_y;
            enemy.transform.position = paramB01.firstBossPos + new Vector3(0.0f, my, 0.0f);
        }
    }
}
