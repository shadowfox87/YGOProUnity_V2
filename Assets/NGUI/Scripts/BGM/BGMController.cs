using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NLayer;// Loads mp3 files


public class BGMController : MonoBehaviour
{
	public AudioSource audioSource;
	AudioClip menuClip;
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
		menuClip = Mp3Loader.LoadMp3(Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song.mp3");
		audioSource = gameObject.AddComponent<AudioSource>();
		#if UNITY_IOS
		multiplier=0.08f;
		#endif
		multiplier = 0.8f;
		//if (Program.I().setting != null && !Program.I().setting.isBGMMute.value)
		//{
		//    PlayAudioFile(0);
		//}
	}

	// Update is called once per frame
	void Update()
	{
		if (!audioSource.isPlaying)
			PlayWhat();
	}

	public void PlayWhat()
	{
		if (Program.I().setting == null || Program.I().setting.isBGMMute.value)
			return;
		if (Program.I().ocgcore != null && Program.I().ocgcore.isShowed)
		{
			int l0 = Program.I().ocgcore.life_0;
			int l1 = Program.I().ocgcore.life_1;
			if (l0 != 0 && l1 != 0)
			if (l0 > l1 && l0 / l1 >= 2)
			{
				if (audioSource.clip.name != advantage.name)
					PlayAudioFile(1);
				return;
			}
			else if (l1 > l0 && l1 / l0 >= 2)
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
		else if (audioSource.clip != menuClip)
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
		string soundPath = new System.Uri(new System.Uri("file:///"), Environment.CurrentDirectory.Replace("\\", "/") + "/" + "sound/song.ogg").ToString();
		WWW request = GetAudioFromFile(soundPath);
		yield return request;
		menuClip = request.GetAudioClip(true, true);
		menuClip.name = System.IO.Path.GetFileName(soundPath);
		PlayAudioFile(0);
	}

	private void PlayAudioFile(int what)
	{
		audioSource.volume = Program.I().setting.vol() * multiplier;
		if (what == 0)
			audioSource.clip = menuClip;
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

				audioSource.loop = !Program.I().setting.rndBGM;
				audioSource.Play();
				return;
			}
		}
		audioSource.loop = true;
		audioSource.Play();
	}

	private WWW GetAudioFromFile(string pathToFile)
	{
		WWW request = new WWW(pathToFile);
		return request;
	}

}