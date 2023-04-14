using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public float Speed;

    private Coroutine Coroutine;

    public void Generate(List<Vector3> PositionGroup)
    {
        if (PositionGroup.Count < 2) return;

        PositionGroup[0] = (PositionGroup[0] + PositionGroup[1]) / 2;
        
        if (Coroutine != null) StopCoroutine(Coroutine);
        Coroutine = StartCoroutine(GenerateCoroutine(PositionGroup));
    }

    private IEnumerator GenerateCoroutine(List<Vector3> PositionGroup)
    {
        SetCount(GetCount() + 1);
        
        LineRenderer.SetPosition(0, new Vector3(
            PositionGroup[0].x, 0.25f, PositionGroup[0].z));
        
        for (int i = 1; i < PositionGroup.Count; i++)
        {
            SetCount(GetCount() + 1);
            
            LineRenderer.SetPosition(i, new Vector3(
                PositionGroup[i - 1].x, 0.25f, PositionGroup[i - 1].z));
            
            Vector3 Position = new Vector3(PositionGroup[i].x, 0.25f, PositionGroup[i].z);

            while (LineRenderer.GetPosition(i) != Position)
            {
                LineRenderer.SetPosition(i, Vector3.MoveTowards(LineRenderer.GetPosition(i), 
                    Position, Speed * Time()));

                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void SetCount(int Count)
    {
        LineRenderer.positionCount = Count;
    }

    private int GetCount()
    {
        return LineRenderer.positionCount;
    }

    public void Clear()
    {
        if (Coroutine != null) StopCoroutine(Coroutine);
        
        SetCount(0);
    }

    private float Time()
    {
        return UnityEngine.Time.deltaTime;
    }
}
