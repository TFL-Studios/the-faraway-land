using UnityEngine;
using UnityEngine.Pool;


public class AudioManager : MonoBehaviour
{
    private IObjectPool<AudioSource> audioPool;
    public IObjectPool<AudioSource> AudioPool { set => audioPool = value; }
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool poolCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    
    private void Awake()
    {
        audioPool = new AudioPool<AudioSource>(poolCheck, defaultCapacity);
    }
    
    private AudioSource CreateAudio()
    {
        AudioSource audioSourceInstantiate = Instantiate(audioSource);
        audioSourceInstantiate.AudioPool = audioPool;
        return audioSourceInstantiate;
    }

    private void OnReleaseToPool(AudioSource pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
        pooledObject.transform.parent = this.transform;
        pooledObject.transform.localPosition = Vector3.zero;
    }

    private void OnGetFromPool(AudioSource pooledObject, Vector3 audioPosition)
    {
        pooledObject.gameObject.SetActive(true);
        pooledObject.transform.parent = null;
        pooledObject.transform.position = audioPosition;
    }

    private void OnDestroyPooledObject(AudioSource pooledObject)
    {
        Destroy(pooledObject.gameObject);
    }
}
