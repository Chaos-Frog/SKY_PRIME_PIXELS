using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_05 : Base_MovePatern {
    public MoveParameter05 param05;

    public MovePatern_05(Base_MoveParameters parameter) : base(parameter) {
        param05 = parameter as MoveParameter05;
    }

    public override void Init(GameObject obj) {
        base.Init(obj);
    }

    protected override void Moving_Always(float time) {
        if(enemy.transform.position.y > GameController.Instance.playerObj.transform.position.y) {
            LookPlayer(param05.rotateLimit);
        }
        enemy.transform.position += enemy.transform.up * param05.moveSpeed * Time.deltaTime;

        if(enemy.transform.position.y < GameController.window_RBPos.y) withdrawal = true;
    }
}
