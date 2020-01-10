using PurpleCable;
using UnityEngine;

public class LevelSelectScreen : MonoBehaviour
{
    [SerializeField] LevelButton LevelButtonPrefab = null;

    [SerializeField] GameObject LevelButtonPanel = null;

    int LevelCount = 2;

    private void Start()
    {
        SaveStateManager.LoadSlot(1);

        int[] starCountList = new int[LevelCount];

        LevelStates.LoadLevelStars();

        for (int i = 0; i < LevelCount; i++)
        {
            starCountList[i] = LevelStates.GetLevelStarCount(i + 1);
        }

        for (int i = 0; i < LevelCount; i++)
        {
            LevelButton levelButton = Instantiate(LevelButtonPrefab, LevelButtonPanel.transform);

            int starCount = starCountList[i];
            if (starCount >= 0)
                levelButton.SetLevel(i + 1, starCount);
        }
    }
}
