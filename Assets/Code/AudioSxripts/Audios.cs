using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Audios : MonoBehaviour
{
    private IObjectPool<AudioSource> audioPool;
    public IObjectPool<AudioSource> AudioPool { set => audioPool = value; }


}
