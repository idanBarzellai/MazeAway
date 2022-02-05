using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasControl : MonoBehaviour
{
    // Control the level canvas and menu bar
    float currTime;
    float timeToFinish;
    public TMP_Text timerText;
    public TMP_Text counterText;
    public TMP_Text counterCoin;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject timerPanel;
    public GameObject menuPanel;
    public GameObject soundToggle;
    public GameObject pauseMenu;
    public TMP_Text threeTwoOne;
    public TMP_Text message;
    public TMP_Text itemMessage;
    public Image keyImg;
    public AudioClip tickTockAudio;
    private AudioSource audio;
    private AudioController threeTwoOneControl;
    public LevelLoad levelLoader;

    Color32 currColor;
    bool tenSecs =false;
    private GameManager gameManager;

    void Start()
    {
        threeTwoOneControl = GameObject.FindGameObjectWithTag("music").GetComponent<AudioController>();
        currColor = timerPanel.GetComponentInChildren<Image>().color;
        gameManager = GameManager.Instance;
        timeToFinish = gameManager.timeToFinish;
        audio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Update the currect timer and coin status
        // Sends the game into losing state if timer is on 0
        counterCoin.text = "x " + PlayerPrefs.GetInt("Coins");
        currTime = gameManager.currTime;
        float timer = timeToFinish - (Time.time - currTime);
        if (timer >= 0)
        {
            if (timer <= 10)
            {
                if (!tenSecs)
                {
                    GetComponent<AudioSource>().clip = tickTockAudio;
                    GetComponent<AudioSource>().Play();
                    timerPanel.GetComponentInChildren<Image>().color = Color32.Lerp(currColor, new Color32(255, 0, 0, 100), 2f);
                    StartCoroutine(MessageOut("HURRY UP!"));
                    InvokeRepeating("FlashText", 0, 0.2f);
                    InvokeRepeating("VibrateDevice", 0, 1f);
                    tenSecs = true;
                }
                timerText.text = "0" + (timer).ToString("F2");
            }
            else
            {
                GetComponent<AudioSource>().Pause();
                timerText.text = (timer).ToString("F2");
            }
            counterText.text = "ROUND : " + gameManager.round;
        }
        else
        {
            CancelInvoke("VibrateDevice");
            timerText.text = "00:00";
            gameManager.LoseState();
            counterText.text = "ROUND : --";
        }
    }

    void FlashText()
    {
        timerPanel.GetComponentInChildren<Image>().enabled = !timerPanel.GetComponentInChildren<Image>().enabled;  
    }

    public void ResetCanvas()
    {
        timerPanel.GetComponentInChildren<Image>().color = currColor;
        timerPanel.GetComponentInChildren<Image>().enabled = true;
        CancelInvoke("FlashText"); CancelInvoke("VibrateDevice");
        loseScreen.SetActive(false);
        winScreen.SetActive(false);
        tenSecs = false;
    }

    public void ShowLostScreen(int round)
    {
        loseScreen.SetActive(true);
        loseScreen.GetComponentInChildren<TMP_Text>().text = "YOU  SURVIVED  " + (round -1) + "  ROUNDS!";  
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        PauseGame();
        StartCoroutine(Countdown(5));
    }

    public void GotKey(bool isReset)
    {
        Color32 temp = keyImg.color;
        if (!isReset)
            keyImg.color = new Color32(temp.r, temp.g, temp.b, 255);
        else
        {
            keyImg.color = new Color32(temp.r, temp.g, temp.b, 50);
            gameManager.player.GotKey(false);
        }
    }

    public void VibrateDevice()
    {
        if(PlayerPrefs.GetInt("SoundToggle") == 1)
            Handheld.Vibrate();
    }

    public void PauseGame()
    {
        if(Time.timeScale == 1)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public IEnumerator Countdown(int count)
    {  
        if ( count == 5)
        {
            audio.Pause();
            
            threeTwoOne.text = "YOU WON!";
            yield return new WaitForSecondsRealtime(1);
            count--;
        }
        if (count == 4)
        {
            threeTwoOne.text = "NEXT MAZE";
            yield return new WaitForSecondsRealtime(1);
            count--;
        }        
        while (count > 0 && count < 4)
        {
            if (count == 3)
                threeTwoOneControl.PlayThreetwoOne();

            threeTwoOne.text = "" +count;
            yield return new WaitForSecondsRealtime(1);
            count--;
        }
        if (count == 0)
        {
            threeTwoOne.text = "GO!";
            yield return new WaitForSecondsRealtime(1);
        }
        audio.clip = tickTockAudio;
        RetryGame();  
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

    public IEnumerator ItemMessageOut(string msg)
    {
        while (itemMessage.enabled == false)
        {
            itemMessage.enabled = true;
            itemMessage.text = msg;
            yield return new WaitForSecondsRealtime(1);
        }
        itemMessage.enabled = false;
    }

    public void RetryGame()
    {  
        gameManager.RetryGame();
        if (Time.timeScale == 0)
            PauseGame();
    }

    public void SoundToggle()
    {
        if (AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
            soundToggle.SetActive(true);
            PlayerPrefs.SetInt("SoundToggle", 0);
        }
        else
        {
            AudioListener.volume = 1;
            soundToggle.SetActive(false);
            PlayerPrefs.SetInt("SoundToggle", 1);
        }        
    }

    public void tenSecsOn()
    {
        tenSecs = false;
    }

    public void MainMenuButton()
    {
        PauseGame();
        StartCoroutine(levelLoader.TransitionLoad("StartMenu"));      
        pauseMenu.SetActive(false);  
    }

    public void RetryButton()
    {
        PauseGame();
        pauseMenu.SetActive(false);
        PlayerPrefs.SetInt("Round", 1);
        StartCoroutine(levelLoader.TransitionLoad("Level1"));
    }

    public void PauseButton()
    {
        PauseGame();
        pauseMenu.SetActive(!pauseMenu.active);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
