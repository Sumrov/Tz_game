using System.Collections;
using UnityEngine;

public class DrawingElement : MonoBehaviour
{
    public bool IsComplete { get; private set; }
    public Color BaseColor { get; private set; }
    public bool CanDraw { get; private set; } = true;
    private Material _material;
    private Drawing _drawing;

    public DrawingElement Init(Drawing drawing)
    {
        _drawing = drawing;
        return this;
    }

    public IEnumerator DisplayBaseColor(float delay)
    {
        var wait = new WaitForSeconds(delay);
        Color currentColor = _material.color;
        CanDraw = false;

        _material.color = BaseColor;
        yield return wait;
        _material.color = currentColor;
        CanDraw = true;
    }

    public void ColorElement(Color color)
    {
        if (IsComplete || !CanDraw) return;
        _material.color = color;
        if (BaseColor == color) IsComplete = true;
        _drawing.OnDraw(IsComplete);
    }

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        BaseColor = _material.color;
        _material.color = Color.white;
    }
}
