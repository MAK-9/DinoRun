using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashMeter : MonoBehaviour
{

    public Slider dashSlider;
    private float dashValue=1f;
    private Dash dash;

    private void Awake()
    {
        dash = new Dash();
    }

    private void Update()
    {
        dash.Update();
        dashSlider.value = dash.GetDashValue();
    }

    public bool TryToDash()
    {
        return dash.PerformDash();
    }
}
