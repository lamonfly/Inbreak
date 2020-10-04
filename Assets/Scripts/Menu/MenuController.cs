using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Buttons")]
    public Button play;
    public Button exit;
    public Slider volume;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        // Add listener to UI
        play.onClick.AddListener(Play);
        exit.onClick.AddListener(ExitGame);
        volume.onValueChanged.AddListener(delegate { SetVolume(volume.value); });

        // Set volume to base value
        float currentVolume = 0f;
        audioMixer.GetFloat("Volume", out currentVolume);
        volume.value = currentVolume;
    }

    // On quit button click
    public void ExitGame()
    {
        Application.Quit();
    }

    // On play button click
    public void Play()
    {
        SceneManager.LoadScene("Main");
    }

    // On volume changed
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
