using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Drawing : MonoBehaviour
{
    public event Action<bool> Draw;
    public ReadOnlyCollection<DrawingElement> DrawingElements => new(_drawingElements);
    private List<DrawingElement> _drawingElements = new();

    public Color[] UniqueDrawingColors()
    {
        Color[] colors = new Color[_drawingElements.Count];
        for (var i = 0; i < colors.Length; i++) colors[i] = _drawingElements[i].BaseColor;

        return colors.Distinct().ToArray();
    }

    public void DisplayBaseColor(float delay)
    {
        foreach (var i in _drawingElements)
        {
            if (!i.CanDraw) continue;
            StartCoroutine(i.DisplayBaseColor(delay));
        }
    }

    public void Awake()
    {
        foreach (Transform i in transform)
        {
            var driwingElement = i.AddComponent<DrawingElement>().Init(this);
            _drawingElements.Add(driwingElement);
        }
    }

    public void OnDraw(bool success)
    {
        Draw?.Invoke(success);
    }
}
