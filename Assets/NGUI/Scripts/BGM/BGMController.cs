using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour {
    public string soundPath;
    public AudioSource audioSource;
    AudioClip audioClip;
    private float multiplier;
    // Use this for initialization
    public void Start() {

        audioSource = gameObject.AddComponent<AudioSource>();
        soundPath = new System.Uri(new System.Uri("file:///"),Environment.CurrentDirectory.Replace("\\", "/") + "/" +"sound/bgm/song.ogg").ToString();
        if (!Program.I().setting.isBGMMute.value)
        {
            StartCoroutine(LoadBGM());
        }
#if UNITY_IOS
        multiplier=0.08f;
#endif
        multiplier = 0.08f;
    }

    // Update is called once per frame
    void Update() {
    }
    public void changeBGMVol(float vol)
    {
        try
        {
            if (audioSource != null)
            {
                audioSource.volume = vol*multiplier;
            }
        }
        catch { }

    }
    private IEnumerator LoadBGM()
    {
        WWW request = GetAudioFromFile(soundPath);
        yield return request;
        audioClip = request.GetAudioClip(true,true);
        audioClip.name = System.IO.Path.GetFileName(soundPath);
        PlayAudioFile();
    }

    private void PlayAudioFile()
    {
        audioSource.clip = audioClip;
        audioSource.volume = Program.I().setting.vol()*multiplier;
        audioSource.loop = true;
        audioSource.Play();
    }

    private WWW GetAudioFromFile(string pathToFile)
    {
        WWW request = new WWW(pathToFile);
        return request;
    }
}
