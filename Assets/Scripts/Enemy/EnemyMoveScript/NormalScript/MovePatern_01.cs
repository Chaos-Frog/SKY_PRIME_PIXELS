using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_01 : Base_MovePatern {
    public MoveParameter01 param01;

    public MovePatern_01(Base_MoveParameters parameter) : base(parameter) {
        param01 = parameter as MoveParameter01;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
    }

    protected override void Moving_In(float time) {
        float t = time / param.time_in;
        Vector3 endPos = firstPos + new Vector3(0.0f, -param01.move_y, 0.0f);
        enemy.transform.position = Easing.Ease_Out_Quad(firstPos, endPos, t);
    }

    protected override void Moving_Wait(float time) {
        firstPos = enemy.transform.position;
    }

    protected override void Moving_Out(float time) {
        float mt = time / param.time_out;
        Vector3 endPos = firstPos + new Vector3(0.0f, param01.move_y + 2.0f, 0.0f);
        enemy.transform.position = Easing.Ease_In_Quad(firstPos, endPos, mt);

        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), 4.0f);
    }
}
