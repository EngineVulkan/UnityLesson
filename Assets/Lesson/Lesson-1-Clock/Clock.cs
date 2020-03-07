using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform hoursTransform;
    public Transform minutesTransform;
    public Transform secondsTransform;
    public bool continuous;

    const float degreePerHours = 30f;
    const float degreePerMinutes = 6f;
    const float degreePerSeconds = 6f;
    void Start()
    {
        //DateTime time = DateTime.Now;
        //hoursTransform.localRotation=Quaternion.Euler(0, time.Hour * degreePerHours, 0);
        //minutesTransform.localRotation =Quaternion.Euler(0f, time.Minute* degreePerMinutes,0f);
        //secondsTransform.localRotation =Quaternion.Euler(0f, time.Second* degreePerSeconds,0f);
    }

    // Update is called once per frame
    void Update()
    {
        DateTime time = DateTime.Now;
        hoursTransform.localRotation = Quaternion.Euler(0, time.Hour * degreePerHours, 0);
        minutesTransform.localRotation = Quaternion.Euler(0f, time.Minute * degreePerMinutes, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, time.Second * degreePerSeconds, 0f);
        if (continuous)
        {
            UpdateContinuous();
        }
        else
        {
            UpdateDiscrete();
        }
    }

    void UpdateContinuous()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hoursTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalHours * degreePerHours, 0f);
        minutesTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalMinutes * degreePerMinutes, 0f);
        secondsTransform.localRotation = Quaternion.Euler(0f, (float)time.TotalSeconds * degreePerSeconds, 0f);
    }
    void UpdateDiscrete()
    {
        DateTime time = DateTime.Now;
        hoursTransform.localRotation =Quaternion.Euler(0f, time.Hour * degreePerHours, 0f);
        minutesTransform.localRotation =Quaternion.Euler(0f, time.Minute * degreePerMinutes, 0f);
        secondsTransform.localRotation =Quaternion.Euler(0f, time.Second * degreePerSeconds, 0f);
    }
}
