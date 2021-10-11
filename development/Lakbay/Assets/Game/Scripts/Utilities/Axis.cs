/*
 * Date Created: Sunday, October 10, 2021 5:47 PM
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
using UnityEngine.UI;

namespace Utilities {
    public enum Axis {X, Y, Z}
    public enum AxisDirection {Negative=-1, Zero=0, Positive=1}

    [Serializable]
    public struct AxesDirection3 {
        public static AxesDirection3 positive => new AxesDirection3(
            AxisDirection.Positive,
            AxisDirection.Positive,
            AxisDirection.Positive
        );

        public AxisDirection x;
        public AxisDirection y;
        public AxisDirection z;

        public AxesDirection3(AxisDirection x, AxisDirection y, AxisDirection z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public AxesDirection3(AxisDirection x, AxisDirection y)
            : this(x, y, AxisDirection.Zero) {}
    }

    [Serializable]
    public struct AxesDirection2 {
        public static AxesDirection2 positive => new AxesDirection2(
            AxisDirection.Positive,
            AxisDirection.Positive
        );

        public AxisDirection x;
        public AxisDirection y;

        public AxesDirection2(AxisDirection x, AxisDirection y) {
            this.x = x;
            this.y = y;
        }
    }
}