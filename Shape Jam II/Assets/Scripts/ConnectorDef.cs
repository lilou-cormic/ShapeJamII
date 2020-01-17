using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Connector", menuName = "Connector")]
public class ConnectorDef : ScriptableObject, IEqualityComparer<ConnectorDef>
{
    public string Name { get => name; set => name = value; }

    public string DisplayName;

    public string RelativeFilePath = @"Resources/Connector128x128.png";

    public string RelativeSoundPath = @"Resources/ConnectorSound.wav";

    public bool IsBad = false;

    public override string ToString()
    {
        return Name;
    }

    public override int GetHashCode()
    {
        return Name?.GetHashCode() ?? base.GetHashCode();
    }

    public override bool Equals(object other)
    {
        return Equals(this, other as ConnectorDef);
    }

    public bool Equals(ConnectorDef x, ConnectorDef y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (ReferenceEquals(null, x))
            return false;

        if (ReferenceEquals(null, y))
            return false;

        return x.Name == y.Name;
    }

    public int GetHashCode(ConnectorDef obj)
    {
        return obj.GetHashCode();
    }
}
