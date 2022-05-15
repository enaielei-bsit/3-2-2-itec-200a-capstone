/*
 * Date Created: Thursday, December 23, 2021 1:22 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.ParallelParking
{
    using Core;
    using UnityEngine.Localization;

    [AddComponentMenu("SA Parallel Parking Player")]
    public class SAPPPlayer : SAVehicleParkingPlayer
    {
        [Header("Messages")]
        public LocalizedString parkedCorrectly;
        public LocalizedString didntHitCar;
        public LocalizedString hitCar;

        [Space]
        public RectTransform gameFinishedUI;

        public override void Proceed()
        {
            // LoadNextScene();
            if (gameFinishedUI)
            {
                gameFinishedUI.gameObject.SetActive(true);
            }
            else LoadScene(BuiltScene.MainMenu);
        }

        public override void OnPark()
        {
            base.OnPark();
            Session.checkpointController.Clear();
            Session.checkpointController.Save();
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