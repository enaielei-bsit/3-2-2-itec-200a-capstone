/*
 * Date Created: Wednesday, January 12, 2022 1:25 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ph.CoDe_A.Lakbay.SteppedApplication
{
    using UnityEngine.Localization;
    using UnityEngine.Localization.Components;
    using Utilities;

    public class SAGameOverUI : Core.GameMenuUI
    {
        [Header("Buttons")]
        public Button proceed;
        public Button retry;
        public Button end;

        [Space]
        public LocalizeStringEvent correctPointer;
        public LocalizeStringEvent wrongPointer;
        public LayoutGroup pointers;

        [Space]
        public TextMeshProUGUI passed;
        public TextMeshProUGUI failed;


        public virtual void Show()
        {
            gameObject.SetActive(true);
            pointers?.transform.DestroyChildren();
            passed?.gameObject.SetActive(false);
            failed?.gameObject.SetActive(false);

            proceed?.gameObject.SetActive(false);
            retry?.gameObject.SetActive(false);
            end?.gameObject.SetActive(true);
        }

        public virtual void ShowPassed(params LocalizedString[] pointers)
        {
            Show();
            passed?.gameObject.SetActive(true);
            proceed?.gameObject.SetActive(true);
            ShowPointers(true, pointers);
        }

        public virtual void ShowFailed(params LocalizedString[] pointers)
        {
            Show();
            failed?.gameObject.SetActive(true);
            retry?.gameObject.SetActive(true);
            ShowPointers(false, pointers);
        }

        public virtual void ShowPointers(bool correct, params LocalizedString[] pointers)
        {
            foreach (var pointer in pointers)
            {
                var npointer = Instantiate(
                    correct ? correctPointer : wrongPointer,
                    this.pointers.transform);
                npointer.StringReference = pointer;
                npointer.RefreshString();
            }
        }
    }
}