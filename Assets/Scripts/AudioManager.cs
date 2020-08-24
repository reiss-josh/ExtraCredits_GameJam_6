using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip useSound;
    public AudioClip toolSound;
    public AudioClip doneSound;
    public AudioClip gameOverSound;
    public AudioClip loseHealthSound;
    public AudioClip robotDeathSound;
    public AudioClip textSound;
    public AudioClip failUseSound;
    public AudioClip failDoneSound;
    AudioSource mySource;
    
    void Start()
    {
        mySource = GetComponent<AudioSource>();
    }

    public void playDone()
    {
        mySource.PlayOneShot(doneSound);
    }

    public void playFail()
    {
        mySource.PlayOneShot(failDoneSound);
    }

    public void playTool()
    {
        mySource.PlayOneShot(toolSound);
    }

    public void playToolFail()
    {
        mySource.PlayOneShot(failUseSound);
    }

    public void playUse()
    {
        mySource.PlayOneShot(useSound);
    }

    public void playText(float delay = 0, int times = 1)
    {
        StartCoroutine(playSoundRepeating(textSound, delay, times));
    }

    public void playDamage(float delay = 0, int times = 1)
    {
        StartCoroutine(playSoundRepeating(loseHealthSound, delay, times));
    }

    public void playRobotDeath(float delay = 0, int times = 1)
    {
        StartCoroutine(playSoundRepeating(robotDeathSound, delay, times));
    }

    public void playGameOver()
    {
        mySource.PlayOneShot(gameOverSound);
    }

    IEnumerator playSoundRepeating(AudioClip sound, float delay = 0, int times = 1)
    {
        //Debug.Log(delay);
        yield return new WaitForSeconds(delay);
        while (times > 0)
        {
            times -= 1;
            mySource.PlayOneShot(sound);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
