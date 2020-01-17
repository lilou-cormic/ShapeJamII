using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Connectors
{
    private static ConnectorDef[] _connectors;

    [RuntimeInitializeOnLoadMethod]
    public static void LoadConnectors()
    {
        _connectors = Resources.LoadAll<ConnectorDef>("Connectors");

        //if (_connectors == null)
        //    _connectors = new ConnectorDef[] { new ConnectorDef() { Name = "Connector", DisplayName = "Connector" } };
    }

    public static ConnectorDef GetRandomGoodConnector()
    {
        return _connectors.Where(x => !x.IsBad).ToArray().GetRandom();
    }

    public static ConnectorDef GetRandomBadConnector()
    {
        return _connectors.Where(x => x.IsBad).ToArray().GetRandom();
    }
}
