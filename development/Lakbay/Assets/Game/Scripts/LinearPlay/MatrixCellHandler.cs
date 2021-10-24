/*
 * Date Created: Monday, October 11, 2021 2:51 PM
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

using Utilities;

namespace Ph.CoDe_A.Lakbay.LinearPlay {
    public abstract class MatrixCellHandler : ScriptableObject {
        public abstract void OnBuild(
            Matrix matrix,
            GameObject cell, Vector2Int index, float chance);
    }
}