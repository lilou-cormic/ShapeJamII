using PurpleCable;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour, IPoolable
{
    public ConnectorDef ConnectorDef { get; private set; }

    public int Col => Mathf.RoundToInt(transform.position.x);
    public int Row => Mathf.RoundToInt(transform.position.y);

    public HashSet<Connector> Connectors { get; private set; }

    private void Awake()
    {
        Connectors = new HashSet<Connector>();
    }

    public void Set(ConnectorDef connectorDef)
    {
        ConnectorDef = connectorDef;
    }

    #region IPoolable

    bool IPoolable.IsInUse => gameObject.activeSelf;

    public void SetAsAvailable()
    {
        gameObject.SetActive(false);
        Connectors.Clear();
    }

    void IPoolable.SetAsInUse() => gameObject.SetActive(true);

    #endregion
}
