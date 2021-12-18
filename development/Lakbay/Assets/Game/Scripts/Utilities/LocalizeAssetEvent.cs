/*
 * Date Created: Thursday, December 2, 2021 2:29 PM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

namespace Utilities {
    public class LocalizeAssetEvent<T> :
        LocalizedAssetEvent<
        T, LocalizedAsset<T>, UnityEvent<T>> where T : UnityEngine.Object {
        public virtual void Subscribe(
            LocalizedAsset<T> reference,
            params UnityAction<T>[] actions) {
            AssetReference = reference;
            OnUpdateAsset.RemoveAllListeners();
            Subscribe(actions);
        }

        public virtual void Subscribe(params UnityAction<T>[] actions) {
            foreach(var action in actions) {
                OnUpdateAsset.AddListener(action);
            }
        }
    }
}