/*
 * Date Created: Saturday, October 16, 2021 9:54 PM
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

namespace Utilities.Event {
    public class StandardEvent<T0, T1> : UnityEvent<T0, T1, T1> {}

    public class Generic<T0, T1> : StandardEvent<T0, T1> {}
    public class String<T> : StandardEvent<T, string> {}
    public class Float<T> : StandardEvent<T, float> {}
    public class Int<T> : StandardEvent<T, int> {}
    public class Bool<T> : StandardEvent<T, bool> {}
}