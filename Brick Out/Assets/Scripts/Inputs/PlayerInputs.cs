using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LineRenderer _trajectoryLine;
    [SerializeField] private float _launchForce;
    [SerializeField] private Collider2D _inputArea;

    private IBallLauncher _ballLauncher;
    private Vector2 _startDragPos;
    private bool _isDragging;

    private void Awake() => _ballLauncher = GetComponent<IBallLauncher>();

    private void Update()
    {
        HandleInput();
    }

    private bool IsInInputArea(Vector2 position)
    {
        return _inputArea.OverlapPoint(position);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && IsInInputArea(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            StartDrag();
        else if (Input.GetMouseButton(0) && _isDragging)
            UpdateTrajectory();
        else if (Input.GetMouseButtonUp(0) && _isDragging)
            EndDrag();
    }

    private void StartDrag()
    {
        if (!GameManager.Instance.LaunchStatus())
        {
            Debug.Log("IsDraw");
            _startDragPos = GameManager.Instance.GetCurrentSpawnPosition();
            _isDragging = true;
            _trajectoryLine.gameObject.SetActive(true);

        }
    }

    private void UpdateTrajectory()
    {
        Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (endPos - _startDragPos).normalized;

        _trajectoryLine.positionCount = 2;
        _trajectoryLine.SetPosition(0, _startDragPos);
        _trajectoryLine.SetPosition(1, _startDragPos + direction * 2f);
    }

    private void EndDrag()
    {
        Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (endPos - _startDragPos).normalized;

        _ballLauncher.LaunchBalls(direction, _launchForce);
        _trajectoryLine.gameObject.SetActive(false);
        GameManager.Instance.SetLaunchStatus();
        _isDragging = false;
    }
}