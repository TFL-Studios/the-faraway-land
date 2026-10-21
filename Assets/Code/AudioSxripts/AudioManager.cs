using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


public class AudioManager : MonoBehaviour
{
    private GameObject audiosPrefab;
    private List<Audios> audioPool = new List<Audios>();
    
    
    
    
    
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
                audioPool[i].gameObject.SetActive(true);
                audioPool[i].transform.parent = null;
                audioPool[i].transform.position = audioPosition;
                audioPool[i].isUsing = true;
                return true;
            }
            
        }
        return false;
    }

    private void OnDestroyPooledObject(Audios pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }

    private void PlayAudio()
    {

    }
}
