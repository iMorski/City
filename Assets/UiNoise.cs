using UnityEngine;
using UnityEngine.UI;

public class UiNoise : MonoBehaviour
{
    public UiNoiseManager UiNoiseManager;
    public Animator Animator;
    public Image Bar;

    [HideInInspector] public Transform Anchor;

    private void Update()
    {
        if (Anchor) transform.position = Position(Anchor.transform.position);
    }

    public void In()
    {
        Animator.Play("Ui-Noise-In");
    }

    public void Out()
    {
        Animator.Play("Ui-Noise-Out");
    }

    public void ChangeValue(float Value)
    {
        Fill(Bar, Value);
    }

    private Vector3 Position(Vector3 Position)
    {
        Vector3 DistanceInView = UiNoiseManager.Camera.ViewportToScreenPoint(new Vector3(0.0f, UiNoiseManager.Distance, 0.0f));
        Vector3 CameraView = new Vector3((UiNoiseManager.Camera.WorldToScreenPoint(Position).x - Screen.width / 2.0f) * 2.0f / 100.0f, 0.0f, 0.0f);

        return UiNoiseManager.Camera.WorldToScreenPoint(Position) + DistanceInView + CameraView;
    }
    
    private void Fill(Image Image, float Value)
    {
        Image.fillAmount = Value;
    }

    private float GetFill(Image Image)
    {
        return Image.fillAmount;
    }
}
