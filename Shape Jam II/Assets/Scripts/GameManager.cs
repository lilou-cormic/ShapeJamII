using PurpleCable;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Transform[] SpawnPoints = null;

    private float Speed { get; set; } = 1f;

    private List<Connector> ConnectorList { get; set; }

    private List<Connection> ConnectionList { get; set; }

    private bool _isInitiating;

    private float _timeLeft;

    private Player player;

    private Transform middleSpawnPoint;

    private HashSet<int> prevConnectors;

    private Connector currentConnector;

    private bool IsGameOver;

    private void Awake()
    {
        Instance = this;

        middleSpawnPoint = SpawnPoints[SpawnPoints.Length / 2];

        ConnectorList = new List<Connector>();
        ConnectionList = new List<Connection>();

        prevConnectors = new HashSet<int>();

        IsGameOver = false;
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
    }

    private void Start()
    {
        ScoreManager.ResetScore();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.OnPlayerTryMove += Player_OnPlayerTryMove;

        player.transform.position = middleSpawnPoint.position;

        prevConnectors.Add(SpawnPoints.Length / 2);

        currentConnector = null;

        _isInitiating = true;
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        if (_isInitiating)
        {
            if (middleSpawnPoint.position.y < 12)
            {
                if (_timeLeft <= 0)
                {
                    Spawn();

                    if (currentConnector == null)
                        currentConnector = ConnectorList.FirstOrDefault(x => x.Col == 0);

                    foreach (var spawnPoint in SpawnPoints)
                    {
                        spawnPoint.transform.position += Vector3.up * 3;
                    }

                    _timeLeft = Speed;
                }
                else
                {
                    _timeLeft -= Time.deltaTime;
                }
            }
            else
            {
                _isInitiating = false;
            }
        }
        else
        {
            if (_timeLeft <= 0)
            {
                Spawn();
                Move();

                Speed = Mathf.Clamp(Speed - 0.01f, 0.1f, 1f);

                _timeLeft = Speed;
            }
            else
            {
                _timeLeft -= Time.deltaTime;
            }
        }
    }

    private void Player_OnPlayerTryMove(Vector3 newPosition)
    {
        if (IsGameOver)
            return;

        int newCol = Mathf.RoundToInt(newPosition.x);
        int newRow = Mathf.RoundToInt(newPosition.y);

        var connector = ConnectorList.FirstOrDefault(x => x.Col == newCol && x.Row == newRow);
        if (connector != null)
            Connect(connector);
    }

    private void Connect(Connector connector)
    {
        player.transform.position = connector.transform.position;

        string soundFullPath = Application.dataPath + "/" + connector.ConnectorDef.RelativeSoundPath;

        if (File.Exists(soundFullPath))
        {
            WWW audioLoader = new WWW("file://" + soundFullPath);
            while (!audioLoader.isDone)
            { }

            audioLoader.GetAudioClip().Play(Random.Range(0, 10));
        }

        if (connector.ConnectorDef.IsBad)
        {
            GameOver();
            return;
        }

        if (!currentConnector.Connectors.Contains(connector))
        {
            ScoreManager.AddPoints(1);

            currentConnector.Connectors.Add(connector);

            var connection = ConnectionPool.GetConnection(currentConnector, connector);
            ConnectionList.Add(connection);
        }

        currentConnector = connector;
    }

    private void Spawn()
    {
        HashSet<int> sss = new HashSet<int>();

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            if (Random.Range(0, 2) == 1)
                sss.Add(i);
        }

        if (!sss.Intersect(prevConnectors).Any())
            sss.Add(prevConnectors.Where(x => x >= 0).ToArray().GetRandom());

        for (int i = 0; i < SpawnPoints.Length - 2; i++)
        {
            if (!sss.Contains(i) || !sss.Any(x => x >= i + 2))
                continue;

            int k = sss.Where(x => x >= i + 2).Min();

            for (int j = i + 1; j <= k; j++)
            {
                sss.Add(j);
            }
        }

        prevConnectors.Clear();

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            bool spawnHere = sss.Contains(i);

            if (spawnHere)
            {
                var connector = ConnectorPool.GetConnector(Connectors.GetRandomGoodConnector());
                connector.transform.position = SpawnPoints[i].position;
                ConnectorList.Add(connector);

                connector.name = connector.ConnectorDef.Name;

                prevConnectors.Add(i);
            }
            else
            {
                if (Random.Range(0f, 1f) > 0.7f)
                {
                    var connector = ConnectorPool.GetConnector(Connectors.GetRandomBadConnector());
                    connector.transform.position = SpawnPoints[i].position;
                    ConnectorList.Add(connector);

                    connector.name = connector.ConnectorDef.Name;
                }
            }
        }
    }

    private void Move()
    {
        foreach (var connector in ConnectorList)
        {
            connector.transform.position += Vector3.down * 3;

            if (connector.transform.position.y < -5)
                connector.SetAsAvailable();
        }

        ConnectorList.RemoveAll(x => !x.gameObject.activeSelf);

        foreach (var connection in ConnectionList)
        {
            connection.transform.position += Vector3.down * 3;

            connection.name = connection.transform.position.ToString();

            if (connection.transform.position.y < -5)
                connection.SetAsAvailable();
        }

        ConnectionList.RemoveAll(x => !x.gameObject.activeSelf);

        player.gameObject.transform.position += Vector3.down * 3;

        if (player.gameObject.transform.position.y < -3)
            GameOver();
    }

    private void GameOver()
    {
        IsGameOver = true;

        StartCoroutine(MainMenu.GoToScene("GameOver"));
    }
}
