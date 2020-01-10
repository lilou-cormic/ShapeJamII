using PurpleCable;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public static class LevelStates
{
    private static readonly Dictionary<int, int> _levelStars = new Dictionary<int, int>();

    public static void SetLevelStars(int level, int starCount)
    {
        if (!_levelStars.TryGetValue(level, out int currentStarCount) || currentStarCount < starCount)
            _levelStars[level] = starCount;

        if (starCount > 0 && !_levelStars.ContainsKey(level + 1))
            _levelStars[level + 1] = 0;

        SaveStateManager.SaveScene("LevelStates", _levelStars.Select(x => new XElement("L" + x.Key.ToString(), x.Value)));
        SaveStateManager.SaveSlot(1);
    }

    public static void LoadLevelStars()
    {
        _levelStars.Clear();
        _levelStars[1] = 0;

        XElement xLevelStates = SaveStateManager.LoadScene("LevelStates");

        if (xLevelStates != null)
        {
            foreach (var xLvl in xLevelStates.Elements())
            {
                int.TryParse(xLvl.Name.LocalName.Substring(1), out int level);
                int starCount = (int?)xLvl ?? 0;

                if (level > 0)
                    _levelStars[level] = starCount;
            }
        }
    }

    public static int GetLevelStarCount(int level)
    {
        return (_levelStars.TryGetValue(level, out int starCount) ? starCount : -1);
    }
}
