/*
 * Date Created: Sunday, January 2, 2022 8:28 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.PerpendicularParking
{
    using Core;
    using UnityEngine.Localization;

    [AddComponentMenu("SA Perpendicular Parking Player")]
    public class SAPPPlayer : SAVehicleParkingPlayer
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