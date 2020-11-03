using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

//[System.Serializable]
//public class IntUnityEvent : UnityEvent<int>
//{
//    public int intParam;
//}

public class clapTracker_unityEvent : MonoBehaviour
{
    public float Interval = .1f; //seconds
    public UnityEvent onClapEvent;
    
    private LeapProvider provider = null;
    private bool velocityThresholdExceeded = false;

    public float Proximity = 0.1f; //meters
    public float VelocityThreshold = 0.1f; //meters/s
    public float PalmAngleLimit = 75; //degrees

#if UNITY_EDITOR
    //Debugging variables --set Inspector to debug mode
    private float currentAngle = 0;
    private float currentVelocityVectorAngle = 0;
    private float currentDistance = 0;
    private float currentVelocity = 0;
#endif

    void Start()
    {
        //Find LeapEngine provider
        provider = GetComponentInChildren<LeapServiceProvider>();
        if (provider == null)
        {
            provider = this.transform.parent.GetComponentInChildren<LeapServiceProvider>();
        }
        if (provider == null)
        {
            provider = GameObject.Find("Leap Rig").GetComponentInChildren<LeapServiceProvider>();
        }
    }

    void Update()
    {
        Hand thisHand;
        Hand thatHand;
        if (provider == null)
        {
            Debug.Log("clapTracker_unityEvent: Provider is null. Check components.");
            return;
        }
        Frame frame = provider.CurrentFrame;
        if (frame != null && frame.Hands.Count >= 2)
        {
            thisHand = frame.Hands[0];
            thatHand = frame.Hands[1];
            if (thisHand != null && thatHand != null)
            {
                Vector velocityDirection = thisHand.PalmVelocity.Normalized;
                Vector otherhandDirection = (thisHand.PalmPosition - thatHand.PalmPosition).Normalized;

#if UNITY_EDITOR
                //for debugging
                Debug.DrawRay(thisHand.PalmPosition.ToVector3(), velocityDirection.ToVector3());
                Debug.DrawRay(thatHand.PalmPosition.ToVector3(), otherhandDirection.ToVector3());
                currentAngle = thisHand.PalmNormal.AngleTo(thatHand.PalmNormal) * Constants.RAD_TO_DEG;
                currentDistance = thisHand.PalmPosition.DistanceTo(thatHand.PalmPosition);
                currentVelocity = thisHand.PalmVelocity.MagnitudeSquared + thatHand.PalmVelocity.MagnitudeSquared;
                currentVelocityVectorAngle = velocityDirection.AngleTo(otherhandDirection) * Constants.RAD_TO_DEG;
#endif

                if (thisHand.PalmVelocity.MagnitudeSquared + thatHand.PalmVelocity.MagnitudeSquared > VelocityThreshold &&
                  velocityDirection.AngleTo(otherhandDirection) >= (180 - PalmAngleLimit) * Constants.DEG_TO_RAD)
                {
                    velocityThresholdExceeded = true;
                }
            }
        }
    }

    void OnEnable()
    {
        StartCoroutine(clapWatcher());
    }

    void OnDisable()
    {
        StopCoroutine(clapWatcher());
    }

    IEnumerator clapWatcher()
    {
        Hand thisHand;
        Hand thatHand;
        while (true)
        {
            if (provider)
            {
                Frame frame = provider.CurrentFrame;
                if (frame != null && frame.Hands.Count >= 2)
                {
                    thisHand = frame.Hands[0];
                    thatHand = frame.Hands[1];
                    if (thisHand != null && thatHand != null)
                    {
                        //decide if clapped
                        if (velocityThresholdExceeded && //went fast enough
                                  thisHand.PalmPosition.DistanceTo(thatHand.PalmPosition) < Proximity && // and got close 
                                  thisHand.PalmNormal.AngleTo(thatHand.PalmNormal) >= (180 - PalmAngleLimit) * Constants.DEG_TO_RAD) //while facing each other
                        {
                            if (onClapEvent != null) onClapEvent.Invoke();
                            else Debug.LogWarning("clapTracker_unityEvent: onClapEvent is null.");
                        }
                    }
                }
            }
            velocityThresholdExceeded = false;
            yield return new WaitForSeconds(Interval);
        }
    }
}