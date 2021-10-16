/*
 * Date Created: Thursday, October 14, 2021 10:05 PM
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

namespace Ph.CoDe_A.Lakbay.LinearPlay.Buffs {
    public class StopwatchBuff : Buff {
        public GameObject overlay;
        public float speedFactor = 0.5f;

        protected GameObject _overlay;

        public override void OnAdd(Buffable buffable, float duration) {
            var player = buffable.GetComponent<Player>();
            if(player && player.travel) player.travel.timeScale *= speedFactor;
            var ui = FindObjectOfType<UserInterface>();
            if(ui && overlay) _overlay = Instantiate(overlay, ui.transform); 
        }

        public override void OnLinger(
            Buffable buffable, float duration, float elapsedTime) {
                
        }

        public override void OnRemove(Buffable buffable) {
            var player = buffable.GetComponent<Player>();
            if(player && player.travel) player.travel.timeScale /= speedFactor;
            if(_overlay) Destroy(_overlay.gameObject);
        }
    }
}