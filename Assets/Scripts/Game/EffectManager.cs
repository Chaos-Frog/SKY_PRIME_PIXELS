using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour {
    // インスタンスオブジェクト
    public static EffectManager Instance;

    // エフェクト列挙体
    public enum Effects {
        Explosion,
        Hit,
        PlayerHit,
        PlayerBarrier,
        Bomber,
        Exp_Charge,
    }

    // エフェクトプレハブ
    public GameObject[] effectPrefabs;


    void Start() {
        Instance = this;
    }

    public GameObject InstantiateEffect(Effects effect, Vector3 position) {
        GameObject ef = Instantiate(effectPrefabs[(int)effect]);
        ef.transform.position = position;

        return ef;
    }
}
