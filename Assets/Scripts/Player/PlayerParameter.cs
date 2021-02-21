using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PlayerParam")]
public class PlayerParameter : ScriptableObject {
    public float moveSpeed;
    public float coreRadius;
    public float shotInterval;
    public float defaultShot_speed;

    [Space(20)]
    public float fourWay_speed;
    public float fourWay_betweenAngle;
    public float fourWay_betweenAngle_slow;

    [Space(20)]
    public float spread_maxSpeed;
    public float spread_minSpeed;
    public float spread_betweenAngle;
    public float spread_betweenAngle_slow;
}
