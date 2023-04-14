using UnityEngine;

public class HouseQuit : MonoBehaviour
{
    public PlaceManager PlaceManager;
    
    [HideInInspector] public Vector3 Position;

    private void Awake()
    {
        Position = transform.position + Look();
    }
    
    private void OnMouseDown()
    {
        PlaceManager.MouseDown(Position);
    }
    
    private void OnMouseEnter()
    {
        PlaceManager.MouseEnter(Position);
    }

    private void OnMouseExit()
    {
        PlaceManager.MouseExit();
    }

    private void OnMouseUp()
    {
        PlaceManager.MouseUp();
    }
    
    private Vector3 Look()
    {
        return transform.forward;
    }
}
