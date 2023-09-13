using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _displayBaseColorDelay = 3f;
    [SerializeField] private BrushBar _brushBar;
    [SerializeField] private Drawing _drawing;
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private Audio _audio;

    private void Start()
    {
        _drawing.Draw += OnDraw;
        _drawing.DisplayBaseColor(_displayBaseColorDelay);
        _brushBar.Init(_drawing.UniqueDrawingColors());
    }

    private void OnDraw(bool success)
    {
        if (success) _audio.PlayGoodChoice();
        else _audio.PlayBadChoice();

        UpdateProgressBar();

        var currentBrush = _brushBar.CurrentBrush;
        if (!_drawing.DrawingElements.Any(o => o.BaseColor == currentBrush.Color && !o.IsComplete))
            _brushBar.DeleteCurrentBrush();
    }

    private void UpdateProgressBar()
    {
        var countDravingElements = _drawing.DrawingElements.Count;
        var coloredElements = _drawing.DrawingElements.Where(o => o.IsComplete).Count();
        _progressBar.OnValueChanged(coloredElements, countDravingElements);
    }

    private void OnDestroy()
    {
        _drawing.Draw -= OnDraw;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit)) return;
            if (!hit.collider.TryGetComponent(out DrawingElement element)) return;

            var currentBrush = _brushBar.CurrentBrush;
            if(currentBrush != null) element.ColorElement(currentBrush.Color);
        }
    }
}
