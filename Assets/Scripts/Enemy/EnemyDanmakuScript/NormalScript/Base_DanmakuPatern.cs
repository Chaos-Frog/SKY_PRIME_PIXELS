using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_DanmakuPatern {
    // 基底弾幕パラメータ
    protected BaseDanmakuParameter baseParam;

    protected GameObject enemy;  // 敵オブジェクト
    protected float allTime;     // 全体経過時間
    public bool end;             // 終了判定

    public Base_DanmakuPatern(BaseDanmakuParameter dp) {
        baseParam = dp;
    }

    public virtual void Init(GameObject e) {
        enemy = e;
    }

    public virtual void Reset() {
        allTime = 0.0f;
        end = false;
    }

    public void Shot() {
        if(!end) {
            if(allTime >= baseParam.time_start) {
                ShotDanmaku();
            } else {
                allTime += Time.deltaTime;
            }
        }
    }

    public virtual void ShotDanmaku() {}

    // 敵弾発射関数
    protected void ShotBullet(GameObject bullet, Vector3 pos, float speed, float angle, float size, bool rank = false) {
        if(pos.y >= GameController.window_RBPos.y / 2.0f) {
            EnemyBullets e_bullet = Object.Instantiate(bullet, pos, Quaternion.identity).GetComponent<EnemyBullets>();

            if(rank) {
                speed += (float)GameController.Instance.gameRank / GameController.Instance.gameRank_Max * 3.0f - 1.5f;
            }

            e_bullet.SetUp(speed, angle, size);
        }
    }

    // プレイヤーまでの角度算出（中心から）
    protected float LookPlayer() {
        GameObject player = GameController.Instance.playerObj;
        return UtilityFunction.GetToAngle(enemy.transform.position, player.transform.position) * Mathf.Rad2Deg - 90.0f;
    }

    // プレイヤーまでの角度算出（指定座標から）
    protected float LookPlayer(Vector3 pos) {
        GameObject player = GameController.Instance.playerObj;
        return UtilityFunction.GetToAngle(pos, player.transform.position) * Mathf.Rad2Deg - 90.0f;
    }

    // NWay弾
    protected void NWayShot(int shotNum, float baseAngle, float betweenAngle, GameObject bullet, Vector3 pos, float b_speed, float b_size, bool rank = false) {
        for(int i = 0; i < shotNum; i++) {
            float angle = (i * betweenAngle) + baseAngle - (shotNum / 2 * betweenAngle);
            if(shotNum % 2 == 0) angle += betweenAngle / 2.0f;
            ShotBullet(bullet, pos, b_speed, angle, b_size, rank);
        }
    }

    static public Base_DanmakuPatern SetDanmakuPaternClass(BaseDanmakuParameter param) {
        //Debug.Log(param.paternNumber);
        switch(param.paternNumber) {
            case 1: return new DanmakuPatern_NWay(param);
            case 2: return new DanmakuPatern_Rotate_NWay(param);

            case 4: return new DanmakuPatern_Mix(param);
            
            case 11: return new DanmakuPatern_Boss01(param);
            case 12: return new DanmakuPatern_Boss02(param);
            case 13: return new DanmakuPatern_Boss03(param);
            case 14: return new DanmakuPatern_BossEx(param);

            default: return null;
        }
    }
}
