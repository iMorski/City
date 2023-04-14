using UnityEngine;

public class Chase : MonoBehaviour
{
    [SerializeField] private Transform Anchor;
    [SerializeField] private float SmoothTime;
    
    private Vector3 Position;

    private void Awake()
    {
        Position = transform.position;
    }

    private Vector3 Velocity;

    private void Update()
    {
        if (!Anchor) return;
        
        Vector3 OnAnchorPosition = new Vector3(Position.x + Anchor.transform.position.x,
            Position.y, Position.z + Anchor.transform.position.z);
        Vector3 OnAnchorPositionSmooth = Vector3.SmoothDamp(transform.position,
            OnAnchorPosition, ref Velocity, SmoothTime);
        
        transform.position = OnAnchorPositionSmooth;
    }
}