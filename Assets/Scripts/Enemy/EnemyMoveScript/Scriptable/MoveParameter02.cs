using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_02")]
public class MoveParameter02 : Base_MoveParameters {
    [Space(20)]
    public float move_y;
    public float rotateLimit;

    public MoveParameter02() : base(2) { }
}