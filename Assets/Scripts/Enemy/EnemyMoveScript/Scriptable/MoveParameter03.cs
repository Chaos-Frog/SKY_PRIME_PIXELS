using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_03")]
public class MoveParameter03 : Base_MoveParameters {
    [Space(20)]
    public float move_speed;
    public float turnAngle;
    public bool turnLeft;

    public MoveParameter03() : base(3) { }
}