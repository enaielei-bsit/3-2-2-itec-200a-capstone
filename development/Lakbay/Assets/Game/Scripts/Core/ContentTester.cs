/*
 * Date Created: Sunday, January 2, 2022 4:22 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.Core
{
    using Utilities;

    public class ContentTester : Controller
    {
        public Color idle = Color.white;
        public Color succeeded = Color.green;
        public Color failed = Color.red;

        [Space]
        public Button test;
        public TMP_InputField input;
        public Content output;

        public virtual void Copy()
        {
            input?.text?.CopyToClipboard();
        }

        public virtual void Paste()
        {
            Clear();
            string data = Helper.GetClipboard();
            if (data != null && input)
            {
                input.text = data;
            }
        }

        public virtual void Clear()
        {
            if (input) input.text = "";
        }

        public virtual void Test()
        {
            if (input && output)
            {
                output.Clear();
                try
                {
                    var content = input.text != null
                        ? input.text.DeserializeAsYaml<List<Entry>>()
                        : new List<Entry>() { };
                    output.Build(content);
                    SetTestColor(succeeded);
                }
                catch
                {
                    printLog("Something is wrong with the input!");
                    SetTestColor(failed);
                }
                Invoke("SetTestColor", 0.25f);
            }
        }

        public virtual void SetTestColor(Color color)
        {
            if (test)
            {
                test.image.color = color;
                var text = test.GetComponentInChildren<TextMeshProUGUI>();
                if (text) text.color = color.Invert();
            }
        }

        public virtual void SetTestColor() => SetTestColor(idle);
    }
}