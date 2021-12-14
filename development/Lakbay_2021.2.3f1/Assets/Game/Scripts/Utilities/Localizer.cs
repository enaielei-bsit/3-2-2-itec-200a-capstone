/*
 * Date Created: Friday, December 3, 2021 9:10 PM
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
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Utilities {
    public class Localizer : MonoBehaviour {
        public GameObject events;

        public virtual void Awake() {
            if(!events) events = gameObject;
        }

        public virtual void Subscribe<T0, T1>(
            LocalizedAsset<T0> reference, UnityAction<T0> action)
            where T0 : UnityEngine.Object
            where T1 : LocalizeAssetEvent<T0> {
            var @event = GetEvent<T0, T1>(reference, true);
            @event.AssetReference = reference;
            @event.OnUpdateAsset.RemoveListener(action);
            @event.OnUpdateAsset.AddListener(action);
        }

        public virtual void Unsubscribe<T0, T1>(
            LocalizedAsset<T0> reference, UnityAction<T0> action)
            where T0 : UnityEngine.Object
            where T1 : LocalizeAssetEvent<T0> {
            var @event = GetEvent<T0, T1>(reference);
            if(@event) @event.OnUpdateAsset.RemoveListener(action);
        }

        public virtual LocalizeAssetEvent<T0> GetEvent<T0, T1>(
            LocalizedAsset<T0> reference, bool ensure=false)
            where T0 : UnityEngine.Object
            where T1 : LocalizeAssetEvent<T0> {
            var events = this.events.GetComponentsInChildren<T1>();
            T1 @event = default;
            if(events != null && events.Length > 0) {
                @event = Array.Find(events,
                    (e) => e.AssetReference != null
                        && e.AssetReference == reference
                );
            }

            if(ensure && !@event) @event = this.events.AddComponent<T1>();
            return @event;
        }
    }
}