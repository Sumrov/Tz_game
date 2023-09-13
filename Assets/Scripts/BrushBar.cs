using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BrushBar : MonoBehaviour
{
    public Brush CurrentBrush { get; private set; }
    [SerializeField] private SwipeController _swipeController;
    [SerializeField] private RectTransform _brushesScanner;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Brush _prefab;
    private List<Brush> _brushes = new();

    public void Init(Color[] colors)
    {
        Fill(colors);
        if (_brushes.Count % 2 == 0) _scrollRect.content.anchoredPosition += new Vector2(0, _swipeController.SwipeStep / 2);
        UpdateCurrentBrush();
        SetSizeBrushes();
    }

    public void DeleteCurrentBrush()
    {
        Destroy(CurrentBrush.gameObject);
        _brushes.Remove(CurrentBrush);
        _scrollRect.content.anchoredPosition = new Vector2(_scrollRect.content.anchoredPosition.x, .0f);
        if (_brushes.Count % 2 == 0) _scrollRect.content.anchoredPosition += new Vector2(0, _swipeController.SwipeStep / 2);
        List<Action> callback = new() { UpdateCurrentBrush, SetSizeBrushes };
        StartCoroutine(SkipFrame(callback));
    }

    private void UpdateCurrentBrush()
    {
        Canvas.ForceUpdateCanvases();

        RectTransform scannerRectTransform = _brushesScanner.GetComponent<RectTransform>();
        float scannerY = scannerRectTransform.position.y;

        CurrentBrush = _brushes.SingleOrDefault(o =>
        {
            RectTransform itemRectTransform = o.GetComponent<RectTransform>();
            float itemY = itemRectTransform.position.y;

            return itemY < scannerY + 1 && itemY > scannerY - 1;
        });
    }

    private IEnumerator SkipFrame(List<Action> callback)
    {
        yield return null;
        foreach (var i in callback) i();
    }

    private void Awake()
    {
        _swipeController.Swiped += OnSwipe;
    }

    private void OnDestroy()
    {
        _swipeController.Swiped -= OnSwipe;
    }

    private void Fill(Color[] colors)
    {
        foreach (var i in colors)
        {
            var brush = Instantiate(_prefab, _scrollRect.content.transform);
            brush.Init(i);
            _brushes.Add(brush);
        }
    }

    private void OnSwipe()
    {
        if (_brushes.Count == 0) return;
        UpdateCurrentBrush();
        if (CurrentBrush == null)
        {
            _swipeController.UndoSwipe();
            return;
        }
        SetSizeBrushes();
    }

    private void SetSizeBrushes()
    {
        var indexCurrentBrush = _brushes.IndexOf(CurrentBrush);

        foreach (var brush in _brushes)
        {
            brush.CanvasGroup.alpha = 1f;

            if (_brushes.IndexOf(brush) == indexCurrentBrush)
                brush.transform.localScale = new(1.2f, 1.2f, brush.transform.localScale.z);
            else if (_brushes.IndexOf(brush) == indexCurrentBrush + 1 || _brushes.IndexOf(brush) == indexCurrentBrush - 1)
                brush.transform.localScale = new(1.1f, 1.1f, brush.transform.localScale.z);
            else if (_brushes.IndexOf(brush) == indexCurrentBrush + 2 || _brushes.IndexOf(brush) == indexCurrentBrush - 2)
                brush.transform.localScale = new(.9f, .9f, brush.transform.localScale.z);
            else if (_brushes.IndexOf(brush) == indexCurrentBrush + 3 || _brushes.IndexOf(brush) == indexCurrentBrush - 3)
                brush.transform.localScale = new(.7f, .7f, brush.transform.localScale.z);
            else brush.CanvasGroup.alpha = .0f;
        }
    }
}
