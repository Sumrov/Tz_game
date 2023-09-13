using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioClip _goodChoice;
    [SerializeField] private AudioClip _badChoice;
    [SerializeField] private AudioSource _audioSource;

    public void PlayGoodChoice()
    {
        _audioSource.clip = _goodChoice;
        _audioSource.Play();
    }

    public void PlayBadChoice()
    {
        _audioSource.clip = _badChoice;
        _audioSource.Play();
    }
}
