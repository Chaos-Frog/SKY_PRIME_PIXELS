using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_M01 : Base_MovePatern {
    public MoveParameter_M01 paramM01;

    private float speed;

    public MovePatern_M01(Base_MoveParameters parameter) : base(parameter) {
        paramM01 = parameter as MoveParameter_M01;
        speed = paramM01.moveSpeed;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
    }

    protected override void Moving_In(float time) {
        speed = Easing.Ease_In_Quad(paramM01.moveSpeed_Fast, paramM01.moveSpeed, time / paramM01.time_in);
    }

    protected override void Moving_Wait(float time) {
    }

    protected override void Moving_Out(float time) {
    }

    protected override void Moving_Always(float time) {
        enemy.transform.position += enemy.transform.up * speed * Time.deltaTime;
    }
}
