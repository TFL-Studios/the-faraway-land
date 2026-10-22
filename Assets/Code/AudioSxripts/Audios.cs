using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Audios : MonoBehaviour
{
  public bool isUsing = false;
  public AudioSource audioSource;

    public void InitAudio(AudioClip audioClip, Vector3 audioPosition)
    {
        this.audioSource.clip = audioClip;
        this.gameObject.SetActive(true);
        this.transform.parent = null;
        this.transform.position = audioPosition;
        this.audioSource.Play();

        this.isUsing = true;
    }

    private void Update()
    {
        if(this.isUsing && audioSource.isPlaying == false)
        {
            AudioManager.OnAudioStop.Invoke(this);

        }
    }


}
