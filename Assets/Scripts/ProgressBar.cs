using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider _progress;

    public void OnValueChanged(int value, int maxValue)
    {
        _progress.value = (float)value / maxValue;
    }
}
