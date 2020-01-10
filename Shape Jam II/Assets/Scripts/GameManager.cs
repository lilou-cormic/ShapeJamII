using PurpleCable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        ScoreManager.ResetScore();
    }
}
