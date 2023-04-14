using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiNoiseManager : MonoBehaviour
{
    public Camera Camera;
    public UiNoise UiNoise;
    public float Distance;

    public UiNoise Generate(Transform Anchor)
    {
        UiNoise UiNoise = Instantiate(this.UiNoise, transform);
        UiNoise.UiNoiseManager = this;
        UiNoise.Anchor = Anchor;

        return UiNoise;
    }
}
