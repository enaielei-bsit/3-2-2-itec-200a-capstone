/*
 * Date Created: Wednesday, November 17, 2021 8:59 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.Compilation;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Utilities {
    public static class Compiler {
        public static void Compile() {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}