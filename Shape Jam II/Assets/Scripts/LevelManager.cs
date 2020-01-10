using PurpleCable;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public static int CurrentLevelIndex = 0;

    private static LevelDef[] Levels;

    [SerializeField]
    private TMPro.TextMeshProUGUI LevelText = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI DescriptionText = null;

    [SerializeField] LevelEnd LevelEndPanel = null;

    public static bool IsPlaying = false;
    public static bool IsAnimating = false;
    public static bool IsResolving = false;
    public static bool IsWinning = false;

    private void Awake()
    {
        _instance = this;

        LevelEndPanel.Hide();

        if (Levels == null)
            Levels = Resources.LoadAll<LevelDef>("Levels").OrderBy(x => x.Number).ToArray();

        LoadLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Retry();
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Skip();
            return;
        }
    }

    public void Retry()
    {
        StartCoroutine(MainMenu.GoToScene(SceneManager.GetActiveScene().name));
    }

    public void Skip()
    {
        GoToNextLevel();
    }

    public void Back()
    {
        if (CurrentLevelIndex > 0)
            CurrentLevelIndex--;

        Retry();
    }

    private void LoadLevel()
    {
        IsPlaying = false;
        IsAnimating = false;
        IsResolving = false;
        IsWinning = false;

        LevelDef level = Levels[CurrentLevelIndex];

        LevelText.text = $"{level.Number}/{Levels.Length}";
        DescriptionText.text = level.DescriptionText;

        string layout = level.LevelLayout;

        int row = 0;

        foreach (var line in layout.Split(new string[] { System.Environment.NewLine }, System.StringSplitOptions.None))
        {
            int col = 0;

            foreach (var letter in line)
            {
                //Debug.Log($"{col}, {row}: {letter}");

                switch (letter)
                {
                    case '.':
                        {
                            
                        }
                        break;

                    case '#':
                        {
                            
                        }
                        break;

                    case 'O':
                        {
                            
                        }
                        break;

                    case 'P':
                        {
                            
                        }
                        break;

                    case 'E':
                        {
                           
                        }
                        break;

                    case '*':
                        {
                            
                        }
                        break;

                    default:
                        break;
                }

                col++;
            }

            row++;
        }

        StartTurn();
    }

    public static void Win(int starCount)
    {
        LevelStates.SetLevelStars(CurrentLevelIndex + 1, starCount);

        _instance.LevelEndPanel.ShowWin(starCount);
    }

    public static void Lose()
    {
        _instance.LevelEndPanel.ShowLose();
    }

    public void GoToNextLevel()
    {
        if (CurrentLevelIndex < Levels.Length - 1)
        {
            CurrentLevelIndex++;
            Retry();
        }
        else
        {
            StartCoroutine(MainMenu.GoToScene("LevelSelect"));
        }
    }

    public static void StartTurn()
    {
        IsPlaying = true;
        IsAnimating = false;
        IsResolving = false;
    }

    public static void Resolve()
    {
        IsPlaying = false;
        IsAnimating = false;
        IsResolving = true;

        //bool dd = false;
        //do
        //{
        //    dd = false;
        //    foreach (var item in Lovables)
        //    {
        //        dd |= item.Resolve();
        //    }
        //}
        //while (dd);

        StartTurn();
    }
}
