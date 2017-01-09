using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    class ScreenSetting : MonoBehaviour, IResponsive
    {
        ScreenSetting() { }

        [SerializeField]
        Responser responser;

        [SerializeField]
        Dropdown resolutionDropdown;

        [SerializeField]
        Toggle fullScreenToggle;

        List<Resolution> resolutions;

        List<string> strResolutions;

        int currentResolutionIndex;

        void Start()
        {
            InitResolutionInfo();
            InitFullScreenInfo();
        }

        void InitResolutionInfo()
        {
            resolutions = Screen.resolutions.ToList();
            strResolutions = resolutions.Select(resolution => resolution.ToString()).ToList();
            currentResolutionIndex = resolutions.FindIndex(item => item.Equals(Screen.currentResolution));

            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(strResolutions);
            resolutionDropdown.value = currentResolutionIndex;

            resolutionDropdown.onValueChanged.AddListener(OnResolutionDropdownChanged);
        }

        void InitFullScreenInfo()
        {
            fullScreenToggle.isOn = Screen.fullScreen;
            fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggleChanged);
        }


        void OnResolutionDropdownChanged(int index)
        {
            responser.SubscribeApply(this);
        }

        void OnFullScreenToggleChanged(bool isOn)
        {
            responser.SubscribeApply(this);
        }


        void IResponsive.OnApply()
        {
            int targetResolutionIndex = resolutionDropdown.value;

            Resolution resolution = resolutions[targetResolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, fullScreenToggle.isOn, resolution.refreshRate);
            currentResolutionIndex = targetResolutionIndex;
        }

        void IResponsive.OnReset()
        {
            resolutionDropdown.value = currentResolutionIndex;
            fullScreenToggle.isOn = Screen.fullScreen;
        }

    }

}
