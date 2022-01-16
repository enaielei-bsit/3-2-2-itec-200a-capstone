/*
 * Date Created: Sunday, October 10, 2021 5:25 AM
 * Author: enaielei <nommel.isanar.lavapie.amolat@gmail.com>
 * 
 * Copyright © 2021 CoDe_A. All Rights Reserved.
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using YamlDotNet.Serialization;

namespace Utilities
{
    public static class Helper
    {
        public static readonly ISerializer YamlSerializer =
            new SerializerBuilder()
            .Build();
        public static readonly IDeserializer YamlDeserializer =
            new DeserializerBuilder()
            .Build();
        public static Locale[] locales =>
            LocalizationSettings.AvailableLocales.Locales.ToArray();
        public static Vector3[] inputPositions
        {
            get
            {
                var position = new List<Vector3>();
                if (Input.touchSupported)
                {
                    return Input.touches.Select((t) => t.position)
                    .Select((p) => (Vector3)p).ToArray();
                }
                else
                {
                    return new Vector3[] { Input.mousePosition };
                }
            }
        }

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
            ConditionalRunCondition condition = default,
            ConditionalRunOnStart onStart = default,
            ConditionalRunOnProgress onProgress = default,
            ConditionalRunOnFinish onFinish = default,
            bool fixedUpdate = false
        )
        {
            float elapsedTime = 0.0f;
            onStart?.Invoke(elapsedTime);
            while (condition == null || condition(elapsedTime))
            {
                float deltaTime = onProgress != null
                    ? onProgress(elapsedTime) : Time.deltaTime;
                if (fixedUpdate) yield return new WaitForFixedUpdate();
                else yield return new WaitForEndOfFrame();
                elapsedTime += deltaTime;
            }
            onFinish?.Invoke(elapsedTime);
        }

        public static IEnumerator Run(
            float duration,
            TimedRunOnStart onStart = default,
            TimedRunOnProgress onProgress = default,
            TimedRunOnFinish onFinish = default,
            bool fixedUpdate = false
        )
        {
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
            ConditionalRunOnStart onStart = default,
            ConditionalRunOnProgress onProgress = default,
            bool fixedUpdate = false
        )
        {
            return Run(
                (e) => true,
                onStart,
                onProgress,
                null,
                fixedUpdate
            );
        }

        public static float GetExpectedReadTime(string str)
        {
            return GetExpectedReadTime(str, AverageReadSpeed);
        }

        public static float GetExpectedReadTime(string str, float readSpeed)
        {
            return 60 * (str.Split(' ').Length / readSpeed);
        }

        public static void TriggerLocaleChange()
        {
            if (!LocalizationSettings.InitializationOperation.IsDone) return;
            var value = LocalizationSettings.SelectedLocale;
            LocalizationSettings.SelectedLocale =
                LocalizationSettings.AvailableLocales.Locales.Find(
                    (l) => l != value);
            LocalizationSettings.SelectedLocale = value;
        }

        public static string GetClipboard()
        {
            return GUIUtility.systemCopyBuffer;
        }

        public static void WriteFile(
            string path,
            string content,
            bool append,
            Encoding encoding
        )
        {
            var writer = new StreamWriter(path, append, encoding);
            writer.Write(content);
            writer.Close();
        }

        public static void WriteFile(
            string path, string content,
            bool append = false
        )
        {
            WriteFile(path, content, append, Encoding.UTF8);
        }

        public static string ReadFile(
            string path, Encoding encoding, bool create = true
        )
        {
            if (!File.Exists(path) && create)
                WriteFile(path, "", false, encoding);
            var reader = new StreamReader(path, encoding);
            string content = reader.ReadToEnd();
            reader.Close();
            return content;
        }

        public static string ReadFile(string path, bool create = true) =>
            ReadFile(path, Encoding.UTF8, create);
    }
}