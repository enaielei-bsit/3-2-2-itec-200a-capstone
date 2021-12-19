/*
 * Date Created: Wednesday, November 24, 2021 6:32 AM
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

    public class Session : Controller {
        public static Database database;
        public static Localizer localizer;
        public static LoadingScreen loadingScreen;
        public static SceneController sceneController;
        public static CheatEngine cheatEngine;

        public static GameMode mode = GameMode.NonPro;


        public static readonly List<QRLevel> qrLevels = new List<QRLevel>();
        private static int _qrLevelIndex = 0;
        public static int qrLevelIndex {
            get => qrLevels.Count > 0 ? _qrLevelIndex : -1;
            set => _qrLevelIndex = Mathf.Clamp(value, 0, qrLevels.Count - 1);
        }
        public static QRLevel qrLevel => qrLevelIndex.Within(0, qrLevels.Count - 1)
            ? qrLevels[qrLevelIndex] : default;
        public static int qrScore => qrLevels.Sum((l) => l.score);
        public static int qrMaxScore => qrLevels.Sum((l) => l.maxScore);
        public static float qrPassingScorePercentage = 0.75f;
        public static int qrPassingScore =>
            (int) (qrPassingScorePercentage * qrMaxScore);
        public static bool qrPassed => qrScore >= qrPassingScore;


        public static SABBLevel sabbLevel;

        public static void SetMode(int value) => mode = (GameMode) value;
    }
}