using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public CanvasControl canvas;
    public PlayerController player;
    public Camera mainCamera;
    public BuildLevel levelBuilder;
    public AudioClip gameLoopAudio;
    public AudioClip loseAudio;
    public AudioSource audio;
    public MapContorller map;
    public int round;
    public float currTime;
    private float timeToFinish;
    float magnifing;

    public static GameManager Instance; // A static reference to the GameManager instance

    void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this) // If there is already an instance and it's not `this` instance
            Destroy(gameObject); // Destroy the GameObject, this component is attached to
        if (PlayerPrefs.GetInt("Round") == null || PlayerPrefs.GetInt("Round") < 1)
            round = 1;
        else
            round = PlayerPrefs.GetInt("Round");
        audio = GetComponent<AudioSource>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        timeToFinish = 90;
    }

    void Start()
    {
        currTime = Time.time;
        levelBuilder.StartBuildLevel();
        if (PlayerPrefs.GetInt("SoundToggle") == 0)
        {
            AudioListener.volume = 0;
            canvas.soundToggle.text = "mute";

        }
        else AudioListener.volume = 1;
        canvas.winScreen.SetActive(true);

        StartCoroutine(canvas.Countdown(3));
    }

    public void WinLevel()
    {
        canvas.ShowWinScreen();
        round++;
        PlayerPrefs.SetInt("Round", round);
    }

    public void LoseState()
    {
        player.LostLevel();
        canvas.ShowLostScreen(round);
        AudioLoopToLose(loseAudio);
    }

    public void AudioLoopToLose(AudioClip clip)
    {
        audio.Pause();
        audio.clip = clip;
        audio.Play();
    }

    public void StartMovement()
    {
        player.StartMovement();
        AudioLoopToLose(gameLoopAudio);
        StartCoroutine(AudioFade(audio, 0.5f, true));
    }

    public void SetCurrTime(float newTime)
    {
        currTime += newTime;
        canvas.ResetCanvas();
    }

    public void MagnifyingGLass(float reqSize)
    {
        magnifing = reqSize;
        InvokeRepeating("cameraResize", 0.1f, 0.1f);

    }

    private void cameraResize()
    {
        mainCamera.orthographicSize *= 1.01f;
        if (mainCamera.orthographicSize >= magnifing)
            CancelInvoke("cameraResize");
    }

    public void RetryGame()
    {
        // Reset all gameobjects as the start of a new level
        if (PlayerPrefs.GetInt("Record") < round)
            PlayerPrefs.SetInt("Record", round);
        currTime = Time.time;
        levelBuilder.ResetAllLevel();
        canvas.ResetCanvas();
        canvas.GotKey(true);
        CancelInvoke("cameraResize");
        mainCamera.orthographicSize = 0.5f;
        mainCamera.gameObject.GetComponent<Camera_Follow>().SetWaited(false); ;
        StartMovement();
        map.resetIcons();

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

            audioSource.Stop();
            audioSource.volume = startVolume;
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

    public float GetTimeToFinish()
    {
        return timeToFinish;
    }
    public void SetTimeToFinish(float toSet)
    {
        timeToFinish = toSet;
    }
}
