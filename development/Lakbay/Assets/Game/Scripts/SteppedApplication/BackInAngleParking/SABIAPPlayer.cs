/*
 * Date Created: Monday, January 3, 2022 3:56 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.BackInAngleParking
{
    using Core;
    using UnityEngine.Localization;

    public class SABIAPPlayer : SAVehicleParkingPlayer
    {
        [Header("Messages")]
        public LocalizedString parkedCorrectly;
        public LocalizedString didntHitCar;
        public LocalizedString hitCar;

        public override void Proceed()
        {
            LoadNextScene();
        }

        public override void OnPark()
        {
            base.OnPark();
            Session.checkpointController?.SaveCheckpoint(new Checkpoint(
                Session.mode, SceneController.GetNext())
            );
            gameOverUI?.ShowPassed(
                parkedCorrectly, didntHitCar
            );
        }

        public override void OnObstacleHit()
        {
            base.OnObstacleHit();
            gameOverUI?.ShowFailed(
                hitCar
            );
        }
    }
}