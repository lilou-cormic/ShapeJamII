using PurpleCable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] Transform LeftSpawnPoint = null;

    [SerializeField] Transform MiddleSpawnPoint = null;

    [SerializeField] Transform RightSpawnPoint = null;

    private float Speed { get; set; } = 1f;

    private Transform[] SpawnPoints { get; set; }

    private List<Connector> ConnectorList { get; set; }

    private List<Connection> ConnectionList { get; set; }

    private bool _isInitiating;

    private float _timeLeft;

    private Player player;

    private HashSet<int> prevConnectors;

    private Connector currentConnector;

    private bool IsGameOver;

    private void Awake()
    {
        Instance = this;

        ConnectorList = new List<Connector>();
        ConnectionList = new List<Connection>();

        SpawnPoints = new Transform[] { LeftSpawnPoint, MiddleSpawnPoint, RightSpawnPoint };

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

        prevConnectors.Add(1);

        currentConnector = null;

        _isInitiating = true;
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        if (_isInitiating)
        {
            if (LeftSpawnPoint.position.y < 12)
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
        List<int> sss = new List<int>(3);

        for (int i = 0; i < 3; i++)
        {
            if (Random.Range(0, 3) == i)
                sss.Add(i);
        }

        if (!sss.Intersect(prevConnectors).Any())
            sss.Add(prevConnectors.Where(x => x >= 0).ToArray().GetRandom());

        if (sss.Contains(0) && sss.Contains(2))
            sss.Add(1);

        prevConnectors.Clear();

        for (int i = 0; i < 3; i++)
        {
            bool spawnHere = sss.Contains(i);

            if (spawnHere)
            {
                var connector = ConnectorPool.GetConnector(Connectors.GetRandomConnector());
                connector.transform.position = SpawnPoints[i].position;
                ConnectorList.Add(connector);

                connector.name = connector.transform.position.ToString();

                prevConnectors.Add(i);
            }
        }
    }

    private void Move()
    {
        foreach (var connector in ConnectorList)
        {
            connector.transform.position += Vector3.down * 3;

            connector.name = connector.transform.position.ToString();

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
