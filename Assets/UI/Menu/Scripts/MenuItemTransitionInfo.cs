using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Menu Item Transition Info", menuName = "Utilities/UI/MenuItemTransitionInfo", order = 1)]
public class MenuItemTransitionInfo : ScriptableObject
{

    public CanvasAnchorPoint canvasAnchorPoint;
    public Vector2 exitPositionOffset;
    public bool localTransformation = true;
    

    public AnimationCurve exitMovement;


    
    public Vector3 CalculateExitPosition(Vector3 originalPosiion, Canvas canvas)
    {
        Vector3 newExitPosition = Vector3.zero;


        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        float verticalWidth = canvasRect.rect.width;

        float horizontalHeight = canvasRect.rect.height;

        switch(canvasAnchorPoint) {
            case CanvasAnchorPoint.CENTER:
                newExitPosition = new Vector3(exitPositionOffset.x, exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.TOP_CENTER:
                newExitPosition = new Vector3(exitPositionOffset.x, horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.BOTTOM_CENTER:
                newExitPosition = new Vector3(exitPositionOffset.x, -horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.LEFT_MIDDLE:
                newExitPosition = new Vector3(-verticalWidth / 2f + exitPositionOffset.x, exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.RIGHT_MIDDLE:
                newExitPosition = new Vector3(verticalWidth / 2f + exitPositionOffset.x, exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.TOP_LEFT:
                newExitPosition = new Vector3(-verticalWidth / 2f + exitPositionOffset.x, horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.TOP_RIGHT:
                newExitPosition = new Vector3(verticalWidth / 2f + exitPositionOffset.x, horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.BOTTOM_LEFT:
                newExitPosition = new Vector3(-verticalWidth / 2f + exitPositionOffset.x, -horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
            case CanvasAnchorPoint.BOTTOM_RIGHT:
                newExitPosition = new Vector3(verticalWidth / 2f + exitPositionOffset.x, -horizontalHeight / 2f + exitPositionOffset.y, 0);
                break;
        }

        if (localTransformation)
            newExitPosition += originalPosiion;

        

        return newExitPosition;
    }

    public Vector3 CalculateNextExitPosition(Vector3 currentPosition, Vector3 originalPosiion, Vector3 exitPosition, float delta, Canvas canvas)
    {
        //Vector3 exitPosition = CalculateExitPosition(originalPosiion, canvas);
        float progress = Vector3.Distance(exitPosition, currentPosition) / Vector3.Distance(exitPosition, originalPosiion);

        return Vector3.Lerp(currentPosition, exitPosition, exitMovement.Evaluate(progress) * delta);
    }

    public Vector3 CalculateNextEnterPosition(Vector3 currentPosition, Vector3 originalPosiion, Vector3 exitPosition, float delta, Canvas canvas)
    {
        //Vector3 exitPosition = CalculateExitPosition(originalPosiion, canvas);
        float progress = 1f - Vector3.Distance(exitPosition, currentPosition) / Vector3.Distance(exitPosition, originalPosiion);

        return Vector3.Lerp(currentPosition, originalPosiion, exitMovement.Evaluate(progress) * delta);
    }

    public Vector3 CalculateNextExitPosition(Vector3 currentPosition, Vector3 originalPosiion, float delta, Canvas canvas)
    {
        Vector3 exitPosition = CalculateExitPosition(originalPosiion, canvas);
        return CalculateNextExitPosition(currentPosition, originalPosiion, exitPosition, delta, canvas);
    }

    public Vector3 CalculateNextEnterPosition(Vector3 currentPosition, Vector3 originalPosiion, float delta, Canvas canvas)
    {
        Vector3 exitPosition = CalculateExitPosition(originalPosiion, canvas);
        return CalculateNextEnterPosition(currentPosition, originalPosiion, exitPosition, delta, canvas);
    }


    public enum CanvasAnchorPoint
    {
        CENTER,
        TOP_CENTER,
        BOTTOM_CENTER,
        LEFT_MIDDLE,
        RIGHT_MIDDLE,
        TOP_LEFT,
        TOP_RIGHT,
        BOTTOM_LEFT,
        BOTTOM_RIGHT

    }
}
