/*
 * Date Created: Sunday, October 10, 2021 5:25 AM
 * Author: nommel-isanar <nommel.isanar.lavapie.amolat@gmail.com>
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
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Utilities {
    public class Helper : MonoBehaviour {
        public static Locale[] GetLocales() {
            Locale[] locales;
            locales = LocalizationSettings.AvailableLocales.Locales.ToArray();

            return locales;
        }
    }
}