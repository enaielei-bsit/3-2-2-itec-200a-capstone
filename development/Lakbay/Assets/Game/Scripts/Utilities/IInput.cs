/*
 * Date Created: Monday, December 20, 2021 5:54 PM
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
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Utilities {
    // using Sys = UnityEngine.InputSystem;
    public static class IInput {
        public static Keyboard keyboard => Keyboard.current;
        public static Mouse mouse => Mouse.current;
        public static Touchscreen touchscreen => Touchscreen.current;
    }
}