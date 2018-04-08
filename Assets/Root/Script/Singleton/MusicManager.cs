using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	private static MusicManager _instance = null;
	public static MusicManager Instance {
		get {
			if (_instance == null) {
				GameObject obj = new GameObject("MusicManager");
				_instance = obj.AddComponent<MusicManager>();
				_audioSource = obj.AddComponent<AudioSource>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}


	private static AudioSource _audioSource = null;
	public  AudioSource GetAudioSource() {
		return _audioSource;
	}

	public void SetMusic(AudioClip ac) {

		_audioSource.clip = ac;
	}

	public void Play() {
		_audioSource.Play();
	}
}
