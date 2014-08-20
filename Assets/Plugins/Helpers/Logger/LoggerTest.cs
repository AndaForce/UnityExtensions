using System;
using System.Collections;
using Assets.Plugins.Helpers;
using UnityEngine;

public class LoggerTest : MonoBehaviour
{
    [SerializeField] private TextMesh _textMesh;
    private Logger _logger;
    private int _value;
    private const String CustomLogLayer = "TextMesh";

    private void Start()
    {
        _logger = new Logger();

        _logger.Log("Normal message", DefaultLogLayer.Normal);
        _logger.Log("Warning message", DefaultLogLayer.Warning);
        _logger.Log("Error message", DefaultLogLayer.Error);

        _logger.CreateNewLoggerLayer(Debug.Log, "Custom", true);
        _logger.Log("Custom logger", "Custom");

        _logger.SetLogLayerEnabled(DefaultLogLayer.Normal, false);
        _logger.Log("This message won't show", DefaultLogLayer.Normal);

        _logger.CreateNewLoggerLayer(TextMeshLog, CustomLogLayer);
        StartCoroutine(LogCounter());
    }

    private IEnumerator LogCounter()
    {
        while (true)
        {
            _logger.Log(String.Format("Elapsed seconds: {0}", _value), CustomLogLayer);
            _value += 1;

            yield return new WaitForSeconds(1.0f);
        }
    }

    private void TextMeshLog(String message)
    {
        _textMesh.text = message;
    }
}