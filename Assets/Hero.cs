using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hero : Unit
{
    public GameManager GameManager;
    public UiLoot UiLoot;
    public ParticleSystem StepFX;
    public float StepGap;
    public float Noise;
    public float NoiseDistance;
    public float DieDistance;

    private bool Run;

    private float Count;

    private void Update()
    {
        foreach (Zombie Zombie in GameManager.ZombieGroup)
        {
            if (Vector3.Distance(Guide.transform.position, Zombie.Guide.transform.position) < DieDistance)
            {
                GameManager.Quit(GameManager.ConclusionGroup.Caught);
            }
        }
        
        if (Run)
        {
            Count = Count + Time();
            
            if (Count >= StepGap)
            {
                foreach (NavMeshAgent Unit in UnitGroup)
                {
                    Instantiate(StepFX, Unit.transform.position, new Quaternion());

                    /* foreach (Zombie Zombie in GameManager.ZombieGroup)
                    {
                        if (Vector3.Distance(Guide.transform.position, Zombie.Guide.transform.position) < NoiseDistance)
                        {
                            Zombie.GenerateNoise(Noise);
                        }
                    } */
                }
                
                Count = 0;
            }
        }
    }

    private House House;

    public override void Move(List<Vector3> PositionGroup)
    {
        GameManager.Busy = true;

        if (House && House.State != House.StateGroup.None)
        {
            UiLoot.Out();

            House.Loot = false;
        }
        
        base.Move(PositionGroup);
    }

    public override IEnumerator MoveCoroutine(List<Vector3> PositionGroup)
    {
        Run = true;
        
        yield return base.MoveCoroutine(PositionGroup);

        Run = false;
        
        Count = 0;
        
        House = GetHouse(PositionGroup[^1], GameManager.HouseQuitGroup);

        if (GameManager.HouseQuitGroup.Contains(House))
        {
            GameManager.Quit(GameManager.ConclusionGroup.Escape);
            
            yield break;
        }
        
        House = GetHouse(PositionGroup[^1], GameManager.HouseGroup);

        if (House)
        {
            UiLoot.In(House);

            House.Loot = true;
        }
        
        GameManager.Busy = false;
    }

    private House GetHouse(Vector3 Position, List<House> HouseGroup)
    {
        foreach (House House in HouseGroup)
        {
            if (House.State != House.StateGroup.None && Vector3.Distance(House.Position, Position) < 0.05f)
            {
                return House;
            }
        }

        return null;
    }
}
