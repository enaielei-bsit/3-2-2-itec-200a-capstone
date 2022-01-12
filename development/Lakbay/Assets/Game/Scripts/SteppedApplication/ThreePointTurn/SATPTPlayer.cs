/*
 * Date Created: Tuesday, January 4, 2022 9:00 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ThreePointTurn {
    using Utilities;
    using Core;
    using UnityEngine.Localization;

    public class SATPTPlayer : SASteppedVehiclePlayer {
        [Header("Messages")]
        public LocalizedString performedThreePoint;

        public override void Proceed() {
            LoadScene(BuiltScene.Tailgating);
        }

        public override void OnPark() {
            base.OnPark();
            Session.checkpointController?.SaveCheckpoint(
                new Checkpoint(Session.mode, BuiltScene.Tailgating)
            );
            gameOverUI?.ShowPassed(
                performedThreePoint
            );
        }

        // public override void OnObstacleHit() {
        //     base.OnObstacleHit();
        //     gameOverUI?.ShowFailed(
        //         hitCar
        //     );
        // }
    }
}