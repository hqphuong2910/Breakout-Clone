using System;
using System.Collections.Generic;
using System.Linq;
using _Project.ScriptableObjects.Scripts;
using _Project.Scripts.Enums;
using _Project.Scripts.Managers;
using _Project.Scripts.Patterns;
using _Project.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SettingScreen : Singleton<SettingScreen>
    {
        [Header("Dependencies")] [SerializeField]
        private GameConfigSO gameConfig;

        [SerializeField] private ControlConfigSO controlConfig;

        [Header("Audio UI Element(s)")] [SerializeField]
        private Slider masterVolumeSlider;

        [SerializeField] private Slider bgmVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        [Header("Performance UI Element(s)")] [SerializeField]
        private Toggle vSyncToggle;

        [SerializeField] private TMP_Dropdown fpsDropdown;

        [Header("Control UI Element(s)")] [SerializeField]
        private TMP_Dropdown controlMethodsDropdown;

        [Header("Action Button(s)")] [SerializeField]
        private Button saveButton;

        private SettingsData _currentSettings;

        #region UNITY_LIFECYCLE

        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _currentSettings = SaveSystem.Load<SettingsData>(SysConst.SettingsFileName);

            InitializeDropdowns();

            LoadUIValues();
            RegisterListeners();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnregisterListeners();
        }

        #endregion

        #region INITIALIZATION

        private void InitializeDropdowns()
        {
            if (fpsDropdown)
            {
                fpsDropdown.ClearOptions();
                var fpsOptions = new List<string>();
                foreach (var fpsName in Enum.GetNames(typeof(FPSLimit)))
                {
                    var displayName = fpsName.Replace("Limit", "");
                    if (displayName != nameof(FPSLimit.Unlimited)) displayName += " FPS";
                    fpsOptions.Add(displayName);
                }

                fpsDropdown.AddOptions(fpsOptions);
            }

            if (controlMethodsDropdown)
            {
                controlMethodsDropdown.ClearOptions();
                var methodOptions = Enum.GetNames(typeof(ControlMethods))
                    .Select(methodName => methodName.Replace("Or", " / ")).ToList();
                controlMethodsDropdown.AddOptions(methodOptions);
            }
        }

        private void LoadUIValues()
        {
            if (masterVolumeSlider) masterVolumeSlider.value = _currentSettings.masterVolume;
            if (bgmVolumeSlider) bgmVolumeSlider.value = _currentSettings.bgmVolume;
            if (sfxVolumeSlider) sfxVolumeSlider.value = _currentSettings.sfxVolume;

            if (vSyncToggle) vSyncToggle.isOn = _currentSettings.vSync;
            if (fpsDropdown)
            {
                var fpsArr = Enum.GetValues(typeof(FPSLimit));
                fpsDropdown.value = Array.IndexOf(fpsArr, _currentSettings.targetFPS);
            }

            if (controlMethodsDropdown)
            {
                var methodArr = Enum.GetValues(typeof(ControlMethods));
                controlMethodsDropdown.value = Array.IndexOf(methodArr, _currentSettings.controlMethods);
            }
        }

        private void RegisterListeners()
        {
            if (saveButton) saveButton.onClick.AddListener(SaveSettings);

            if (masterVolumeSlider) masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            if (bgmVolumeSlider) bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
            if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

            if (vSyncToggle) vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
            if (fpsDropdown) fpsDropdown.onValueChanged.AddListener(OnFPSChanged);

            if (controlMethodsDropdown) controlMethodsDropdown.onValueChanged.AddListener(OnControlTypeChanged);
        }

        private void UnregisterListeners()
        {
            if (saveButton) saveButton.onClick.RemoveListener(SaveSettings);

            if (masterVolumeSlider) masterVolumeSlider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
            if (bgmVolumeSlider) bgmVolumeSlider.onValueChanged.RemoveListener(OnBGMVolumeChanged);
            if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);

            if (vSyncToggle) vSyncToggle.onValueChanged.RemoveListener(OnVSyncChanged);
            if (fpsDropdown) fpsDropdown.onValueChanged.RemoveListener(OnFPSChanged);

            if (controlMethodsDropdown) controlMethodsDropdown.onValueChanged.RemoveListener(OnControlTypeChanged);
        }

        #endregion

        #region UI_CALLBACKS

        private void OnMasterVolumeChanged(float value)
        {
            _currentSettings.masterVolume = value;
            if (AudioManager.Instance) AudioManager.Instance.SetMasterVolume(value);
        }

        private void OnBGMVolumeChanged(float value)
        {
            _currentSettings.bgmVolume = value;
            if (AudioManager.Instance) AudioManager.Instance.SetBGMVolume(value);
        }

        private void OnSFXVolumeChanged(float value)
        {
            _currentSettings.sfxVolume = value;
            if (AudioManager.Instance) AudioManager.Instance.SetSFXVolume(value);
        }

        private void OnVSyncChanged(bool isOn)
        {
            _currentSettings.vSync = isOn;
            if (gameConfig) gameConfig.vSync = isOn;
            QualitySettings.vSyncCount = isOn ? 1 : 0;
        }

        private void OnFPSChanged(int index)
        {
            var fpsArr = (FPSLimit[])Enum.GetValues(typeof(FPSLimit));
            var newFps = fpsArr[index];

            _currentSettings.targetFPS = newFps;
            if (gameConfig) gameConfig.targetFPS = newFps;

            Application.targetFrameRate = newFps switch
            {
                FPSLimit.Unlimited => -1,
                FPSLimit.Limit30 => 30,
                FPSLimit.Limit60 => 60,
                FPSLimit.Limit90 => 90,
                FPSLimit.Limit120 => 120,
                FPSLimit.Limit144 => 144,
                FPSLimit.Limit165 => 165,
                FPSLimit.Limit180 => 180,
                FPSLimit.Limit240 => 240,
                _ => 60
            };
        }

        private void OnControlTypeChanged(int index)
        {
            var methodArr = (ControlMethods[])Enum.GetValues(typeof(ControlMethods));
            var newType = methodArr[index];

            _currentSettings.controlMethods = newType;
            if (controlConfig) controlConfig.controlMethods = newType;
        }

        private void SaveSettings()
        {
            if (_currentSettings == null) return;
            SaveSystem.Save(_currentSettings, SysConst.SettingsFileName);
        }

        #endregion
    }
}