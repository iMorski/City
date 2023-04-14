using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiQuit : MonoBehaviour
{
    public Animator Animator;
    public TMP_Text Description;
    public TMP_Text Score;
    public Color Escape;
    public Color Caught;

    public void Quit(GameManager.ConclusionGroup Conclusion, int Count)
    {
        Description.text = Conclusion != GameManager.ConclusionGroup.Caught ? "ESCAPED" : "CAUGHT";
        Description.color = Conclusion != GameManager.ConclusionGroup.Caught ? Escape : Caught;
        
        Score.text = "x<size=24>" + Count;
        
        Animator.Play("Quit-Out");
    }
    
    public void Re()
    {
        SceneManager.LoadScene(0);
    }
}
