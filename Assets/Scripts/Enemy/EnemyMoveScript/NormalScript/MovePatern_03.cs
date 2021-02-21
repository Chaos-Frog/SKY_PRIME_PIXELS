using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_03 : Base_MovePatern {
    public MoveParameter03 param03;

    public MovePatern_03(Base_MoveParameters parameter) : base(parameter) {
        param03 = parameter as MoveParameter03;
    }

    protected override void Moving_Wait(float time) {
        float t = time / param.time_wait;
        float toAngle;
        if(param03.turnLeft) toAngle = 180.0f - param03.turnAngle;
        else                 toAngle = 180.0f + param03.turnAngle;

        float zAngle = Mathf.Lerp(180.0f, toAngle, Mathf.Clamp01(t));
        enemy.transform.rotation = Quaternion.Euler(0, 0, zAngle);
    }

    protected override void Moving_Always(float time) {
        Vector3 velocity = enemy.transform.rotation * new Vector3(0, param03.move_speed, 0);
        enemy.transform.position += velocity * Time.deltaTime;
    }
}
