using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeAudio : MonoBehaviour {
    private AudioSource audio;

    void Start() {
        audio = this.GetComponent<AudioSource>();
    }

    void Update() {
        if(!audio.isPlaying && audio.time == 0.0f) {
            Destroy(this.gameObject);
        }
    }
}
