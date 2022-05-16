/*
 * Date Created: Tuesday, January 4, 2022 9:00 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ThreePointTurn
{
    using Core;
    using UnityEngine.Localization;

    public class SATPTPlayer : SASteppedVehiclePlayer
    {
        [Header("Messages")]
        public LocalizedString performedThreePoint;

        public override void Proceed()
        {
            LoadNextScene();
        }

        public override void OnPark()
        {
            base.OnPark();
            Session.checkpointController?.SaveCheckpoint(new Checkpoint(
                Session.mode,
                Session.transmission,
                SceneController.GetNext())
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