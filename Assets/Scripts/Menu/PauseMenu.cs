using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Slider volume;
    public Button toMenu;

    [Header("Mixer")]
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        // Add listener to UI
        toMenu.onClick.AddListener(Menu);
        volume.onValueChanged.AddListener(delegate { SetVolume(volume.value); });

        // Set volume to base value
        float currentVolume = 0f;
        audioMixer.GetFloat("Volume", out currentVolume);
        volume.value = currentVolume;
    }


    // On menu button click
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }


    // On volume changed
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }
}
