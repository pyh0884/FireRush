using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using DigitalRubyShared;


public class FingerInitiate : MonoBehaviour
{
    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;
    private SwipeGestureRecognizer swipeGesture;
    private LongPressGestureRecognizer longPressGesture;

    private InteractiveObject activeObject;
    private string longPressTag;

    private float gestureRayCastLength = 1000f;
    private int gestureRayCastLayerMasks;

    AudioSystem audio_gesture;
    [SerializeField] AudioClip[] tapAudioClip;
    [SerializeField] AudioClip[] doubleTapAudioClip;


    /********************** Gesture Funtions Start**********************/
    /* Create: use for gesture initiate
     * Callback: use for calling to react to the gesture    
     * 
     * Gestures List:
     * Tap
     * Double Tap
     * Swipe
     */

    /********************* Tap ********************/
    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    private void TapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugExtension.DebugText("Tap Screen at {0}, {1}", gesture.FocusX, gesture.FocusY);
            HandleTapGetsture(gesture.FocusX, gesture.FocusY);
        }
    }

    private void HandleTapGetsture(float screenX, float screenY)
    {
        Vector3 screenPos = new Vector3(screenX, screenY,0f);
        screenPos = Camera.main.ScreenToWorldPoint(screenPos);

        RaycastHit2D rayHit;


        if (rayHit = Physics2D.Raycast(screenPos, Vector2.zero, gestureRayCastLength, gestureRayCastLayerMasks))
        {
            try
            {
                rayHit.collider.gameObject.GetComponent<InteractiveObject>().Tap(rayHit.point.x, rayHit.point.y);

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.Log("Missing InteractiveObject Component on " + rayHit.collider.gameObject.name);

                audio_gesture.PlayAudio(doubleTapAudioClip); //play the click sound effect if click nothing
            }
        }

    }

    /********************* Double Tap ********************/
    private void CreateDoubleTapGesture()
    {
        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
        FingersScript.Instance.AddGesture(doubleTapGesture);
    }


    private void DoubleTapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugExtension.DebugText("Double tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
            HandleDoubleTapGesture(gesture.FocusX, gesture.FocusY);
        }
    }

    private void HandleDoubleTapGesture(float screenX, float screenY)
    {
        Vector3 screenPos = new Vector3(screenX, screenY, 0f);
        screenPos = Camera.main.ScreenToWorldPoint(screenPos);

        RaycastHit2D rayHit;


        if (rayHit = Physics2D.Raycast(screenPos, Vector2.zero, gestureRayCastLength, gestureRayCastLayerMasks))
        {
            try
            {
                rayHit.collider.gameObject.GetComponent<InteractiveObject>().DoubleTap(rayHit.point.x, rayHit.point.y);

            }
            catch
            {
                Debug.Log("Missing InteractiveObject Component on " + rayHit.collider.gameObject.name);
                
                audio_gesture.PlayAudio(doubleTapAudioClip); //play the click sound effect if click nothing
            }
        }
    }

    /********************* Swipe ********************/
    private void CreateSwipeGesture()
    {
        swipeGesture = new SwipeGestureRecognizer();
        swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
        swipeGesture.StateUpdated += SwipeGestureCallback;
        swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        FingersScript.Instance.AddGesture(swipeGesture);
    }

    private void SwipeGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugExtension.DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
            HandleSwipe(gesture.StartFocusX, gesture.StartFocusY,gesture.FocusX, gesture.FocusY, swipeGesture.VelocityX, swipeGesture.VelocityY);
        }
    }

    private void HandleSwipe(float startX, float startY, float endX, float endY, float velocityX, float velocityY)
    {
        int loop = 5;
        Vector3 lerp = new Vector3((endX - startX) / 2 / loop, (endY - startY) / 2 / loop, 0f);
        Vector3 mid = new Vector3((endX + startX) / 2, (endY + startY) / 2 , 0f);
        Vector3 start = new Vector3(startX, startY, 0f);

        for (int i = 0; i < loop + 1; i++)
        {
            Vector3 startPos = start + lerp * i;
            startPos = Camera.main.ScreenToWorldPoint(startPos);

            RaycastHit2D rayHit;


            if (rayHit = Physics2D.Raycast(startPos, Vector2.zero, gestureRayCastLength, gestureRayCastLayerMasks))
            {
                try
                {
                    rayHit.collider.gameObject.GetComponent<InteractiveObject>().Swipe(startX, startY, endX, endY, velocityX, velocityY);
                    break;
                }
                catch
                {
                    Debug.Log("Missing InteractiveObject Component on " + rayHit.collider.gameObject.name);
                }
            }
        }
    }


    /********************** Gesture Funtions End**********************/

    private static bool? CaptureGestureHandler(GameObject obj)
    {
        if (obj.name.StartsWith("PassThrough"))
        {
            // allow the pass through for any element named "PassThrough*"
            return false;
        }
        else if (obj.name.StartsWith("NoPass"))
        {

            return true;
        }

        // fall-back to default behavior for anything else
        return null;

    }

    void Start()
    {
        //set gesture raycast layer
        gestureRayCastLayerMasks = 1 << LayerMask.NameToLayer("Interactive") | 1 << LayerMask.NameToLayer("Card");

        //Initiate Finger Gesture at first
        CreateDoubleTapGesture();
        CreateTapGesture();
        CreateSwipeGesture();

        FingersScript.Instance.CaptureGestureHandler = CaptureGestureHandler;

        audio_gesture = GetComponent<AudioSystem>();
    }

}
