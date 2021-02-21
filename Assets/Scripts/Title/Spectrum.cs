using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectrum : MonoBehaviour {
    public AudioSource audio;
    public float[] spectrum;
    public float[] volume;

    void Start() {
        spectrum = new float[1024];
        volume = new float[1024];
        audio = this.GetComponent<AudioSource>();
    }

    void Update() {
        audio.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        audio.GetOutputData(volume, 0);
    }
}
