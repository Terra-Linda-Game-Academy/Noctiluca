using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Menu Item Transition Info", menuName = "Utilities/UI/MenuItemTransitionInfo", order = 1)]
public class MenuItemTransitionInfo : ScriptableObject
{

    public CanvasAnchorPoint canvasAnchorPoint;
    public Vector2 exitPosition;
    public bool localTransformation = true;
    

    public AnimationCurve exitMovement;


    
    public Vector3 CalculateExitPosition(Vector3 correctPosiion)
    {
        Vector3 newExitPosition = Vector3.zero;

        if (localTransformation)
            newExitPosition += correctPosiion;

        return newExitPosition;
    }

    public Vector3 CalculateNextExitPosition(Vector3 currentPosition, Canvas canvas)
    {
        return Vector3.zero;
    }

    public Vector3 CalculateNextEnterPosition(Vector3 currentPosition, Vector3 endPosition, Canvas canvas)
    {
        return Vector3.zero;
    }


    public enum CanvasAnchorPoint
    {
        TOP_CENTER,
        BOTTOM_CENTER,
        LEFT_MIDDLE,
        RIGHT_MIDDLE,
        TOP_LEFTCORNER,
        TOP_RIGHT_CORNER,
        BOTTOM_LEFT_CORNER,
        BOTTOM_RIGHT_CORNER

    }
}
