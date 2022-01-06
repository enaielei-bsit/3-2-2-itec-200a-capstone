/*
 * Date Created: Thursday, January 6, 2022 2:58 AM
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

namespace Ph.CoDe_A.Lakbay.SteppedApplication.RightOfWay {
    using Core;

    public class SAROWPlayer : SASteppedVehiclePlayer {
        protected bool _failed = false;
        public virtual bool failed => _failed;
        protected bool _done = false;
        public virtual bool done => _done;
        protected bool _pedestriansStartedWalking = false;
        public virtual bool pedestriansStartedWalking =>
            _pedestriansStartedWalking;
        public List<Pedestrian> pedestrians = new List<Pedestrian>();

        public override void OnTriggerEnter(Collider collider) {
            base.OnTriggerEnter(collider);
            var ptrigger = collider.GetTrigger<PedestrianTrigger>();
            if(ptrigger && !pedestriansStartedWalking) {
                WalkPedestrians();
                ptrigger.gameObject.SetActive(false);
            }
        }

        public override void Proceed() {
            LoadScene();
        }

        public virtual void WalkPedestrians() {
            _pedestriansStartedWalking = true;
            foreach(var pedestrian in pedestrians) {
                pedestrian.StartWalking();
            }
        }
    }
}