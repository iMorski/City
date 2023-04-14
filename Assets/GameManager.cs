using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<House> HouseGroup;
    public List<House> HouseQuitGroup;
    public List<Zombie> ZombieGroup;
    public Hero Hero;
    public UiQuit UiQuit;

    [HideInInspector] public int Count;
    [HideInInspector] public bool Busy;

    private void Awake()
    {
        ChangeFPS();
    }

    public enum ConclusionGroup
    {
        Escape,
        Caught
    }

    public void Quit(ConclusionGroup Conclusion)
    {
        if (Conclusion != ConclusionGroup.Escape) Count = 0;
        
        UiQuit.Quit(Conclusion, Count);
    }
    
    private void ChangeFPS()
    {
        Application.targetFrameRate =
            Screen.currentResolution.refreshRate;
    }
}
