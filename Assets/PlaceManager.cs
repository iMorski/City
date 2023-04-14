using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    public GameManager GameManager;
    public Line Line;
    public int From;
    public int To;

    private List<Place> PlaceGroup = new List<Place>();
    private List<Place> RegionGroup = new List<Place>();
    public class Place
    {
        public Vector3 Position;
        public List<Place> NeighbourGroup = new List<Place>();
    }

    private void Awake()
    {
        /* Generate Place */
        
        for (int Z = From; Z < To + 1; Z++)
        {
            for (int X = From; X < To + 1; X++)
            {
                Vector3 Position = new Vector3(X, 0, Z);
                
                if (Available(Position))
                {
                    Place Place = new Place();
                    Place.Position = Position;
                    
                    PlaceGroup.Add(Place);
                }
            }
        }
        
        /* Generate Place Neighbour Group */
        
        float Distance = Vector3.Distance(PlaceGroup[0].Position,
            PlaceGroup[1].Position) + 0.05f;

        for (int i = 0; i < PlaceGroup.Count; i++)
        {
            for (int j = 0; j < PlaceGroup.Count; j++)
            {
                if (PlaceGroup[i] != PlaceGroup[j] && Vector3.Distance(
                        PlaceGroup[i].Position,PlaceGroup[j].Position) < Distance)
                {
                    PlaceGroup[i].NeighbourGroup.Add(PlaceGroup[j]);
                }
            }
        }
        
        /* Generate Region */

        for (int Z = -5; Z < 5 + 1; Z++)
        {
            for (int X = -5; X < 5 + 1; X++)
            {
                Vector3 Position = new Vector3(X, 0, Z);

                if (Available(Position))
                {
                    RegionGroup.Add(GetPlace(Position));
                }
            }
        }
    }
    
    private List<Vector3> PositionGroup = new List<Vector3>();
    
    private bool Mouse;
    private bool MouseOn;
    
    public void MouseDown(Vector3 Position)
    {
        if (GameManager.Busy) return;
        
        Mouse = true;
        MouseOn = true;

        GeneratePositionGroup(Position);
    }
    
    public void MouseEnter(Vector3 Position)
    {
        if (!Mouse) return;
        
        MouseOn = true;

        GeneratePositionGroup(Position);
    }

    private void GeneratePositionGroup(Vector3 Position)
    {
        PositionGroup.Clear();
        PositionGroup.Add(GameManager.Hero.Position);
        
        List<Place> PlaceGroup = GetRouteGroup( GetPlace(GameManager.Hero.Position), 
            GetPlace(Position), this.PlaceGroup);

        foreach (Place Place in PlaceGroup)
        {
            PositionGroup.Add(Place.Position);
        }
        
        Line.Clear();
        Line.Generate(PositionGroup);
    }
    
    public void MouseExit()
    {
        if (!Mouse) return;

        MouseOn = false;
    }
    
    public void MouseUp()
    {
        if (MouseOn && PositionGroup.Count > 0)
        {
            GameManager.Hero.Move(PositionGroup);
        }
        
        MouseOn = false;
        Mouse = false;
        
        Line.Clear();
    }
    
    /* Route */
    
    private class Route
    {
        public Place Place;
        public Place PlacePrevious;
        public float Distance;
    }

    public List<Vector3> GetPositionGroup(Vector3 Position)
    {
        List<Place> PlaceGroup = GetRouteGroup(GetPlace(Position),
            RegionGroup[Random.Range(0, RegionGroup.Count)], RegionGroup);
        
        List<Vector3> PositionGroup = new List<Vector3>();
        
        foreach (Place Place in PlaceGroup)
        {
            PositionGroup.Add(Place.Position);
        }

        return PositionGroup;
    }
    
    public List<Place> GetRouteGroup(Place PlaceBegin, Place PlaceFinish, List<Place> PlaceCheckGroup)
    {
        List<Route> RouteGroup = new List<Route>();

        foreach (Place Place in PlaceGroup)
        {
            Route Route = new Route();
            Route.Place = Place;
            
            RouteGroup.Add(Route);
        }

        int GetRouteIndex(Place Place)
        {
            int Index = 0;
            
            for (int i = 0; i < RouteGroup.Count; i++)
            {
                if (!(RouteGroup[i].Place != Place))
                {
                    Index = i;

                    break;
                }
            }

            return Index;
        }
        
        List<Place> PlaceRouteGroup = new List<Place>();
        List<Place> PlaceFoundGroup = new List<Place>();
        List<Place> PlaceCheckedGroup = new List<Place>();
        
        PlaceFoundGroup.Add(PlaceBegin);

        while (PlaceFoundGroup.Count != PlaceCheckedGroup.Count)
        {
            int PlaceFoundIndex = 0;

            if (PlaceCheckedGroup.Contains(PlaceFoundGroup[PlaceFoundIndex]))
            {
                for (int i = 1; i < PlaceFoundGroup.Count; i++)
                {
                    if (!PlaceCheckedGroup.Contains(PlaceFoundGroup[i]))
                    {
                        PlaceFoundIndex = i;
                    }
                }
            }
            
            for (int i = 1; i < PlaceFoundGroup.Count; i++)
            {
                if (!PlaceCheckedGroup.Contains(PlaceFoundGroup[i]) && RouteGroup[GetRouteIndex(PlaceFoundGroup[i])].Distance <
                    RouteGroup[GetRouteIndex(PlaceFoundGroup[PlaceFoundIndex])].Distance)
                {
                    PlaceFoundIndex = i;
                }
            }

            if (!(PlaceFoundGroup[PlaceFoundIndex] != PlaceFinish))
            {
                Place Place = PlaceFoundGroup[PlaceFoundIndex];

                while (Place != PlaceBegin)
                {
                    PlaceRouteGroup.Add(Place);

                    Place = RouteGroup[GetRouteIndex(Place)].PlacePrevious;
                }
        
                PlaceRouteGroup.Reverse();

                break;
            }

            PlaceCheckedGroup.Add(PlaceFoundGroup[PlaceFoundIndex]);

            foreach (Place Place in PlaceCheckGroup)
            {
                if (!PlaceCheckedGroup.Contains(Place) && !PlaceFoundGroup.Contains(Place) &&
                    PlaceFoundGroup[PlaceFoundIndex].NeighbourGroup.Contains(Place))
                {
                    float DistanceToBegin = Vector3.Distance(Place.Position, PlaceBegin.Position);
                    float DistanceToFinish = Vector3.Distance(Place.Position, PlaceFinish.Position);

                    RouteGroup[GetRouteIndex(Place)].PlacePrevious = PlaceFoundGroup[PlaceFoundIndex];
                    RouteGroup[GetRouteIndex(Place)].Distance = DistanceToBegin + DistanceToFinish;
                
                    PlaceFoundGroup.Add(Place);
                }
            }
        }

        return PlaceRouteGroup;
    }
    
    /* API */
    
    private bool Available(Vector3 Position)
    {
        foreach (House House in GameManager.HouseGroup)
        {
            if (Vector3.Distance(Position, House.transform.position) < 0.05f) return false;
        }
        
        return true;
    }

    private Place GetPlace(Vector3 Position)
    {
        foreach (Place Place in PlaceGroup)
        {
            if (Vector3.Distance(Position, Place.Position) < 0.05f) return Place;
        }

        return null;
    }
}
