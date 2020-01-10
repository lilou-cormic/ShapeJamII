using UnityEngine;

public class UIThreeStars : MonoBehaviour
{
    [SerializeField] UIStar Star1 = null;

    [SerializeField] UIStar Star2 = null;

    [SerializeField] UIStar Star3 = null;

    public void SetStars(int starCount)
    {
        if (Star1 != null)
        {
            Star1.IsWon = starCount >= 1;
            Star2.IsWon = starCount >= 2;
            Star3.IsWon = starCount >= 3;
        }
    }
}
