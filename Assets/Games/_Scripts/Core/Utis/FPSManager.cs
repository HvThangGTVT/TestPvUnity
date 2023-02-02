using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    [SerializeField] float time = 1;
    [SerializeField] TMP_Text text = null;
    [SerializeField] bool showOnlyNumbers = true;
    [SerializeField] bool limitToRefreshRate = true;

    float _result = 0;
    int _samples = 0;

    string _output = "FPS : ";

    float _fps = 60f;

    public float Fps
    {
        get { return _fps; }
    }


    void Awake()
    {
        _fps = Screen.currentResolution.refreshRate;
    }

    void Update()
    {
        if (time > 0)
        {
            _result += 1f / Time.unscaledDeltaTime;
            _samples++;
            time -= Time.unscaledDeltaTime;
        }
        else
        {
            _fps = _result / _samples;

            if (limitToRefreshRate && QualitySettings.vSyncCount != 0)
                _fps = Mathf.Min(_fps, Screen.currentResolution.refreshRate);

            _output = showOnlyNumbers ? _fps.ToString("F1") : "FPS : " + _fps.ToString("F1");

            if (text != null)
                text.text = _output;

            _result = 0;
            _samples = 0;
            time = 1;
        }
    }
}