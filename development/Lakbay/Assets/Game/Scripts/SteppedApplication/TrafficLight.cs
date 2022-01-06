/*
 * Date Created: Friday, January 7, 2022 1:52 AM
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
using Utilities;

namespace Ph.CoDe_A.Lakbay.SteppedApplication {
    public class TrafficLight : Core.Controller {
        public enum State {None, Red, Yellow, Green}

        protected Coroutine _setState;

        public Light red;
        public Light yellow;
        public Light green;
        [SerializeField]
        protected State _state = State.None;
        public State state {
            get => _state;
            set {
                if(value == State.Red) {
                    red?.gameObject.SetActive(true);
                    yellow?.gameObject.SetActive(false);
                    green?.gameObject.SetActive(false);
                } else if(value == State.Yellow) {
                    red?.gameObject.SetActive(false);
                    yellow?.gameObject.SetActive(true);
                    green?.gameObject.SetActive(false);
                } else if(value == State.Green) {
                    red?.gameObject.SetActive(false);
                    yellow?.gameObject.SetActive(false);
                    green?.gameObject.SetActive(true);
                } else if(value == State.None) {
                    red?.gameObject.SetActive(false);
                    yellow?.gameObject.SetActive(false);
                    green?.gameObject.SetActive(false);
                }
                var old = _state;
                _state = value;
                if(value != state) {
                    onStateChange?.Invoke(this, old, state);
                }
            }
        }
        protected float _duration = 0.0f;
        public virtual float duration => _duration;
        protected float _elapsedTime = 0.0f;
        public virtual float elapsedTime => _elapsedTime;

        public UnityEvent<TrafficLight, State, State> onStateChange = new UnityEvent<TrafficLight, State, State>();
        public UnityEvent<TrafficLight, State, State> onStateFinish = new UnityEvent<TrafficLight, State, State>();

        public virtual void ToggleState(State state, float duration) {
            if(_setState != null) StopCoroutine(_setState);
            _duration = 0.0f;
            _elapsedTime = 0.0f;
            var old = state;
            this.state = state;
            if(duration <= 0) {
                state = State.None;
                onStateFinish?.Invoke(this, old, state);
            }
            _setState = this.Run(
                duration,
                onProgress: (d, e) => {
                    _duration = d;
                    _elapsedTime = e;
                    return Time.deltaTime;
                },
                onFinish: (d, e) => {
                    _duration = 0.0f;
                    _elapsedTime = 0.0f;
                    this.state = State.None;
                    onStateFinish?.Invoke(this, old, state);
                }
            );
        }

        public virtual void StopToggleState() {
            if(_setState != null) StopCoroutine(_setState);
            _duration = 0.0f;
            _elapsedTime = 0.0f;
        }

        public override void OnValidate() {
            state = _state;
        }
    }
}