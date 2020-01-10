using PurpleCable;
using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI LevelNumberText = null;

    [SerializeField] Color StarWonColor;

    [SerializeField] Color StarLostColor;

    [SerializeField] GameObject LockedImage = null;

    [SerializeField] GameObject UnlockedPanel = null;

    [SerializeField] UIThreeStars StarPanel = null;

    private int _levelNumber = 0;

    private void Awake()
    {
        LockedImage.SetActive(true);

        UnlockedPanel.SetActive(false);
    }

    public void SetLevel(int levelNumber, int starCount)
    {
        _levelNumber = levelNumber;

        LevelNumberText.text = levelNumber.ToString();

        StarPanel.SetStars(starCount);

        LockedImage.SetActive(false);

        UnlockedPanel.SetActive(true);
    }

    public void Play()
    {
        if (_levelNumber > 0)
        {
            LevelManager.CurrentLevelIndex = _levelNumber - 1;

            StartCoroutine(MainMenu.GoToScene("Main"));
        }
    }
}
