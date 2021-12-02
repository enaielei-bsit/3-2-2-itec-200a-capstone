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
    using Core;
    using QuestionRunner;
    using SteppedApplication;
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    using UnityEngine.Localization.Settings;

    public class Initialization : Controller {
        protected static bool _finished = false;
        public static bool finished => _finished;

        public override void Awake() {
            base.Awake();
            if(finished) return;
        }

        public new virtual IEnumerator Start() {
            if(finished) yield break;

            yield return LocalizationSettings.InitializationOperation;

            Session.database = FindObjectOfType<Database>();
            if(Session.database) {
                Session.database.Load<Sprite>();
                yield return new WaitUntil(() => Session.database.loading);

                Session.database.Load<QRLevel>();
                yield return new WaitUntil(() => Session.database.loading);
            }

            _finished = true;
            printLog("finished");
        }

        public override void Update() {
            base.Update();
        }

        protected static Action<QRLevel> OnQRLevelChange(int index) {
            return (l) =>  Session.qrLevels[index] = l;
        }
    }
}