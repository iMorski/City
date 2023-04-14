using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit
{
    public PlaceManager PlaceManager;
    public UiNoiseManager UiNoiseManager;
    public UiNoise UiNoise;
    public float NoiseReduction;

    [HideInInspector] public float Noise;
    [HideInInspector] public bool IsNoise;
    
    private void Start()
    {
        UiNoise = UiNoiseManager.Generate(Guide.transform);

        Move(PlaceManager.GetPositionGroup(Guide.transform.position));
    }

    private void Update()
    {
        UiNoise.ChangeValue(Noise);
        
        Noise = Mathf.MoveTowards(Noise, 0.0f, 
            NoiseReduction * Time());

        if (!IsNoise && !(Noise != 0))
        {
            UiNoise.Out();
            
            IsNoise = true;
        }
    }

    public override IEnumerator MoveCoroutine(List<Vector3> PositionGroup)
    {
        yield return base.MoveCoroutine(PositionGroup);
        
        Move(PlaceManager.GetPositionGroup(Guide.transform.position));
    }

    public void GenerateNoise(float Amount)
    {
        if (IsNoise)
        {
            UiNoise.In();
        }
        
        IsNoise = false;

        Noise = Mathf.Clamp(Noise + Amount, 0, 1);
    }
}
