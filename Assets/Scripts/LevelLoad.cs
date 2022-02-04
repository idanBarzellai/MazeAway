using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoad : MonoBehaviour
{
    public Animator anim;

    public IEnumerator TransitionLoad(string levelname)
    {
        anim.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(levelname);
    }
}

