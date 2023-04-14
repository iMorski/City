using UnityEngine;

public class House : MonoBehaviour
{
    public UiLoot UiLoot;
    public GameManager GameManager;
    public PlaceManager PlaceManager;
    public StateGroup State;
    public enum StateGroup
    {
        None,
        Loot
    }

    public MeshRenderer Mesh;
    public MeshRenderer MeshDoor;
    public Material Material;
    public Material MaterialLoot;
    public ParticleSystem FX;
    public float NoiseDistance;
    public float Noise;
    
    [HideInInspector] public Vector3 Position;

    private void Awake()
    {
        Position = transform.position + Look();
    }

    [HideInInspector] public bool Loot;
    
    private void OnMouseDown()
    {
        if (Loot)
        {
            UiLoot.Generate();

            Instantiate(FX, transform);

            /* foreach (Zombie Zombie in GameManager.ZombieGroup)
            {
                if (Vector3.Distance(transform.position, Zombie.Guide.transform.position) < NoiseDistance)
                {
                    Zombie.GenerateNoise(Noise);
                }
            } */
        }
        else
        {
            PlaceManager.MouseDown(Position);
        }
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

    public void SetNone()
    {
        State = StateGroup.None;
        
        Loot = false;
        
        ChangeMaterial(Material);
    }

    private void ChangeMaterial(Material Material)
    {
        Mesh.material = Material;
        MeshDoor.material = Material;
    }

    private Vector3 Look()
    {
        return transform.forward;
    }
}
