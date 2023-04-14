using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiLoot : MonoBehaviour
{
    public GameManager GameManager;
    public Camera Camera;
    public Animator Animator;
    public Image Bar;
    public TMP_Text Score;
    public float Distance;
    public float Amount;

    private void Update()
    {
        if (House) transform.position = Position(House.transform.position);
    }

    private House House;

    public void In(House House)
    {
        this.House = House;
        
        Fill(Bar, 0.0f);

        Animator.Play("Loot-In");
    }

    public void Out()
    {
        Animator.Play("Loot-Out");
    }

    public void Generate()
    {
        Fill(Bar, GetFill(Bar) + Amount);
        
        Animator.Play("Loot-Push-In", 1);

        if (GetFill(Bar) >= 1)
        {
            Animator.Play("Loot-Drop");
            
            House.SetNone();

            Increment();
        }
    }
    
    private Vector3 Position(Vector3 Position)
    {
        Vector3 DistanceInView = Camera.ViewportToScreenPoint(new Vector3(0.0f, Distance, 0.0f));
        Vector3 CameraView = new Vector3((Camera.WorldToScreenPoint(Position).x - Screen.width / 2.0f) * 2.0f / 100.0f, 0.0f, 0.0f);

        return Camera.WorldToScreenPoint(Position) + DistanceInView + CameraView;
    }
    
    private void Fill(Image Image, float Value)
    {
        Image.fillAmount = Value;
    }

    private float GetFill(Image Image)
    {
        return Image.fillAmount;
    }

    private void Increment()
    {
        GameManager.Count = GameManager.Count + 1;

        Score.text = "x<size=24>" + GameManager.Count;
    }

    private float Time()
    {
        return UnityEngine.Time.deltaTime;
    }
}
