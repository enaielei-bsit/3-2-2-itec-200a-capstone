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
    public abstract class LocalizeAssetEvent<T> :
        LocalizedAssetEvent<
        T, LocalizedAsset<T>, UnityEvent<T>> where T : UnityEngine.Object {
    }
}