using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/MovePatern/MoveParam_05")]
public class MoveParameter05 : Base_MoveParameters {
    [Space(20)]
    public float moveSpeed;
    public float rotateLimit;

    public MoveParameter05() : base(5) { }
}