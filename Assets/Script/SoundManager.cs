using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None,
    NomalButton, // 기본 버튼음,
    CatSound,
    Result,
    PrefectFood,
    PrefectMeow,
    FailMeow,
    Pickup,
    Count,
    Feever,





}

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance;

    public AudioSource bgm;

    public AudioClip clipNomalButton;

    public AudioClip clipCatSound;

    public AudioClip clipResultSound;


    public AudioClip clipPrefectFood;
    public AudioClip clipPrefectMeow;
    public AudioClip clipFailMeow;
    public AudioClip clipPickup;
    public AudioClip clipCount;

    public AudioClip clipFeever;

    void Awake()
    {
        Instance = this;
    }

    public static void BGM_Play()
    {
        Instance.bgm.Play();
    }
    public static void BGM_Stop()
    {
        Instance.bgm.Stop();
    }
       

    public static void BGM_Mute(bool isMute)
    {
        Instance.bgm.mute = isMute;
    }

    public static void Play(SoundType soundType)
    {
        if (OptionManager.Instance.isEffectSound == false)
            return;

        if (soundType == SoundType.Result)
        {
            Instance.bgm.mute = true;
            Instance.sound(soundType).Play();
            Instance.StartCoroutine(Instance.ResultSoundWait(Instance.sound(soundType)));
        }
        else if(soundType == SoundType.Feever)
        {
           
            if (Instance.feeverCoroutine == null)
            {
                Instance.bgm.mute = true;
                Instance.feeverCoroutine = Instance.StartCoroutine(Instance.FeeverSoundWait(Instance.sound(soundType)));
            }
        }
        else
            Instance.sound(soundType).Play();
    }
    Coroutine feeverCoroutine;
    IEnumerator FeeverSoundWait(AudioSource audio)
    {
        audio.Play();
        while (GameController.Instance.isFeever)
            yield return null;
       
        feeverCoroutine = null;
        audio.Stop();
        bgm.mute = false;

    }

    IEnumerator ResultSoundWait(AudioSource audio)
    {
        while (audio.isPlaying)
            yield return null;

        bgm.mute = false;
    }
    public int poolingCount = 5;


    public Dictionary<SoundType, Queue<AudioSource>> stackDic = new Dictionary<SoundType, Queue<AudioSource>>();


    AudioSource sound(SoundType soundType)
    {
        AudioSource audioSource = null;

        if(stackDic.ContainsKey(soundType))
        {
            for (int i = 0; i < stackDic[soundType].Count; i++)
            {
                AudioSource a = stackDic[soundType].Dequeue();
                if (a == null)
                    break;

                stackDic[soundType].Enqueue(a);
                if (a.isPlaying == false)
                {
                    audioSource = a;                    
                    break;                    
                }
            }
            // 최대 갯수라면 가장 오래된 것 부터 사용
            if(stackDic[soundType].Count >= poolingCount)
            {
                AudioSource a = stackDic[soundType].Dequeue();
                stackDic[soundType].Enqueue(a);
                audioSource = a;
                //Debug.Log(soundType.ToString() + "재활용");
            }
        }
        else
        {
            Queue<AudioSource> queue = new Queue<AudioSource>();
            stackDic.Add(soundType, queue);
        }

        if(audioSource == null)
        {
            GameObject go = new GameObject(soundType.ToString());
            go.transform.SetParent(transform);
            audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = Clip(soundType);
            stackDic[soundType].Enqueue(audioSource);
        }
        

        return audioSource;
    }


    AudioClip Clip(SoundType soundType)
    {
        AudioClip clip = null;

        switch (soundType)
        {
            case SoundType.None:
                break;
            case SoundType.NomalButton:
                clip = clipNomalButton;
                break;
            case SoundType.CatSound:
                clip = clipCatSound;
                break;
            case SoundType.Result:
                clip = clipResultSound;
                break;
            case SoundType.PrefectFood:
                clip = clipPrefectFood;
                break;
            case SoundType.PrefectMeow:
                clip = clipPrefectMeow;
                break;
            case SoundType.FailMeow:
                clip = clipFailMeow;
                break;
            case SoundType.Pickup:
                clip = clipPickup;
                break;
            case SoundType.Count:
                clip = clipCount;
                break;
            case SoundType.Feever:
                clip = clipFeever;
                break;
            default:
                break;
        }
    
        return clip;
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.S))
        //{
        //    Play(SoundType.BookUnfoldLight);
        //}
    }

}
