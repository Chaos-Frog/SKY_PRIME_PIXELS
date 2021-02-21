using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_M01")]
public class MoveParameter_M01 : Base_MoveParameters {
    [Space(20)]
    public float moveSpeed_Fast;
    public float moveSpeed;

    public MoveParameter_M01() : base(6) { }
}
