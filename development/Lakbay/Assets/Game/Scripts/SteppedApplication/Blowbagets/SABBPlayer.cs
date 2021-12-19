/*
 * Date Created: Tuesday, November 30, 2021 12:50 PM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.Blowbagets {
    using Cinemachine;
    using Core;
    using UnityEngine.Localization;
    using Utilities;

    [Serializable]
    public class BlowbagetsInfo {
        public string title = "";
        public string image = "";
        public List<Entry> content = new List<Entry>();
    }

    public class SABBPlayer : SAPlayer {
        protected Vector3 _baseFollowOffset;

        [Header("Level")]
        public new CinemachineVirtualCamera camera;
        public BlowbagetsUI blowbagetsUI;
        public virtual CinemachineOrbitalTransposer transposer =>
            camera?.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        [Header("Gameplay")]
        public float maxScale = 1.0f;
        public float maxRotation = 360.0f;
        
        [Space]
        public bool battery = false;
        [Min(0)]
        public int maxLightsCount = 4;
        [Min(0)]
        public int lightsCount = 0;
        public virtual bool lights => lightsCount >= maxLightsCount;
        public bool oil = false;
        public bool water = false;
        public bool brakes = false;
        public bool air = false;
        public bool gas = false;
        public bool engine = false;
        [Min(0)]
        public int maxTiresCount = 4;
        [Min(0)]
        public int tiresCount = 0;
        public virtual bool tires => tiresCount >= maxTiresCount;
        public bool self = false;
        public virtual bool finished {
            get {
                return new bool[] {
                    battery, lights, oil, water,
                    brakes, air, gas, engine,
                    tires, self
                }.All();
            }
        }

        public override void Build() {
            base.Build();
            Session.sabbLevel = Session.database.Get<SABBLevel>().First().Value;
            Session.localizer.Subscribe<TextAsset, LocalizeTextAssetEvent>(
                Session.sabbLevel.batteryFile, UpdateBlowbagetsInfo);

            _baseFollowOffset = transposer.m_FollowOffset;
        }
        
        public virtual void Rotate(float factor) {
            if(camera) {
                transposer.m_XAxis.Value = maxRotation * factor;
                printLog($"Rotating: {transposer.m_XAxis.Value}");
            }
        }

        public virtual void Scale(float factor) {
            if(camera) {
                transposer.m_FollowOffset = _baseFollowOffset + (Vector3.forward * (maxScale * factor));
                printLog($"Scaling: {transposer.m_FollowOffset.z}");
            }
        }

        public virtual void ShowBattery() {
            battery = true;
            ShowInfo(Session.sabbLevel.battery);
        }

        public virtual void ShowLights() {
            lightsCount = Mathf.Clamp(lightsCount + 1, 0, maxLightsCount);
            if(lights) ShowInfo(Session.sabbLevel.lights);
        }

        public virtual void ShowOil() {
            oil = true;
            ShowInfo(Session.sabbLevel.oil);
        }

        public virtual void ShowWater() {
            water = true;
            ShowInfo(Session.sabbLevel.water);
        }

        public virtual void ShowBrakes() {
            brakes = true;
            ShowInfo(Session.sabbLevel.brakes);
        }

        public virtual void ShowAir() {
            air = true;
            ShowInfo(Session.sabbLevel.air);
        }

        public virtual void ShowGas() {
            gas = true;
            ShowInfo(Session.sabbLevel.gas);
        }

        public virtual void ShowEngine() {
            engine = true;
            ShowInfo(Session.sabbLevel.engine);
        }

        public virtual void ShowTires() {
            tiresCount = Mathf.Clamp(tiresCount + 1, 0, maxTiresCount);
            if(tires) ShowInfo(Session.sabbLevel.tires);
        }

        public virtual void ShowSelf() {
            self = true;
            ShowInfo(Session.sabbLevel.self);
        }

        public virtual void ShowInfo(TextAsset asset) {
            if(asset) ShowInfo(asset.ToString().DeserializeAsYaml<BlowbagetsInfo>());
        }

        public virtual void ShowInfo(BlowbagetsInfo info) {
            if(blowbagetsUI) {
                blowbagetsUI.Build(info);
            }
        }

        public virtual void UpdateBlowbagetsInfo(TextAsset info) {
            if(info && blowbagetsUI && blowbagetsUI.gameObject.activeSelf) {
                blowbagetsUI.Build();
            }
        }
    }
}