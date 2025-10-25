using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;

    private void Start()
    {
        if (backgroundMusic != null)
        {
            AudioManager.Instance.PlayBackgroundMusic(backgroundMusic);
        }
    }
}