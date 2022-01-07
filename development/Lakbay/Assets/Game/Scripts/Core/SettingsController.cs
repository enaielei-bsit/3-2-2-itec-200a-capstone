/*
 * Date Created: Friday, January 7, 2022 2:13 PM
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

namespace Ph.CoDe_A.Lakbay.Core {
    using Utilities;

    [Serializable]
    public class Settings {

        public Audio audio;
    }

    public class SettingsController : Controller {
        [SerializeField]
        protected string _filePath = "settings.yaml";
        public virtual string filePath => 
            $"{Application.persistentDataPath}/{_filePath}";

        public virtual void Load() {
            // var
        }
    }
}