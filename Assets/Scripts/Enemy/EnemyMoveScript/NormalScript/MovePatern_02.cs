using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_02 : Base_MovePatern {
    public MoveParameter02 param02;

    public MovePatern_02(Base_MoveParameters parameter) : base(parameter) {
        param02 = parameter as MoveParameter02;
    }

    protected override void Moving_In(float time) {
        float t = time / param.time_in;
        Vector3 end = firstPos + new Vector3(0.0f, -param02.move_y, 0.0f);
        enemy.transform.position = Easing.Ease_Out_Quad(firstPos, end, t);
        LookPlayer(param02.rotateLimit);
    }

    protected override void Moving_Wait(float time) {
        firstPos = enemy.transform.position;
        LookPlayer(param02.rotateLimit);
    }

    protected override void Moving_Out(float time) {
        float mt = time / param.time_out;
        Vector3 end = firstPos + new Vector3(0.0f, param02.move_y + 2, 0.0f);
        enemy.transform.position = Easing.Ease_In_Quad(firstPos, end, mt);

        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.Euler(0.0f, 0.0f, 0.0f), 4.0f);
    }
}
