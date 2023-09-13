using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class SwipeController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public event Action Swiped;
    public float SwipeStep => _swipeStep;
    [SerializeField] private float _swipeThreshold = 50.0f;
    [SerializeField] private float _swipeStep = 138.0f;
    private Vector2 _startDragPosition;
    private ScrollRect _scrollRect;
    private float _lastStep = 0;

    public void Swipe(float step)
    {
        _scrollRect.content.anchoredPosition += new Vector2(0, step);
        _lastStep = step;
        Swiped?.Invoke();
    }

    public void UndoSwipe()
    {
        Swipe(-_lastStep);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startDragPosition = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var endDragPosition = eventData.position;
        float swipeDelta = endDragPosition.y - _startDragPosition.y;

        if (Mathf.Abs(swipeDelta) >= _swipeThreshold)
        {
            if (swipeDelta > 0) Swipe(_swipeStep);
            else Swipe(-_swipeStep);
        }
    }

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }
}
