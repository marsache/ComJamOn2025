using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BeatSystem : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;

    public static int beat;
    public static string marker;

    public string songName;
    public GameObject[] arrowPrefab; // Reference to the arrow prefab
    public Transform[] arrowSpawnPoints; // Define the spawn positions for arrows (lanes)

    private void Start()
    {
        // You can assign your FMOD event instance here in Start
        FMOD.Studio.EventInstance instance = FMODUnity.RuntimeManager.CreateInstance("event:/" + songName);
        AssignBeatEvent(instance);
    }

    public void AssignBeatEvent(FMOD.Studio.EventInstance instance)
    {
        timelineInfo = new TimelineInfo();
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        instance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
        instance.start(); // Start the event
    }

    public void StopAndClear(FMOD.Studio.EventInstance instance)
    {
        instance.setUserData(IntPtr.Zero);
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        timelineHandle.Free();
    }

    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBeat = parameter.beat;
                        beat = timelineInfo.currentMusicBeat;
                    }
                    break;

                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                        marker = timelineInfo.lastMarker;

                        // Call method to spawn the arrow based on the marker name
                        BeatSystem instanceBS = GameObject.FindObjectOfType<BeatSystem>();
                        if (instanceBS != null)
                        {
                            instanceBS.SpawnArrow(marker);
                        }
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    // Function to instantiate arrows at the correct time and lane
    public void SpawnArrow(string marker)
    {
        // Determine the lane based on the marker (you can adapt this logic based on your markers and lanes)
        int laneIndex = GetLaneIndexFromMarker(marker);

        if (laneIndex >= 0 && laneIndex < arrowSpawnPoints.Length)
        {
            // Instantiate the arrow prefab at the corresponding spawn point
            Instantiate(arrowPrefab[laneIndex], arrowSpawnPoints[laneIndex].position, Quaternion.identity);
        }
    }

    // Determine the lane index from the marker (you'll need to customize this logic)
    private int GetLaneIndexFromMarker(string marker)
    {
        switch (marker)
        {
            case "G":
                return 0; // Left
            case "D":
                return 1; // Down
            case "A":
                return 2; // Up
            case "E":
                return 3; // Right
            default:
                return -1; // Invalid marker
        }
    }
}
