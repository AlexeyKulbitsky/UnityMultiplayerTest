using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public delegate void CustomButtonFunc(bool value);

public class CustomButton : Button 
{
    public CustomButtonFunc onCustomButton;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        onCustomButton(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        onCustomButton(false);
    }
}
