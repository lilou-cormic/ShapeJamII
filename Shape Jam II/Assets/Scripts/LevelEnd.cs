using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PurpleCable;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MessageText = null;

    [SerializeField] UIThreeStars StarPanel = null;

    [SerializeField] TextMeshProUGUI SavedText = null;

    [SerializeField] TextMeshProUGUI LostText = null;

    [SerializeField] GameObject NextButton = null;

    [SerializeField] Color SavedColor;

    [SerializeField] Color LostColor;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowWin(int starCount)
    {
        MessageText.text = "You win!";
        StarPanel.SetStars(starCount);
        SavedText.color = SavedColor;
        LostText.color = Color.black;
        NextButton.SetActive(true);

        gameObject.SetActive(true);
    }

    public void ShowLose()
    {
        MessageText.text = "You lose";
        StarPanel.SetStars(0);
        SavedText.color = Color.black;
        LostText.color = LostColor;
        NextButton.SetActive(false);

        gameObject.SetActive(true);
    }
}
