using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePatern_04 : Base_MovePatern {
    public MoveParameter04 param04;

    public MovePatern_04(Base_MoveParameters parameter) : base(parameter) {
        param04 = parameter as MoveParameter04;
    }

    protected override void Moving_In(float time) {
        Vector3 endPos;
        if(param04.inLeft) {
            endPos = new Vector3(GameController.window_RBPos.x - param04.in_XPos, param04.YPos, 0.0f);
        } else {
            endPos = new Vector3(GameController.window_LUPos.x + param04.in_XPos, param04.YPos, 0.0f);
        }
        enemy.transform.position = Easing.Ease_Out_Quad(firstPos, endPos, time / param.time_in);
    }

    protected override void Moving_Wait(float time) {
        float theta = (180.0f / param04.iteration_time) * time;
        float px;
        if(param04.inLeft) {
            px = Mathf.Cos(theta * Mathf.Deg2Rad) * (GameController.window_RBPos.x - param04.in_XPos);
        } else {
            px = Mathf.Cos(theta * Mathf.Deg2Rad) * (GameController.window_LUPos.x + param04.in_XPos);
        }
        enemy.transform.position = new Vector3(px, param04.YPos, 0.0f);

        firstPos = enemy.transform.position;
    }

    protected override void Moving_Out(float time) {
        Vector3 endPos;
        if(param04.inLeft) {
            endPos = new Vector3(GameController.window_RBPos.x + 2.0f, firstPos.y, 0.0f);
        } else {
            endPos = new Vector3(GameController.window_LUPos.x - 2.0f, firstPos.y, 0.0f);
        }
        enemy.transform.position = Easing.Ease_In_Quad(firstPos, endPos, time / param.time_out);
    }

    protected override void Moving_Always(float time) {
        if(!withdrawal) {
            LookPlayer(4.0f);
        } else {
            float toAngle;
            if(param04.inLeft) toAngle = 270.0f;
            else               toAngle = 90.0f;
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.Euler(0.0f, 0.0f, toAngle), 4.0f);
        }
    }
}
