using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class KeyConfig {
    public KeyCode keyCode_Shot;
    public KeyCode keyCode_Bomb;
    public KeyCode keyCode_Slow;
    public KeyCode keyCode_Pose;
}

public class ControllSetting : MonoBehaviour {
    // キーコンフィグ 辞書型
    public static Dictionary<string, KeyCode> keyConfig = new Dictionary<string, KeyCode>();

    // セーブファイル名
    private const string saveFileName = "KeyConfig.json";

    // キー状態取得関数群
    public static bool GetKey(string key) {
        return Input.GetKey(keyConfig[key]);
    }
    public static bool GetKeyDown(string key) {
        return Input.GetKeyDown(keyConfig[key]);
    }
    public static bool GetKeyUp(string key) {
        return Input.GetKeyUp(keyConfig[key]);
    }

    // キーコンフィグリセット
    public static void ResetKeyConfig() {
        keyConfig = new Dictionary<string, KeyCode>();
        keyConfig.Add("Shot", KeyCode.Space);
        keyConfig.Add("Bomb", KeyCode.X);
        keyConfig.Add("Slow", KeyCode.LeftShift);
        keyConfig.Add("Pose", KeyCode.P);
    }

    // キーコンフィグ保存
    public static void SaveKeyConfig() {
        KeyConfig config = new KeyConfig();
        config.keyCode_Shot = keyConfig["Shot"];
        config.keyCode_Bomb = keyConfig["Bomb"];
        config.keyCode_Slow = keyConfig["Slow"];
        config.keyCode_Pose = keyConfig["Pose"];

        var json = JsonUtility.ToJson(config);

#if !UNITY_WEBGL
        string path = Application.dataPath + "/Save/" + saveFileName;
#else
        string path = Application.persistentDataPath + "/Save/" + saveFileName;
# endif
        var writer = new StreamWriter(path, false);
        writer.WriteLine(json);
        writer.Flush();
        writer.Close();
    }

    // キーコンフィグ読み込み
    public static void LoadKeyConfig() {
#if !UNITY_WEBGL
        string path = Application.dataPath + "/Save/" + saveFileName;
#else
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/Save/" + saveFileName;
#endif
        try {
            File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        } catch {
#if UNITY_WEBGL
            Directory.CreateDirectory(Application.persistentDataPath + "/Save");
#endif
            ResetKeyConfig();
            SaveKeyConfig();
        }
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        keyConfig = new Dictionary<string, KeyCode>();
        var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        KeyConfig config = JsonUtility.FromJson<KeyConfig>(json);
        if(config == null) {
            Debug.Log("Cannot Set KeyConfig... Reset");
            ResetKeyConfig();
            SaveKeyConfig();
        } else {
            Debug.Log("Set KeyConfig");
            keyConfig.Add("Shot", config.keyCode_Shot);
            keyConfig.Add("Bomb", config.keyCode_Bomb);
            keyConfig.Add("Slow", config.keyCode_Slow);
            keyConfig.Add("Pose", config.keyCode_Pose);
        }
    }
}
