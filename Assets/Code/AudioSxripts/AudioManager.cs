using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class AudioManager : MonoBehaviour
{
    private Audios audios;
    private List<Audios> audioPool = new List<Audios>();
    
    
    
    
    
    private Audios CreateAudio()
    {
        Audios audioSourceInstantiate = Instantiate(audios);
        audioPool.Add(audioSourceInstantiate);
        return audioSourceInstantiate;
    }

    private void OnReleaseToPool(Audios pooledObject)
    {
        for (int i = 0; i < audioPool.Count; i++)
        {
            if (audioPool[i] != audios.isUsing)
            {
                pooledObject.gameObject.SetActive(false);
                pooledObject.transform.parent = this.transform;
                pooledObject.transform.localPosition = Vector3.zero;
            }

        }
            
        
    }

    private void OnGetFromPool(Audios pooledObject, Vector3 audioPosition)
    {
        for (int i = 0; i < audioPool.Count; i++)
        {
            if (audioPool[i] == audios.isUsing)
            {
                pooledObject.gameObject.SetActive(true);
                pooledObject.transform.parent = null;
                pooledObject.transform.position = audioPosition;
                audioPool[i].isUsing = true;
            }
            return;
        }
        
    }

    private void OnDestroyPooledObject(Audios pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    private void PlayAudio()
    {

    }
}
