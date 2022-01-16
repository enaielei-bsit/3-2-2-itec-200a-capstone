/*
 * Date Created: Wednesday, November 24, 2021 6:32 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Ph.CoDe_A.Lakbay
{
    using Core;
    using QuestionRunner;
    using SteppedApplication.Blowbagets;
    using Utilities;

    public class Session : Controller
    {
        public static Database database;
        public static Localizer localizer;
        public static LoadingScreen loadingScreen;
        public static SceneController sceneController;
        public static AudioController audioController;
        public static SettingsController settingsController;
        public static CheckpointController checkpointController;
        public static CheatEngine cheatEngine;

        public static GameMode mode = GameMode.NonPro;


        public static readonly List<QRLevel> qrLevels = new List<QRLevel>();
        private static int _qrLevelIndex = 0;
        public static int qrLevelIndex
        {
            get => qrLevels.Count > 0 ? _qrLevelIndex : -1;
            set => _qrLevelIndex = Mathf.Clamp(value, 0, qrLevels.Count - 1);
        }
        public static QRLevel qrLevel => qrLevelIndex.Within(0, qrLevels.Count - 1)
            ? qrLevels[qrLevelIndex] : default;
        public static int qrScore => qrLevels.Sum((l) => l.score);
        public static int qrMaxScore => qrLevels.Sum((l) => l.maxScore);
        public static int qrPassingScore => qrLevels.Sum((l) => l.passingScore);
        public static bool qrPassed => qrLevels.All((l) => l.passed);


        public static SABBLevel sabbLevel;

        public static void SetMode(int value) => mode = (GameMode)value;
    }
}