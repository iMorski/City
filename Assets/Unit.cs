using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Transform Guide;
    public List<NavMeshAgent> UnitGroup;
    public float Speed;
    public float Radius;
    
    [HideInInspector] public Vector3 Position;

    private void Awake()
    {
        Position = Guide.transform.position;
    }
    
    private Coroutine Coroutine;
    
    public virtual void Move(List<Vector3> PositionGroup)
    {
        List<Vector3> PositionGroupCopy = new List<Vector3>();

        foreach (Vector3 Position in PositionGroup)
        {
            PositionGroupCopy.Add(Position);
        }
        
        if (Coroutine != null) StopCoroutine(Coroutine);
        Coroutine = StartCoroutine(MoveCoroutine(PositionGroupCopy));
    }
    
    public virtual IEnumerator MoveCoroutine(List<Vector3> PositionGroup)
    {
        for (int i = 0; i < PositionGroup.Count; i++)
        {
            Position = PositionGroup[i];
            
            while (Guide.position != Position)
            {
                Guide.position = Vector3.MoveTowards(Guide.position, Position, Speed * Time());

                for (int j = 0; j < UnitGroup.Count; j++)
                {
                    Vector3 Position = new Vector3(Guide.transform.position.x + Radius * Mathf.Cos(2.0f * Mathf.PI * j / UnitGroup.Count),
                        Guide.transform.position.y, Guide.transform.position.z + Radius * Mathf.Sin(2.0f * Mathf.PI * j / UnitGroup.Count));

                    UnitGroup[j].SetDestination(Position);
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    public float Time()
    {
        return UnityEngine.Time.deltaTime;
    }
}
