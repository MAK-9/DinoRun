using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash
{
    public const int DASH_MAX = 100;

    private float dashAmt;
    private float dashRegenAmt;

    public Dash()
    {
        dashAmt = DASH_MAX;
        dashRegenAmt = 20f;
    }

    public void Update()
    {
        dashAmt += dashRegenAmt * Time.deltaTime;
        dashAmt = Mathf.Clamp(dashAmt, 0f, DASH_MAX);
        Debug.Log(dashAmt);
    }

    public bool PerformDash()
    {
        if (dashAmt >= DASH_MAX)
        {
            dashAmt -= DASH_MAX;
            return true;
        }

        return false;
    }

    public float GetDashValue()
    {
        return dashAmt;
    }
}
