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
    public int round;
    public float currTime;
    public float timeToFinish = 90;
    float magnifing;

    public static GameManager Instance; // A static reference to the GameManager instance

    void Awake()
    {
        if (Instance == null) // If there is no instance already
            Instance = this;
        else if (Instance != this) // If there is already an instance and it's not `this` instance
            Destroy(gameObject); // Destroy the GameObject, this component is attached to

        round = PlayerPrefs.GetInt("Round");
        audio = GetComponent<AudioSource>();
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    void Start()
    {
        currTime = Time.time;
        levelBuilder.StartBuildLevel();
        if (PlayerPrefs.GetInt("SoundToggle") == 0)
        {
            AudioListener.volume = 0;
            canvas.soundToggle.SetActive(true);
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
        AudioLoopToLose( gameLoopAudio);
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
        currTime = Time.time;
        levelBuilder.ResetAllLevel();
        canvas.ResetCanvas();
        canvas.GotKey(true);
        CancelInvoke("cameraResize");
        mainCamera.orthographicSize = 0.5f;
        StartMovement();
    }

    public IEnumerator AudioFade(AudioSource audioSource, float FadeTime , bool fadeIn)
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

            while (audioSource.volume < 0.1f)
            {
                audioSource.volume += startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
        }
    }
    
    

}
