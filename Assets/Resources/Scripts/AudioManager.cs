using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

    public AudioSource audioSource;
    public AudioClip playCardSound;
    public AudioClip drawCardSound;

    public void DrawCardSound() {
        audioSource.PlayOneShot(drawCardSound);
    }

    public void PlayCardSound() {
        audioSource.PlayOneShot(playCardSound);
    }
}
