using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ExtraCreditsText = null;

    private void Start()
    {
        string filename = Application.dataPath + "/Resources/ExtraCredits.txt";

        if (!File.Exists(filename))
            return;

        ExtraCreditsText.text = File.ReadAllText(filename);
    }
}
