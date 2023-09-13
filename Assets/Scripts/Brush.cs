using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Brush : MonoBehaviour
{
    public Color Color { get; private set; }
    public CanvasGroup CanvasGroup { get; private set; }
    [SerializeField] private Image _image;

    public void Init(Color color)
    {
        Color = color;
        _image.color = color;
    }

    private void Awake()
    {
        CanvasGroup = GetComponent<CanvasGroup>();
    }
}
