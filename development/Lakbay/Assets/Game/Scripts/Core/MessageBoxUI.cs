/*
 * Date Created: Tuesday, January 11, 2022 6:44 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2022 CoDe_A. All Rights Reserved.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utilities;

namespace Ph.CoDe_A.Lakbay.Core {
    public enum MessageBox {
        AutoDismiss, Message, Confirmation
    }

    [RequireComponent(typeof(LocalizeStringEvent))]
    public class MessageBoxUI : Controller {
        public virtual LocalizeStringEvent messageEvent =>
            GetComponent<LocalizeStringEvent>();
        protected LTDescr _showHide;
        protected Coroutine _autoDismiss;

        [Space]
        public RectTransform root;
        public TextMeshProUGUI message;
        [Header("Buttons")]
        public Button okay;
        public Button yes;
        public Button no;
        public Button autoDismiss;

        public override void Awake() {
            messageEvent?.OnUpdateString.AddListener(OnUpdateMessage);
        }

        public virtual void Show() => Show(2.0f);

        public virtual void Show(string message) => Show(
            message, MessageBox.Message);
        
        public virtual void Show(float animation=0.25f) {
            gameObject.SetActive(true);
            root?.gameObject.SetActive(true);
            if(root) {
                root.localScale = Vector3.zero;
                if(_showHide != null) LeanTween.cancel(_showHide.uniqueId);
                _showHide = LeanTween.scale(root, Vector3.one, animation);
            }
        }

        public virtual void Show(
            LocalizedString message,
            MessageBox type=MessageBox.Message,
            float autoDismiss=5.0f,
            UnityAction onOkay=default,
            UnityAction onYes=default,
            UnityAction onNo=default,
            float animation=0.25f
        ) {
            Show(animation);
            if(message != null) messageEvent.StringReference = message;
            messageEvent.RefreshString();
            SetButtons(type, autoDismiss, onOkay, onYes, onNo);
        }

        public virtual void Show(
            string message,
            MessageBox type=MessageBox.Message,
            float autoDismiss=5.0f,
            UnityAction onOkay=default,
            UnityAction onYes=default,
            UnityAction onNo=default,
            float animation=0.25f
        ) {
            Show(animation);
            OnUpdateMessage(message);
            if(message != null) messageEvent.StringReference = null;
            SetButtons(type, autoDismiss, onOkay, onYes, onNo);
        }

        public virtual void ShowMessage(
            LocalizedString message,
            UnityAction onOkay=default,
            float animation=0.25f
        ) {
            Show(message, onOkay: onOkay, animation: animation);
        }

        public virtual void ShowMessage(
            string message,
            UnityAction onOkay=default,
            float animation=0.25f
        ) {
            Show(message, onOkay: onOkay, animation: animation);
        }

        public virtual void ShowConfirmation(
            LocalizedString message,
            UnityAction onYes=default,
            UnityAction onNo=default,
            float animation=0.25f
        ) {
            Show(message, MessageBox.Confirmation,
                onYes: onYes, onNo: onNo, animation: animation);
        }

        public virtual void ShowConfirmation(
            string message,
            UnityAction onYes=default,
            UnityAction onNo=default,
            float animation=0.25f
        ) {
            Show(message, MessageBox.Confirmation,
                onYes: onYes, onNo: onNo, animation: animation);
        }

        public virtual void ShowAutoDismiss(
            LocalizedString message,
            float autoDismiss=5.0f,
            float animation=0.25f
        ) {
            Show(message, MessageBox.AutoDismiss,
                autoDismiss, animation: animation);
        }

        public virtual void ShowAutoDismiss(
            string message,
            float autoDismiss=5.0f,
            float animation=0.25f
        ) {
            Show(message, MessageBox.AutoDismiss, autoDismiss);
        }

        public virtual void Hide() {
            gameObject.SetActive(false);
        }

        public virtual void OnUpdateMessage(string message) {
            if(message != null) this.message?.SetText(message);
        }

        public virtual void OnButtonPress() {
            Hide();
        }

        public virtual void SetButton(Button button, UnityAction action) {
            button?.gameObject.SetActive(true);
            button.onClick.RemoveAllListeners();
            if(action != null) button.onClick.AddListener(action);
            button.onClick.AddListener(OnButtonPress);
        }

        public virtual void SetButtons(
            MessageBox type=MessageBox.Message,
            float autoDismiss=5.0f,
            UnityAction onOkay=default,
            UnityAction onYes=default,
            UnityAction onNo=default
        ) {
            okay?.gameObject.SetActive(false);
            yes?.gameObject.SetActive(false);
            no?.gameObject.SetActive(false);
            this.autoDismiss?.gameObject.SetActive(false);
            if(type == MessageBox.Message) {
                SetButton(okay, onOkay);
            } else if(type == MessageBox.Confirmation) {
                SetButton(yes, onYes);
                SetButton(no, onNo);
            } else if(type == MessageBox.AutoDismiss) {
                SetButton(this.autoDismiss, OnButtonPress);
                autoDismiss = Mathf.Max(autoDismiss, 0.001f);
                this.Run(
                    autoDismiss,
                    onProgress: (d, e) => {
                        var text = this.autoDismiss?.
                            GetComponentInChildren<TextMeshProUGUI>();
                        text?.SetText(Mathf.FloorToInt(d - e).ToString());
                        return Time.deltaTime;
                    },
                    onFinish: (d, e) => {
                        this.autoDismiss?.onClick.Invoke();
                    }
                );
            }
        }
    }
}