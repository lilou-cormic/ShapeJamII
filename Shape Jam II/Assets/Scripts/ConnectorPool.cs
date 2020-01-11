using PurpleCable;
using UnityEngine;

public class ConnectorPool : Pool<Connector>
{
    private static ConnectorPool _instance;

    [SerializeField] Connector ConnectorPrefab = null;

    protected override void Awake()
    {
        base.Awake();

        _instance = this;
    }

    protected override Connector CreateItem()
    {
        return Instantiate(ConnectorPrefab, transform);
    }

    public static Connector GetConnector(ConnectorDef connectorDef)
    {
        Connector connector = _instance.GetItem();
        connector.Set(connectorDef);

        return connector;
    }
}
