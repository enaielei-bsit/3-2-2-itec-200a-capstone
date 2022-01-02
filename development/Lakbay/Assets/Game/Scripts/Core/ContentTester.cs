/*
 * Date Created: Sunday, January 2, 2022 4:22 AM
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

using TMPro;

namespace Ph.CoDe_A.Lakbay.Core {
    using Utilities;

    public class ContentTester : Controller {
        public TMP_InputField input;
        public Content output;

        public virtual void Copy() {
            input?.text?.CopyToClipboard();
        }

        public virtual void Paste() {
            Clear();
            string data = Helper.GetClipboard();
            if(data != null && input) {
                input.text = data;
            }
        }

        public virtual void Clear() {
            if(input) input.text = "";
        }

        public virtual void Test() {
            if(input && output) {
                try {
                    var content = input.text != null
                        ? input.text.DeserializeAsYaml<List<Entry>>()
                        : new List<Entry>() {};
                    output.Build(content);
                } catch {
                    printLog("Something is wrong with the input!");
                }
            }
        }
    }
}