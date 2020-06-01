using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class SwitchVR : MonoBehaviour
{
    public void switchToVrMode()
    {
        StartCoroutine(SwitchToVR());
    }

    private IEnumerator SwitchToVR()
    {
        string desiredDevice = "cardboard";
        Camera.main.transform.localRotation = Quaternion.identity;
        if (string.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0)
        {
            XRSettings.LoadDeviceByName(desiredDevice);
            yield return null;
        }
        XRSettings.enabled = true;
    }

    public void switchTo2DMode()
    {
        StartCoroutine(SwitchTo2D());
    }

    private IEnumerator SwitchTo2D()
    {
        XRSettings.LoadDeviceByName(string.Empty);
        yield return null;
        XRSettings.enabled = false;
        ResetCameras();
    }

    private void ResetCameras()
    {
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            Camera camera = Camera.allCameras[i];
            if (camera.enabled)
            {
                camera.transform.localPosition = Vector3.zero;
                camera.transform.localRotation = Quaternion.identity;
            }
        }
        Camera.main.ResetAspect();
    }
}
