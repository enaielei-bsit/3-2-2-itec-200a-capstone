/*
 * Date Created: Friday, January 7, 2022 1:55 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    using UnityEngine.Events;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Settings;
    using UnityEngine.Video;
    using Utilities;

    public class HelpUI : Controller
    {
        protected UnityAction _onClose = null;

        [Header("Files")]
        public LocalizedAsset<TextAsset> modesFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> questionRunnerFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> steppedApplicationFile = new LocalizedAsset<TextAsset>();

        [Space]
        public LocalizedAsset<TextAsset> blowbagetsFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> parallelParkingFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> perpendicularParkingFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> backInAngleParkingFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> threePointTurnFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> tailgatingFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> rightOfWayFile = new LocalizedAsset<TextAsset>();
        public LocalizedAsset<TextAsset> trafficSignalRulesFile = new LocalizedAsset<TextAsset>();

        [Header("Videos")]
        public VideoClip modesVideo;
        public VideoClip questionRunnerVideo;
        public VideoClip steppedApplicationVideo;

        [Space]
        public VideoClip blowbagetsVideo;
        public VideoClip parallelParkingVideo;
        public VideoClip perpendicularParkingVideo;
        public VideoClip backInAngleParkingVideo;
        public VideoClip threePointTurnVideo;
        public VideoClip tailgatingVideo;
        public VideoClip rightOfWayVideo;
        public VideoClip trafficSignalRulesVideo;

        [Header("Controls")]
        public Toggle modes;
        public Toggle questionRunner;
        public Toggle steppedApplication;

        [Space]
        public Toggle blowbagets;
        public Toggle parallelParking;
        public Toggle perpendicularParking;
        public Toggle backInAngleParking;
        public Toggle threePointTurn;
        public Toggle tailgating;
        public Toggle rightOfWay;
        public Toggle trafficSignalRules;

        [Space]
        public ToggleGroup controlGroup;

        [Header("Viewing")]
        public RectTransform root;
        public VideoPlayer video;
        public Content content;

        [Space]
        public VideoViewer videoViewer;

        public virtual void OnLocaleChange(Locale locale)
        {
            var active = controlGroup.GetFirstActiveToggle();
            if (active && gameObject.activeSelf)
            {
                var clip = video?.clip;
                active.onValueChanged.Invoke(active.isOn);
                if (clip)
                {
                    video.transform.parent.gameObject.SetActive(true);
                    video.clip = clip;
                }
            }
        }

        public new virtual IEnumerator Start()
        {
            yield return new WaitUntil(() => Initialization.finished);
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChange;
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChange;
        }

        public override void Awake()
        {
            base.Awake();
            modes?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(modesFile.LoadAsset(), modesVideo);
            });
            steppedApplication?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(steppedApplicationFile.LoadAsset(), steppedApplicationVideo);
            });
            questionRunner?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(questionRunnerFile.LoadAsset(), questionRunnerVideo);
            });

            blowbagets?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(blowbagetsFile.LoadAsset(), blowbagetsVideo);
            });
            parallelParking?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(parallelParkingFile.LoadAsset(), parallelParkingVideo);
            });
            perpendicularParking?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(perpendicularParkingFile.LoadAsset(), perpendicularParkingVideo);
            });
            backInAngleParking?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(backInAngleParkingFile.LoadAsset(), backInAngleParkingVideo);
            });
            threePointTurn?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(threePointTurnFile.LoadAsset(), threePointTurnVideo);
            });
            tailgating?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(tailgatingFile.LoadAsset(), tailgatingVideo);
            });
            rightOfWay?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(rightOfWayFile.LoadAsset(), rightOfWayVideo);
            });
            trafficSignalRules?.onValueChanged.AddListener((v) =>
            {
                if (!v) return;
                Build(trafficSignalRulesFile.LoadAsset(), trafficSignalRulesVideo);
            });
        }

        public virtual void Build(TextAsset asset, VideoClip clip = null,
            bool hideVideoIfNull = true)
        {
            root?.gameObject.SetActive(true);
            if (video && clip)
            {
                video.transform.parent.gameObject.SetActive(true);
                video.clip = clip;
                var button = video.gameObject.EnsureComponent<Button>();
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => videoViewer?.Show(
                    clip, null, null
                ));
            }
            else if (hideVideoIfNull)
                video.transform.parent.gameObject.SetActive(false);
            if (content && asset) content?.Build(
                 asset.text.DeserializeAsYaml<List<Entry>>());
        }

        public virtual void ClearSelected()
        {
            controlGroup?.SetAllTogglesOff();
        }

        public virtual void BackToMenu()
        {
            root?.gameObject.SetActive(false);
            ClearSelected();
        }

        public virtual void LaunchCurrent() => LaunchCurrent(null);

        public virtual void LaunchCurrent(UnityAction onClose=null)
        {
            var current = SceneController.GetCurrent();
            Toggle toggle = null;
            switch (current)
            {
                case BuiltScene.QuestionRunner:
                    toggle = questionRunner;
                    break;
                case BuiltScene.Blowbagets:
                    toggle = blowbagets;
                    break;
                case BuiltScene.ParallelParking:
                    toggle = parallelParking;
                    break;
                case BuiltScene.PerpendicularParking:
                    toggle = perpendicularParking;
                    break;
                case BuiltScene.BackInAngleParking:
                    toggle = backInAngleParking;
                    break;
                case BuiltScene.ThreePointTurn:
                    toggle = threePointTurn;
                    break;
                case BuiltScene.Tailgating:
                    toggle = tailgating;
                    break;
                case BuiltScene.RightOfWay:
                    toggle = rightOfWay;
                    break;
                case BuiltScene.TrafficSignalRules:
                    toggle = trafficSignalRules;
                    break;
            }

            gameObject.SetActive(true);
            if (toggle) toggle.isOn = true;

            _onClose = onClose;
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
            var onClose = _onClose;
            _onClose = null;
            onClose?.Invoke();
        }
    }
}