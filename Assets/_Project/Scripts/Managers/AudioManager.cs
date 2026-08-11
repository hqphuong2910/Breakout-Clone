using _Project.Scripts.Events;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Audio Mixer")] [SerializeField]
        private AudioMixer audioMixer;

        [Header("Exposed Parameters")] [SerializeField]
        private string masterVolume = "MasterVolume";

        [SerializeField] private string BGMVolume = "BGMVolume";
        [SerializeField] private string SFXVolume = "SFXVolume";

        [Header("Audio Sources")] [SerializeField]
        private AudioSource BGMSource;

        [SerializeField] private AudioSource SFXSource;

        #region INITIALIZATION

        protected override void LoadComponents()
        {
            base.LoadComponents();

            LoadAudioSource();
        }

        private void LoadAudioSource()
        {
            LoadBGMSource();
            LoadSFXSource();
        }

        private void LoadBGMSource()
        {
            if (BGMSource) return;
            BGMSource = GetComponent<AudioSource>();

            AppLogger.Log(this, "Successfully loaded BGM audio source.");
        }

        private void LoadSFXSource()
        {
            if (SFXSource) return;
            SFXSource = gameObject.AddComponent<AudioSource>();

            AppLogger.Log(this, "Successfully loaded SFX audio source.");
        }

        #endregion

        #region EVENT_HANDLERS

        protected override void SubscribeEvents()
        {
            base.SubscribeEvents();

            AudioEvents.OnPlayBGM += PlayMusic;
            AudioEvents.OnStopBGM += StopMusic;
            AudioEvents.OnPlaySFX += PlaySFX;
        }

        protected override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            AudioEvents.OnPlayBGM -= PlayMusic;
            AudioEvents.OnStopBGM -= StopMusic;
            AudioEvents.OnPlaySFX -= PlaySFX;
        }

        #endregion

        #region AUDIO_HANDLERS

        #region PLAYERS

        private void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (!clip) return;
            BGMSource.clip = clip;
            BGMSource.loop = loop;
            BGMSource.Play();

            AppLogger.Log(this, $"Playing BGM: {clip.name}.");
        }

        private void StopMusic()
        {
            BGMSource.Stop();
        }

        private void PlaySFX(AudioClip clip)
        {
            if (!clip) return;
            SFXSource.PlayOneShot(clip);
        }

        #endregion

        #region VOLUME_HANDLERS

        private void SetVolume(string paramName, float sliderValue)
        {
            var clampedValue = Mathf.Clamp(sliderValue, 0.0001f, 1);
            audioMixer.SetFloat(paramName, Mathf.Log10(clampedValue) * 20);
        }

        public void SetMasterVolume(float sliderValue)
        {
            SetVolume(masterVolume, sliderValue);
        }

        public void SetBGMVolume(float sliderValue)
        {
            SetVolume(BGMVolume, sliderValue);
        }

        public void SetSFXVolume(float sliderValue)
        {
            SetVolume(SFXVolume, sliderValue);
        }

        #endregion

        #endregion
    }
}