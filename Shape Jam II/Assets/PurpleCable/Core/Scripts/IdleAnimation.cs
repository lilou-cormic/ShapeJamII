using UnityEngine;

public class IdleAnimation : MonoBehaviour
{
    [SerializeField]
    private float Tick = 0.7f;

    [SerializeField]
    private Vector3 MinScale = new Vector3(0.95f, 1f, 1f);

    [SerializeField]
    private Vector3 MaxScale = new Vector3(1f, 1f, 1f);

    [SerializeField] bool IsOnlyOnMouseOver = false;

    private Vector3 _normalScale = Vector3.one;

    private float _timer = 0f;
    private int _scaleFactor = 1;

    private bool _isMouseOver = false;

    private void Start()
    {
        _normalScale = transform.localScale;
    }

    private void Update()
    {
        if (IsOnlyOnMouseOver && !_isMouseOver)
            return;

        _timer += Time.deltaTime * _scaleFactor;

        if (_timer >= Tick)
        {
            _timer = Tick;
            _scaleFactor = -1;
        }
        else if (_timer <= 0)
        {
            _timer = 0;
            _scaleFactor = 1;
        }

        transform.localScale = new Vector3(_normalScale.x * (MinScale.x + ((MaxScale.x - MinScale.x) * (_timer / Tick))), _normalScale.y * (MinScale.y + ((MaxScale.y - MinScale.y) * (_timer / Tick))), _normalScale.z * (MinScale.z + ((MaxScale.z - MinScale.z) * (_timer / Tick))));
    }

    private void OnMouseEnter()
    {
        _isMouseOver = true;
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;

        if (IsOnlyOnMouseOver)
        {
            transform.localScale = _normalScale;

            _timer = 0;
            _scaleFactor = 1;
        }
    }
}
