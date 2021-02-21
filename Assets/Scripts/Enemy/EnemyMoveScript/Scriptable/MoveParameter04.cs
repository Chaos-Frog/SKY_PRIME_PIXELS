using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_04")]
public class MoveParameter04 : Base_MoveParameters {
    [Space(20)]
    public float in_XPos;
    public float YPos;
    public float iteration_time;
    public bool inLeft;

    public MoveParameter04() : base(4) { }
}