using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StageDataItem {
	public int enemyType;     // 敵タイプ"
	public float spawnTime;   // 出現時間"
	public float position_X;  // 出現座標_X"
	public float position_Y;  // 出現座標_Y"
	public int movePatern;    // 移動パターン"
	public int danmakuPatern; // 弾幕パターン"
}

[System.Serializable]
public class StageData {
	public StageDataItem[] items; // リストテーブル
}
