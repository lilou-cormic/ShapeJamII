using PurpleCable;
using UnityEngine;

public class ConnectionPool : Pool<Connection>
{
    private static ConnectionPool _instance;

    [SerializeField] Connection ConnectionPrefab = null;

    protected override void Awake()
    {
        base.Awake();

        _instance = this;
    }

    protected override Connection CreateItem()
    {
        return Instantiate(ConnectionPrefab, transform);
    }

    public static Connection GetConnection(Connector connectorFrom, Connector connectorTo)
    {
        var posFrom = connectorFrom.transform.position;
        var posTo = connectorTo.transform.position;

        Connection connection = _instance.GetItem();
        connection.transform.SetRotation2D(posFrom, posTo);
        connection.transform.position = posFrom + ((posTo - posFrom) / 2f);

        return connection;
    }
}
