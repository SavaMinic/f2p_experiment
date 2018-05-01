using System.Collections;
using System.Collections.Generic;
using Nordeus.Util.CSharpLib;
using UnityEngine;

public class AudioController : MonoBehaviour
{

	#region Static

	private static AudioController instance;
	public static AudioController I
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<AudioController>();
			return instance;
		}
	}

	#endregion

	#region Fields

	[SerializeField]
	private List<AudioClip> backgroundAudioClips;

	[SerializeField]
	private AudioClip positiveAudioClip;

	[SerializeField]
	private AudioClip neutralAudioClip;

	[SerializeField]
	private AudioClip negativeAudioClip;

	[SerializeField]
	private AudioSource backgroundMusicAudioSource;

	[SerializeField]
	private AudioSource sfxAudioSource;

	private List<AudioClip> currentlyPlayingBackgroundMusic;
	private int backgroundMusicIndex = -1;

	private GoTween backgroundMusicTween;

	#endregion

	#region Mono

	private void Update()
	{
		if (!Application.isPlaying || backgroundMusicIndex == -1)
			return;

		if (!backgroundMusicAudioSource.isPlaying)
		{
			backgroundMusicAudioSource.clip = currentlyPlayingBackgroundMusic[backgroundMusicIndex];
			backgroundMusicIndex = (backgroundMusicIndex + 1) % backgroundAudioClips.Count;
			backgroundMusicAudioSource.Play();
		}
	}

	#endregion

	#region Public

	public void InitializeBackgroundMusic()
	{
		if (backgroundMusicAudioSource.isPlaying)
		{
			backgroundMusicAudioSource.Stop();
		}
		currentlyPlayingBackgroundMusic = new List<AudioClip>(backgroundAudioClips);
		currentlyPlayingBackgroundMusic.Shuffle();
		backgroundMusicIndex = 0;
	}

	public void SetBackgroundMusicVolume(float volume, float incrementalDuration = -1)
	{
		if (backgroundMusicTween != null && backgroundMusicTween.state == GoTweenState.Running)
		{
			backgroundMusicTween.destroy();
		}
		if (incrementalDuration <= 0)
		{
			backgroundMusicAudioSource.volume = volume;
		}
		else
		{
			backgroundMusicTween = Go.to(backgroundMusicAudioSource, incrementalDuration, new GoTweenConfig().floatProp("volume", volume));
		}
	}

	public void PlayPositiveSFX()
	{
		sfxAudioSource.PlayOneShot(positiveAudioClip);
	}

	public void PlayNeutralSFX()
	{
		sfxAudioSource.PlayOneShot(neutralAudioClip);
	}

	public void PlayNegativeSFX()
	{
		sfxAudioSource.PlayOneShot(negativeAudioClip);
	}

	#endregion


}
