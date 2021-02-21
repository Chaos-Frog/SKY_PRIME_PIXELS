using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_01")]
public class MoveParameter01 : Base_MoveParameters {
    [Space(20)]
    public float move_y;

    public MoveParameter01() : base(1) { }
}