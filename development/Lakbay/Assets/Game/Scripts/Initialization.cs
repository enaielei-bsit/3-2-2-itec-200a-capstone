/*
 * Date Created: Tuesday, November 30, 2021 11:28 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay {
    using Utilities;
    using Core;
    using QuestionRunner;
    using SteppedApplication.Blowbagets;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    using UnityEngine.Localization.Settings;
    using UnityEngine.SceneManagement;

    // [AddComponentMenu("Init")]
    public class Initialization : Controller {
        protected static bool _finished = false;
        public static bool finished => _finished;

        [SerializeField]
        protected LoadingScreen _loadingScreen; 
        [SerializeField]
        protected Database _database; 
        [SerializeField]
        protected Localizer _localizer; 
        [SerializeField]
        protected SceneController _sceneController;
        [SerializeField]
        protected AudioController _audioController;
        [SerializeField]
        protected CheatEngine _cheatEngine;

        public override void Awake() {
            base.Awake();
        }

        public new virtual IEnumerator Start() {
            if(finished) yield break;
            SceneManager.sceneLoaded += ResetTimeScale;

            Session.loadingScreen =
                _loadingScreen ? _loadingScreen.EnsureInstance() : default;
            if(Session.loadingScreen) {
                Session.loadingScreen.gameObject.MakePersistent();
                Session.loadingScreen.Show();
            }

            // Load Unity stuffs first...
            yield return LocalizationSettings.InitializationOperation;

            // Proceed with loading Game related stuffs...
            Session.database =
                _database ? _database.EnsureInstance() : default;
            if(Session.database) {
                Session.database.gameObject.MakePersistent();

                Session.database.Load<Sprite>();
                yield return new WaitWhile(() => Session.database.loading);

                // Session.database.Load<TextAsset>();
                // yield return new WaitWhile(() => Session.database.loading);

                // Session.database.Load<AudioClip>();
                // yield return new WaitWhile(() => Session.database.loading);

                Session.database.Load<QRLevel>();
                yield return new WaitWhile(() => Session.database.loading);

                Session.database.Load<SABBLevel>();
                yield return new WaitWhile(() => Session.database.loading);
            }

            Session.localizer = 
                _localizer ? _localizer.EnsureInstance() : default;
            if(Session.localizer && Session.database) {
                Session.localizer.gameObject.MakePersistent();

                var localizables = Session.database.Get<ILocalizable>();
                foreach(var localizable in localizables) {
                    localizable.Value.Localize(Session.localizer);
                }
            }

            Session.sceneController =
                _sceneController ? _sceneController.EnsureInstance() : default;
            if(Session.sceneController) {
                Session.sceneController.gameObject.MakePersistent();
            }

            Session.audioController =
                _audioController ? _audioController.EnsureInstance() : default;
            if(Session.audioController) {
                Session.audioController.gameObject.MakePersistent();
            }

            Session.cheatEngine =
                _cheatEngine ? _cheatEngine.EnsureInstance() : default;
            if(Session.cheatEngine) {
                Session.cheatEngine.gameObject.MakePersistent();
            }

            _finished = true;
            Session.loadingScreen?.Hide(Session.loadingScreen.hideDelay);
            // Destroy(gameObject);
        }

        public override void Update() {
            base.Update();
            if(Session.loadingScreen && Session.loadingScreen.showing) {
                if(!LocalizationSettings.InitializationOperation.IsDone) {
                    Session.loadingScreen.Show(
                        "Localization",
                        LocalizationSettings.InitializationOperation.PercentComplete
                    );
                }

                if(Session.database && Session.database.currentLocation != null) {
                    Session.loadingScreen.Show(
                        Session.database.currentLocation.PrimaryKey,
                        Mathf.Clamp(Session.database.loadingProgress, 0.0f, 1.0f)
                    );
                }
            }
        }

        protected static Action<QRLevel> OnQRLevelChange(int index) {
            return (l) =>  Session.qrLevels[index] = l;
        }

        public virtual void ResetTimeScale(Scene scene, LoadSceneMode mode) {
            Time.timeScale = 1.0f;
        }
    }
}