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
    using SteppedApplication;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    using UnityEngine.Localization.Settings;
    using UnityEngine.SceneManagement;

    public class Initialization : Controller {
        protected static bool _finished = false;
        public static bool finished => _finished;

        [SerializeField]
        protected LoadingScreen _loadingScreen; 
        [SerializeField]
        protected Database _database; 
        [SerializeField]
        protected Localizer _localizer; 

        public override void Awake() {
            base.Awake();
        }

        public new virtual IEnumerator Start() {
            if(finished) Destroy(gameObject);

            Session.loadingScreen =
                _loadingScreen ? _loadingScreen.CreateFirstInstance() : default;
            if(Session.loadingScreen) {
                Session.loadingScreen.gameObject.MakePersistent();
                Session.loadingScreen.Show("Loading...", 0.0f);
            }

            // Load Unity stuffs first...
            yield return LocalizationSettings.InitializationOperation;

            // Proceed with loading Game related stuffs...
            Session.database =
                _database ? _database.CreateFirstInstance() : default;
            if(Session.database) {
                Session.database.gameObject.MakePersistent();
                Session.database.Load<Sprite>();
                yield return new WaitWhile(() => Session.database.loading);

                Session.database.Load<QRLevel>();
                yield return new WaitWhile(() => Session.database.loading);
            }

            Session.localizer = 
                _localizer ? _localizer.CreateFirstInstance() : default;
            if(Session.localizer) {
                Session.localizer.gameObject.MakePersistent();
                var localizables = Session.database.Get<ILocalizable>();
                printLog(localizables.Count);
                foreach(var localizable in localizables) {
                    localizable.Value.Localize(Session.localizer);
                }

                Helper.TriggerLocaleChange();
            }

            _finished = true;
            Session.loadingScreen?.Hide();

            if(SceneManager.GetActiveScene().buildIndex == 0) {
                SceneManager.LoadScene(1);
            }

            Destroy(gameObject);
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
                        Session.database.loadingProgress
                    );
                }
            }
        }

        protected static Action<QRLevel> OnQRLevelChange(int index) {
            return (l) =>  Session.qrLevels[index] = l;
        }
    }
}