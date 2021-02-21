using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_BossEx : Base_MovePatern {
    // パラメータ
    public MoveParameter_BossEx paramBEx;

    public MovePatern_BossEx(Base_MoveParameters parameter) : base(parameter) {
        paramBEx = parameter as MoveParameter_BossEx;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
    }

    protected override void Moving_In(float time) {
        enemy.transform.position = (paramBEx.firstBossPos - firstPos) * time / paramBEx.time_in + firstPos;
    }

    protected override void Moving_Always(float time) {
        if(time >= paramBEx.time_in) {
            float my = (Mathf.Cos(360.0f * ((time - param.time_in) / paramBEx.cycleTime) * Mathf.Deg2Rad) - 1.0f) * paramBEx.move_y;
            enemy.transform.position = paramBEx.firstBossPos + new Vector3(0.0f, my, 0.0f);
        }

        withdrawal = false;
    }
}
