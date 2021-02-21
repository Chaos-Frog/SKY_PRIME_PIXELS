using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Easing {
    /* イージング Sine */
    /* float型 */
    public static float Ease_In_Sine(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        return dif * (1 - Mathf.Cos(t * Mathf.PI / 2)) + start;
    }
    public static float Ease_Out_Sine(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        return dif * Mathf.Sin(t * Mathf.PI / 2) + start;
    }
    public static float Ease_InOut_Sine(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        return dif * (-(Mathf.Cos(t + Mathf.PI) - 1) / 2) + start;
    }

    /* Vector3型 */
    public static Vector3 Ease_In_Sine(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        return dif * (1 - Mathf.Cos(t * Mathf.PI / 2)) + start;
    }
    public static Vector3 Ease_Out_Sine(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        return dif * Mathf.Sin(t * Mathf.PI / 2) + start;
    }
    public static Vector3 Ease_InOut_Sine(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        return dif * (-(Mathf.Cos(t + Mathf.PI) - 1) / 2) + start;
    }


    /* イージング Quad */
    /* float型 */
    public static float Ease_In_Quad(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        return dif * Mathf.Pow(t, 2) + start;
    }
    public static float Ease_Out_Quad(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        return dif * (1 - Mathf.Pow(1 - t, 2)) + start;
    }
    public static float Ease_InOut_Quad(float start, float end, float t) {
        t = Mathf.Clamp01(t);
        float dif = end - start;
        if (t < 0.5) {
            return dif * (2 * Mathf.Pow(t, 2)) + start;
        } else {
            return dif * (1 - Mathf.Pow(-2 * t + 2, 2) / 2) + start;
        }
    }

    /* Vector3型 */
    public static Vector3 Ease_In_Quad(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        return dif * Mathf.Pow(t, 2) + start;
    }
    public static Vector3 Ease_Out_Quad(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        return dif * (1 - Mathf.Pow(1.0f - t, 2)) + start;
    }
    public static Vector3 Ease_InOut_Quad(Vector3 start, Vector3 end, float t) {
        t = Mathf.Clamp01(t);
        Vector3 dif = end - start;
        if (t < 0.5) {
            return dif * (2 * Mathf.Pow(t, 2)) + start;
        } else {
            return dif * (1 - Mathf.Pow(-2 * t + 2, 2) / 2) + start;
        }
    }
}
