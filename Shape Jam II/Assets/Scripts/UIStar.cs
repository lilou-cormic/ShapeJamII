using UnityEngine;
using UnityEngine.UI;

public class UIStar : MonoBehaviour
{
    [SerializeField] Color WonColor;

    [SerializeField] Color LostColor;

    [SerializeField] Image StarImage;

    public bool IsWon = false;

    private void OnValidate()
    {
        SetColor();
    }

    private void OnEnable()
    {
        SetColor();
    }

    private void SetColor()
    {
        if (StarImage == null)
            return;

        if (IsWon)
            StarImage.color = WonColor;
        else
            StarImage.color = LostColor;
    }
}
