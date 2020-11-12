using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoomerangUI : MonoBehaviour
{
    public PlayerMouse playerM;

    Slider speedSlider;
    Slider timeSlider;
    Slider angleV1Slider;
    Slider angleV2Slider;
    Slider ySpeedSlider;

    InputField speedField;
    InputField timeField;
    InputField angleV1Field;
    InputField angleV2Field;
    InputField ySpeedField;

    // Start is called before the first frame update
    void Start()
    {
        speedSlider = transform.Find("SpeedVar").Find("Slider").GetComponent<Slider>();
        speedField = transform.Find("SpeedVar").Find("InputField").GetComponent<InputField>();
        speedSlider.value = playerM.boomVal.speed;
        speedField.text = ""+playerM.boomVal.speed;
        speedSlider.onValueChanged.AddListener(UpdateSpeedSlider);
        speedField.onValueChanged.AddListener(UpdateSpeedField);

        timeSlider = transform.Find("DurationVar").Find("Slider").GetComponent<Slider>();
        timeField = transform.Find("DurationVar").Find("InputField").GetComponent<InputField>();
        timeSlider.value = playerM.boomVal.time;
        timeField.text = "" + playerM.boomVal.time;
        timeSlider.onValueChanged.AddListener(UpdateTimeSlider);
        timeField.onValueChanged.AddListener(UpdateTimeField);

        angleV1Slider = transform.Find("AngleSpeedAVar").Find("Slider").GetComponent<Slider>();
        angleV1Field = transform.Find("AngleSpeedAVar").Find("InputField").GetComponent<InputField>();
        angleV1Slider.value = playerM.boomVal.angVel1;
        angleV1Field.text = "" + playerM.boomVal.angVel1;
        angleV1Slider.onValueChanged.AddListener(UpdateA1Slider);
        angleV1Field.onValueChanged.AddListener(UpdateA1Field);


        angleV2Slider = transform.Find("AngleSpeedBVar").Find("Slider").GetComponent<Slider>();
        angleV2Field = transform.Find("AngleSpeedBVar").Find("InputField").GetComponent<InputField>();
        angleV2Slider.value = playerM.boomVal.angVel2;
        angleV2Field.text = "" + playerM.boomVal.angVel2;
        angleV2Slider.onValueChanged.AddListener(UpdateA2Slider);
        angleV2Field.onValueChanged.AddListener(UpdateA2Field);


        ySpeedSlider = transform.Find("YspeedVar").Find("Slider").GetComponent<Slider>();
        ySpeedField = transform.Find("YspeedVar").Find("InputField").GetComponent<InputField>();
        ySpeedSlider.value = playerM.boomVal.yVel;
        ySpeedField.text = "" + playerM.boomVal.yVel;
        ySpeedSlider.onValueChanged.AddListener(UpdateYspeedSlider);
        ySpeedField.onValueChanged.AddListener(UpdateYspeedField);
    }


    public void ResetBoomValues()
    {
        playerM.boomVal.Reset();
    }

    public void UpdateSpeedSlider(float i)
    {
        speedField.text = "" + i;
        playerM.boomVal.speed = i;
        //speedSlider.value = i;
        //Debug.Log(i);
    }
    public void UpdateSpeedField(string s)
    {
        float v = float.Parse(s);
        speedSlider.value = v;
        playerM.boomVal.speed = v;
    }
    
    public void UpdateTimeSlider(float i)
    {
        timeField.text = "" + i;
        playerM.boomVal.time = i;
        //speedSlider.value = i;
        //Debug.Log(i);
    }
    public void UpdateTimeField(string s)
    {
        float v = float.Parse(s);
        timeSlider.value = v;
        playerM.boomVal.time = v;
    }
     
    public void UpdateA1Slider(float i)
    {
        angleV1Field.text = "" + i;
        playerM.boomVal.angVel1 = i;
        //speedSlider.value = i;
        //Debug.Log(i);
    }
    public void UpdateA1Field(string s)
    {
        float v = float.Parse(s);
        angleV1Slider.value = v;
        playerM.boomVal.angVel1 = v;
    }
     
    public void UpdateA2Slider(float i)
    {
        angleV2Field.text = "" + i;
        playerM.boomVal.angVel2 = i;
        //speedSlider.value = i;
        //Debug.Log(i);
    }
    public void UpdateA2Field(string s)
    {
        float v = float.Parse(s);
        angleV2Slider.value = v;
        playerM.boomVal.angVel2 = v;
    }

     public void UpdateYspeedSlider(float i)
    {
        ySpeedField.text = "" + i;
        playerM.boomVal.yVel = i;
        //speedSlider.value = i;
        //Debug.Log(i);
    }
    public void UpdateYspeedField(string s)
    {
        float v = float.Parse(s);
        ySpeedSlider.value = v;
        playerM.boomVal.yVel = v;
    }

    public void Reset()
    {
        playerM.boomVal.Reset();

        speedSlider.value = playerM.boomVal.speed;
        speedField.text = "" + playerM.boomVal.speed;

        timeSlider.value = playerM.boomVal.time;
        timeField.text = "" + playerM.boomVal.time;

        angleV1Slider.value = playerM.boomVal.angVel1;
        angleV1Field.text = "" + playerM.boomVal.angVel1;

        angleV2Slider.value = playerM.boomVal.angVel2;
        angleV2Field.text = "" + playerM.boomVal.angVel2;

        ySpeedSlider.value = playerM.boomVal.yVel;
        ySpeedField.text = "" + playerM.boomVal.yVel;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
