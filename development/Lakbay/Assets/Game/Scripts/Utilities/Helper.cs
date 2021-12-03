/*
 * Date Created: Sunday, October 10, 2021 5:25 AM
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
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

using YamlDotNet.Serialization;

namespace Utilities {
    public static class Helper {
        public static readonly ISerializer YamlSerializer =
            new SerializerBuilder()
            .Build();
        public static readonly IDeserializer YamlDeserializer =
            new DeserializerBuilder()
            .Build();
        public static Locale[] locales =>
            LocalizationSettings.AvailableLocales.Locales.ToArray();

        // source: https://scholarwithin.com/average-reading-speed
        // Word per Minute
        public const float AverageReadSpeed = 220.0f;

        public delegate bool ConditionalRunCondition(float elapsedTime);
        public delegate void ConditionalRunOnStart(float elapsedTime);
        public delegate float ConditionalRunOnProgress(float elapsedTime);
        public delegate void ConditionalRunOnFinish(float elapsedTime);

        public delegate void TimedRunOnStart(float duration, float elapsedTime);
        public delegate float TimedRunOnProgress(
            float duration, float elapsedTime);
        public delegate void TimedRunOnFinish(float duration, float elapsedTime);
        public static IEnumerator Run(
            ConditionalRunCondition condition=default,
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            ConditionalRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            float elapsedTime = 0.0f;
            onStart?.Invoke(elapsedTime);
            while(condition == null || condition(elapsedTime)) {
                float deltaTime = onProgress != null
                    ? onProgress(elapsedTime) : Time.deltaTime;
                if(fixedUpdate) yield return new WaitForFixedUpdate();
                else yield return new WaitForEndOfFrame();
                elapsedTime += deltaTime;
            }
            onFinish?.Invoke(elapsedTime);
        }

        public static IEnumerator Run(
            float duration,
            TimedRunOnStart onStart=default,
            TimedRunOnProgress onProgress=default,
            TimedRunOnFinish onFinish=default,
            bool fixedUpdate=false
        ) {
            return Run(
                (e) => e < duration,
                (e) => onStart?.Invoke(duration, e),
                (e) => onProgress != null
                    ? onProgress(duration, e) : Time.deltaTime,
                (e) => onFinish?.Invoke(duration, e),
                fixedUpdate
            );
        }

        public static IEnumerator Run(
            ConditionalRunOnStart onStart=default,
            ConditionalRunOnProgress onProgress=default,
            bool fixedUpdate=false
        ) {
            return Run(
                (e) => true,
                onStart,
                onProgress,
                null,
                fixedUpdate
            );
        }

        public static float GetExpectedReadTime(string str) {
            return GetExpectedReadTime(str, AverageReadSpeed);
        } 

        public static float GetExpectedReadTime(string str, float readSpeed) {
            return 60 * (str.Split(' ').Length / readSpeed);
        }

        public static void TriggerLocaleChange() {
            var value = LocalizationSettings.SelectedLocale;
            LocalizationSettings.SelectedLocale = 
                LocalizationSettings.AvailableLocales.Locales.Find(
                    (l) => l != value);
            LocalizationSettings.SelectedLocale = value;
        }
    }
}