using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_Boss02 : Base_MovePatern {
    // パラメータ
    public MoveParameter_Boss02 paramB02;

    private Vector3[] firstBitPos  = new Vector3[4];  // ビット初期位置
    private int       activeBitNum = 0;               // アクティブビット数
    private bool[]    activeBit    = new bool[4];     // アクティブビット

    public MovePatern_Boss02(Base_MoveParameters parameter) : base(parameter) {
        paramB02 = parameter as MoveParameter_Boss02;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
        for(int i = 0; i < 4; i++) {
            firstBitPos[i] = Enemy_Boss.Instance.bitObjects[i].transform.position;
            if(Enemy_Boss.Instance.bitObjects[i].activeSelf) {
                activeBit[i] = true;
                activeBitNum++;
            } else {
                activeBit[i] = false;
            }
        }
    }

    protected override void Moving_In(float time) {
        int count = 0;
        for(int i = 0; i < 4; i++) {
            if(activeBit[i]) {
                GameObject bit = Enemy_Boss.Instance.bitObjects[i];

                float angle = -90.0f + (360.0f / activeBitNum * count);
                float xp = Mathf.Cos(angle * Mathf.Deg2Rad) * paramB02.radius;
                float yp = Mathf.Sin(angle * Mathf.Deg2Rad) * paramB02.radius;
                Vector3 pos = Easing.Ease_Out_Quad(firstBitPos[i], new Vector3(xp, yp, 0.0f) + paramB02.firstBossPos, time / paramB02.time_in);
                bit.transform.position = pos;

                Quaternion quat = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, quat, 1);

                count++;
            }
        }
        enemy.transform.position = (paramB02.firstBossPos - firstPos) * time / paramB02.time_in + firstPos;
    }

    protected override void Moving_Wait(float time) {
        int count = 0;
        for(int i = 0; i < 4; i++) {
            if(activeBit[i]) {
                GameObject bit = Enemy_Boss.Instance.bitObjects[i];

                float angle = -90.0f + (360.0f / activeBitNum * count);
                angle += Easing.Ease_InOut_Quad(0.0f, paramB02.bit_rotate, time / paramB02.time_wait);
                float xp = Mathf.Cos(angle * Mathf.Deg2Rad) * paramB02.radius;
                float yp = Mathf.Sin(angle * Mathf.Deg2Rad) * paramB02.radius;
                Vector3 pos = new Vector3(xp, yp, 0.0f) + enemy.transform.position;
                bit.transform.position = pos;

                Quaternion quat = LookPlayerQuat(enemy.transform.position);
                bit.transform.rotation = Quaternion.RotateTowards(bit.transform.rotation, quat, 4);

                count++;
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
        if(time >= paramB02.time_in) {
            float my = (Mathf.Cos(360.0f * ((time - param.time_in) / paramB02.cycleTime) * Mathf.Deg2Rad) - 1.0f) * paramB02.move_y;
            enemy.transform.position = paramB02.firstBossPos + new Vector3(0.0f, my, 0.0f);
        }
    }
}
