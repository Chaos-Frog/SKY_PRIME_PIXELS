using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanmakuPatern_Mix : Base_DanmakuPatern {
    private DanmakuParameterMix param;    // パラメータ
    private Base_DanmakuPatern[] danmaku;  // 弾幕クラス配列

    public DanmakuPatern_Mix(BaseDanmakuParameter dp) : base(dp) {
        param = dp as DanmakuParameterMix;
        danmaku = new Base_DanmakuPatern[param.danmaku.Length];
        for(int i = 0; i < param.danmaku.Length; i++) {
            Base_DanmakuPatern danmaku_p = SetDanmakuPaternClass(param.danmaku[i]);
            danmaku[i] = danmaku_p;
        }
    }

    public override void Init(GameObject e) {
        base.Init(e);
        foreach(Base_DanmakuPatern d_patern in danmaku) {
            d_patern.Init(enemy);
        }
    }

    public override void Reset() {
        base.Reset();
        foreach(Base_DanmakuPatern d_patern in danmaku) {
            d_patern.Reset();
        }
    }

    public override void ShotDanmaku() {
        if(allTime >= param.time_start) {
            bool end_All = true;
            foreach(Base_DanmakuPatern d_patern in danmaku) {
                d_patern.Shot();
                if(!d_patern.end) {
                    end_All = false;
                }
            }
            if(end_All) end = true;
        }
        base.ShotDanmaku();
    }
}
