using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }


    private Dictionary<SoundType, SoundTrack> _soundsTable;

    [SerializeField]
    private SoundTrack[] _soundTracks;

    [Serializable]
    private class SoundObject {
        public SoundType soundType;
        public AudioClip clip;
    }

    [Serializable]
    private class SoundTrack {
        public AudioSource audioSource;
        public SoundObject[] audio;
    }

    private void Awake() {
        if (Instance != null) {
            Destroy(this);
            return;
        }

        Instance = this;
        _soundsTable = new Dictionary<SoundType, SoundTrack>();

        foreach (SoundTrack track in _soundTracks) {
            foreach (SoundObject obj in track.audio) {
                if (!_soundsTable.ContainsKey(obj.soundType)) {
                    _soundsTable.Add(obj.soundType, track);
                }
            }
        }
    }

    private void Start() {
        PlaySound(SoundType.Music13, 0.75f);
    }
    public void PlaySound(SoundType sound, float volume) {
        foreach (var entry in _soundsTable) {
            if (entry.Key == sound) {
                AudioSource source = entry.Value.audioSource;
                foreach (var item in entry.Value.audio) {
                    if (item.soundType == sound) {
                        source.clip = item.clip;
                        source.volume = volume;
                        source.Play();
                        break;
                    }
                }

            }
        }
    }
}
