using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Plugins.Helpers
{
    public class Logger
    {
        private List<LoggerLayer> _loggerLayers;
        private LoggerLayer _loggerLayer;

        public Logger()
        {
            _loggerLayers = new List<LoggerLayer>
            {
                new LoggerLayer(Debug.Log, DefaultLogLayer.Normal.ToString()),
                new LoggerLayer(Debug.LogWarning, DefaultLogLayer.Warning.ToString()),
                new LoggerLayer(Debug.LogError, DefaultLogLayer.Error.ToString()),
            };
        }

        public void CreateNewLoggerLayer(Action<String> logFunction,
            String stringIdentifier,
            bool useLayerMark = false,
            bool isEnabled = true)
        {
            if (_loggerLayers.All(a => a.StringIdentified != stringIdentifier))
            {
                _loggerLayers.Add(new LoggerLayer(logFunction, stringIdentifier, useLayerMark, isEnabled));
            }
            else
            {
                Log(String.Format("Logger layer with identifier [{0}] already presented", stringIdentifier),
                    DefaultLogLayer.Error);
            }
        }

        public void Log(String message, String layer)
        {
            _loggerLayer = _loggerLayers.FirstOrDefault(a => a.StringIdentified == layer && a.IsEnabled);
            if (_loggerLayer != null)
            {
                _loggerLayer.LogThis(message);
            }
        }

        public void Log(String message, DefaultLogLayer layer)
        {
            Log(message, layer.ToString());
        }

        public void SetLogLayerEnabled(String layer, bool enabled)
        {
            _loggerLayer = _loggerLayers.FirstOrDefault(a => a.StringIdentified == layer);
            if (_loggerLayer != null)
            {
                _loggerLayer.IsEnabled = enabled;
            }
        }

        public void SetLogLayerEnabled(DefaultLogLayer layer, bool enabled)
        {
            SetLogLayerEnabled(layer.ToString(), enabled);
        }

        private class LoggerLayer
        {
            public readonly String StringIdentified;
            public bool IsEnabled;
            private readonly bool _useLayerMark;
            private readonly Action<String> _logFunction;

            public LoggerLayer(
                Action<String> logFunction,
                String stringIdentifier,
                bool useLayerMark = false,
                bool isEnabled = true)
            {
                _logFunction = logFunction;
                StringIdentified = stringIdentifier;

                IsEnabled = isEnabled;
                _useLayerMark = useLayerMark;
            }

            public void LogThis(String message)
            {
                _logFunction.Invoke(
                    _useLayerMark
                        ? String.Format("[{0}]: {1}", StringIdentified, message)
                        : message);
            }
        }
    }

    public enum DefaultLogLayer
    {
        Normal = 0,
        Warning = 1,
        Error = 2
    }
}