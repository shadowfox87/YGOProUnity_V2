//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO;



//public class BGMController : MonoBehaviour
//{
//    public enum WhatPlay { Menu, Disadvantage, Advantage, Duel }
//    public string soundPath;
//    public string menuPath;
//    public string disAdvPath;
//    public string advPath;
//    public string bgmPath;
//    private WhatPlay currentPlay = WhatPlay.Menu;
//    public List<string> bgms;
//    public AudioSource audioSource;
//    AudioClip audioClip;
//    private float multiplier;
//    // Use this for initialization
//    public void Start()
//    {
//        audioSource = gameObject.AddComponent<AudioSource>();
//        menuPath = soundPath = new System.Uri(new System.Uri("file:///"), Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song.ogg").ToString();
//        advPath = new System.Uri(new System.Uri("file:///"), Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song-advantage.mp3").ToString();
//        disAdvPath = new System.Uri(new System.Uri("file:///"), Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song-disadvantage.mp3").ToString();
//        bgmPath = Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/bgm/";
//        bgms = new List<string>();
//        bgms.AddRange(Directory.GetFiles(bgmPath));
//        //if (Program.I().setting != null && !Program.I().setting.isBGMMute.value)
//        //{
//        //    StartCoroutine(LoadBGM());
//        //}
//#if UNITY_IOS
//        multiplier=0.08f;
//#endif
//        multiplier = 0.8f;
//    }

//    // Update is called once per frame
//    public void Update()
//    {
//        if (Program.I().setting != null && Program.I().setting.isBGMMute.value)
//            return;
//        if (Program.I().ocgcore.isShowed)
//        {
//            int l0 = Program.I().ocgcore.life_0;
//            int l1 = Program.I().ocgcore.life_1;
//            if (l0 > l1 && l0 - l1 >= 4000 && currentPlay != WhatPlay.Advantage)
//            {
//                Play(WhatPlay.Advantage);
//            }
//            else if (l1 > l0 && l1 - l0 >= 4000 && currentPlay != WhatPlay.Disadvantage)
//            {
//                Play(WhatPlay.Disadvantage);
//            }
//            else if (currentPlay != WhatPlay.Duel)
//            {
//                Play(WhatPlay.Duel);
//            }
//        }
//        else if (currentPlay != WhatPlay.Menu)
//            Play(WhatPlay.Menu);
//        StartCoroutine(LoadBGM());
//    }

//    public void Play(WhatPlay what)
//    {
//        audioSource.Stop();
//        currentPlay = what;
//    }

//    public void changeBGMVol(float vol)
//    {
//        try
//        {
//            if (audioSource != null)
//            {
//                audioSource.volume = vol * multiplier;
//            }
//        }
//        catch { }

//    }

//    private IEnumerator LoadBGM()
//    {
//        WWW request = GetAudioFromFile();
//        yield return request;
//        audioClip = request.GetAudioClip(true, true);
//        audioClip.name = System.IO.Path.GetFileName(soundPath);
//        PlayAudioFile();
//    }

//    private void PlayAudioFile()
//    {
//        audioSource.clip = audioClip;
//        audioSource.volume = Program.I().setting.vol() * multiplier;
//        audioSource.loop = (currentPlay != WhatPlay.Duel || !Program.I().setting.rndBGM);
//        audioSource.Play();
//    }

//    private WWW GetAudioFromFile()
//    {
//        string pathToFile = "";
//        switch (currentPlay)
//        {
//            case WhatPlay.Menu:
//                {
//                    pathToFile = menuPath;
//                    break;
//                }
//            case WhatPlay.Advantage:
//                {
//                    pathToFile = advPath;
//                    break;
//                }
//            case WhatPlay.Disadvantage:
//                {
//                    pathToFile = disAdvPath;
//                    break;

//                }
//            case WhatPlay.Duel:
//                {
//                    pathToFile = bgms[new System.Random().Next(bgms.Count)];
//                    break;
//                }
//        }
//        WWW request = new WWW(pathToFile);
//        return request;
//    }
//}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLayer;

public class BGMController : MonoBehaviour
{
    public string soundPath;
    public AudioSource audioSource;
    AudioClip audioClip;
    AudioClip disAdvantage;
    AudioClip advantage;
    List<AudioClip> audioClips;
    private float multiplier;
    // Use this for initialization
    public void Start()
    {
        audioClips = new List<AudioClip>();
        string[] bgms = System.IO.Directory.GetFiles(Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/bgm/");
        foreach (string s in bgms)
        {
            if (System.IO.Path.GetExtension(s).ToLower() == ".mp3")
            {
                AudioClip add = Mp3Loader.LoadMp3(s);
                audioClips.Add(add);
            }
        }
        advantage = Mp3Loader.LoadMp3(Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song-advantage.mp3");
        disAdvantage = Mp3Loader.LoadMp3(Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song-disadvantage.mp3");
        audioSource = gameObject.AddComponent<AudioSource>();
        soundPath = new System.Uri(new System.Uri("file:///"), Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song.ogg").ToString();
        if (Program.I().setting != null && !Program.I().setting.isBGMMute.value)
        {
            StartCoroutine(LoadBGM());
        }
#if UNITY_IOS
        multiplier=0.08f;
#endif
        multiplier = 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Program.I().setting != null && Program.I().setting.isBGMMute.value)
            return;
        if (Program.I().ocgcore.isShowed)
        {
            int l0 = Program.I().ocgcore.life_0;
            int l1 = Program.I().ocgcore.life_1;
            if (l0 > l1 && l0 - l1 >= 4000)
            {
                if (audioSource.clip.name != advantage.name)
                    PlayAudioFile(1);
                return;
            }
            else if (l1 > l0 && l1 - l0 >= 4000)
            {
                if (audioSource.clip.name != disAdvantage.name)
                    PlayAudioFile(2);
                return;
            }
            else
            {
                if (!audioClips.Contains(audioSource.clip) || (Program.I().setting.rndBGM && !audioSource.isPlaying))
                {
                    PlayAudioFile(3);
                    return;
                }
            }
        }
        else if (audioSource.clip != audioClip)
            PlayAudioFile(0);
    }
    public void changeBGMVol(float vol)
    {
        try
        {
            if (audioSource != null)
            {
                audioSource.volume = vol * multiplier;
            }
        }
        catch { }

    }

    private IEnumerator LoadBGM()
    {
        WWW request = GetAudioFromFile(soundPath);
        yield return request;
        audioClip = request.GetAudioClip(true, true);
        audioClip.name = System.IO.Path.GetFileName(soundPath);
        PlayAudioFile(0);
    }

    private void PlayAudioFile(int what)
    {
        if (what == 0)
            audioSource.clip = audioClip;
        if (what == 1)
            audioSource.clip = advantage;
        if (what == 2)
            audioSource.clip = disAdvantage;
        if (what == 3)
        {
            if (audioClips.Contains(audioSource.clip) && !Program.I().setting.rndBGM)
                return;
            else
            {
                audioSource.clip = audioClips[new System.Random().Next(audioClips.Count)];
                audioSource.volume = Program.I().setting.vol() * multiplier;
                audioSource.loop = !Program.I().setting.rndBGM;
                audioSource.Play();
                return;
            }
        }
        audioSource.volume = Program.I().setting.vol() * multiplier;
        audioSource.loop = true;
        audioSource.Play();
    }

    private WWW GetAudioFromFile(string pathToFile)
    {
        WWW request = new WWW(pathToFile);
        return request;
    }

}
