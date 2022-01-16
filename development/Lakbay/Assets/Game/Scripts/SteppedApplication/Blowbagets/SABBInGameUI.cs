/*
 * Date Created: Sunday, December 19, 2021 6:14 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using UnityEngine;

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Blowbagets
{
    using Core;

    public class SABBInGameUI : InGameUI
    {
        [Space]
        public SABBPlayer player;
        public float minAlpha = 0.25f;

        [Header("Blowbagets")]
        public CanvasGroup battery;
        public CanvasGroup lights;
        public CanvasGroup oil;
        public CanvasGroup water;
        public CanvasGroup brakes;
        public CanvasGroup air;
        public CanvasGroup gas;
        public CanvasGroup engine;
        public CanvasGroup tires;
        public CanvasGroup self;

        public override void Update()
        {
            base.Update();
            if (player)
            {
                if (battery) battery.alpha = player.battery ? minAlpha : 1.0f;
                if (lights) lights.alpha = player.lights ? minAlpha : 1.0f;
                if (oil) oil.alpha = player.oil ? minAlpha : 1.0f;
                if (water) water.alpha = player.water ? minAlpha : 1.0f;
                if (brakes) brakes.alpha = player.brakes ? minAlpha : 1.0f;
                if (air) air.alpha = player.air ? minAlpha : 1.0f;
                if (gas) gas.alpha = player.gas ? minAlpha : 1.0f;
                if (engine) engine.alpha = player.engine ? minAlpha : 1.0f;
                if (tires) tires.alpha = player.tires ? minAlpha : 1.0f;
                if (self) self.alpha = player.self ? minAlpha : 1.0f;
            }
        }
    }
}