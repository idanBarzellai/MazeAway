using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    // Control the Menu scene
    public TMP_Text soundToggle;
    public GameObject continueButton;
    public TMP_Text record;
    public TMP_Text coins;
    public TMP_Text message;
    public LevelLoad levelLoader;

    private void Start()
    {
        if (PlayerPrefs.GetInt("SoundToggle") == 0)
        {
            AudioListener.volume = 0;
            soundToggle.text = "mute";
        }
        else AudioListener.volume = 1;

        if (PlayerPrefs.GetInt("Round") == null || PlayerPrefs.GetInt("Round") <= 1)
            continueButton.SetActive(false);
        else continueButton.SetActive(true);

        record.text = "RECORD: " + PlayerPrefs.GetInt("Record");
        coins.text = "COINS: " + PlayerPrefs.GetInt("Coins");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
    public void StartButton()
    {
        PlayerPrefs.SetInt("Round", 1);
        StartCoroutine(AudioFade(GetComponent<AudioSource>(), 1f, false));
        StartCoroutine(levelLoader.TransitionLoad("Level1"));
    }

    public void SoundToggle()
    {
        if (AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
            soundToggle.text = "mute";
            PlayerPrefs.SetInt("SoundToggle", 0);
        }
        else
        {
            AudioListener.volume = 1;
            soundToggle.text = "sound";
            PlayerPrefs.SetInt("SoundToggle", 1);
        }
    }

    public void ConitnueButton()
    {
        StartCoroutine(AudioFade(GetComponent<AudioSource>(), 1f, false));
        StartCoroutine(levelLoader.TransitionLoad("Level1"));

    }
    public void ShopButton()
    {
        StartCoroutine(MessageOut("COMING SOON!"));
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void DeletePLayerPrefsButton()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Round", 1);
    }

    public IEnumerator MessageOut(string msg)
    {
        while (message.enabled == false)
        {
            message.enabled = true;
            message.text = msg;
            yield return new WaitForSecondsRealtime(1);
        }
        message.enabled = false;
    }

    public IEnumerator AudioFade(AudioSource audioSource, float FadeTime, bool fadeIn)
    {
        if (!fadeIn)
        {

            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }

        }
        else
        {

            float startVolume = 0.01f;

            while (audioSource.volume < 0.25f)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }
}
