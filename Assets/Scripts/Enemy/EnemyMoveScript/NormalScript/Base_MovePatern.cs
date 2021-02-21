using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_MovePatern {
    // パラメータ
    protected Base_MoveParameters param;

    protected float allTime;     // 全体経過時間
    protected Vector3 firstPos;  // 初期座標
    protected GameObject enemy;  // 敵オブジェクト

    public bool withdrawal;      // 離脱判定

    public Base_MovePatern(Base_MoveParameters parameters) {
        param = parameters;
    }

    // 初期化処理
    public virtual void Init(GameObject obj) {
        enemy = obj;
        firstPos = enemy.transform.position;
    }

    // 移動処理群
    protected virtual void Moving_In     (float time) { }  // 入場時
    protected virtual void Moving_Wait   (float time) { }  // 待機時
    protected virtual void Moving_Out    (float time) { }  // 離脱時
    protected virtual void Moving_Always (float time) { }  // 常時

    // 総合移動処理
    public void Moving() {
        if(enemy != null) {
            if(allTime <= param.time_in) {
                Moving_In(allTime);
            } else if((allTime - param.time_in) <= param.time_wait) {
                Moving_Wait(allTime - param.time_in);
            } else if((allTime - param.time_in - param.time_wait) <= param.time_out) {
                if(!withdrawal) withdrawal = true;
                Moving_Out(allTime - param.time_in - param.time_wait);
            } else {
                if(!withdrawal) withdrawal = true;
            }

            Moving_Always(allTime);

            allTime += Time.deltaTime;
        }
    }

    // プレイヤーを注視
    protected void LookPlayer(float rotateLimit) {
        GameObject player = GameController.Instance.playerObj;
        float angle = UtilityFunction.GetToAngle(enemy.transform.position, player.transform.position) * Mathf.Rad2Deg;
        enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.Euler(0.0f, 0.0f, angle - 90), rotateLimit);
    }

    // プレイヤーまでの角度Quaternion
    protected Quaternion LookPlayerQuat(Vector3 basePos) {
        GameObject player = GameController.Instance.playerObj;
        float angle = UtilityFunction.GetToAngle(basePos, player.transform.position) * Mathf.Rad2Deg;
        return Quaternion.Euler(0.0f, 0.0f, angle - 90);
    }


    // 移動パターンクラス設定
    static public Base_MovePatern SetMovePaternClass(Base_MoveParameters param) {
        switch(param.paternNumber) {
            case 1: return new MovePatern_01(param);
            case 2: return new MovePatern_02(param);
            case 3: return new MovePatern_03(param);
            case 4: return new MovePatern_04(param);
            case 5: return new MovePatern_05(param);
            case 6: return new MovePatern_M01(param);

            case 11: return new MovePatern_Boss01(param);
            case 12: return new MovePatern_Boss02(param);
            case 13: return new MovePatern_Boss03(param);
            case 14: return new MovePatern_BossEx(param);

            default: return null;
        }
    }
}
