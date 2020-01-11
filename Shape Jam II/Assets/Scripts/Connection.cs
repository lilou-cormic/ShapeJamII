using PurpleCable;
using UnityEngine;

public class Connection : MonoBehaviour, IPoolable
{
    #region IPoolable

    bool IPoolable.IsInUse => gameObject.activeSelf;

    public void SetAsAvailable() => gameObject.SetActive(false);

    void IPoolable.SetAsInUse() => gameObject.SetActive(true);

    #endregion
}
