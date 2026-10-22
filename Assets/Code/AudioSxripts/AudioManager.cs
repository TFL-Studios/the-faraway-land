using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class AudioManager : PersistentSingleton<AudioManager>
{
    private GameObject audiosPrefab;
    private List<Audios> audioPool = new List<Audios>();
    public static UnityAction<Audios> OnAudioStop;

    public void PlayAudio(AudioClip audioClip, Vector3 audioPosition)
    {
        
        bool audioFlag = OnGetFromPool(audioClip, audioPosition);
        if (!audioFlag) 
        {
            CreateAudio().InitAudio(audioClip, audioPosition);
        
        
        }
    }

    private void OnEnable()
    {
        OnAudioStop += OnReleaseToPool;
    }

    private void OnDisable()
    {
        OnAudioStop -= OnReleaseToPool;
    }

    private Audios CreateAudio()
    {
        Audios audioSourceInstantiate = Instantiate(audiosPrefab).GetComponent<Audios>();
        audioPool.Add(audioSourceInstantiate);
        
        return audioSourceInstantiate;
    }

    private void OnReleaseToPool(Audios pooledObject)
    {
                
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.parent = this.transform;
                pooledObject.transform.localPosition = Vector3.zero;
        
            
        
    }

    private bool OnGetFromPool(AudioClip audioClip, Vector3 audioPosition)
    {
        for (int i = 0; i < audioPool.Count; i++)
        {
            if (!audioPool[i].isUsing)
            {
                audioPool[i].InitAudio(audioClip, audioPosition);
               
                return true;
            }
            
        }
        return false;
    }

    private void OnDestroyPooledObject(Audios pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    
}
