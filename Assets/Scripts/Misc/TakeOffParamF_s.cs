using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeOffParamF_s : MonoBehaviour
{
    void OnEnable()
    {
        float value = MainParameters.Instance.joints.takeOffParam.rotation;
        transform.Find("PanelParameters/PanelParametersColumn1/PanelSomersaultPosition").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
        value = MainParameters.Instance.joints.takeOffParam.tilt;
        transform.Find("PanelParameters/PanelParametersColumn1/PanelTilt").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
        value = MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed;
        transform.Find("PanelParameters/PanelParametersColumn2/PanelHorizontalSpeed").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
        value = MainParameters.Instance.joints.takeOffParam.verticalSpeed;
        transform.Find("PanelParameters/PanelParametersColumn2/PanelVerticalSpeed").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
        value = MainParameters.Instance.joints.takeOffParam.somersaultSpeed;
        transform.Find("PanelParameters/PanelParametersColumn2/PanelSomersaultSpeed").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
        value = MainParameters.Instance.joints.takeOffParam.twistSpeed;
        transform.Find("PanelParameters/PanelParametersColumn2/PanelTwistSpeed").gameObject.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
    }

    public void DropDownDDLNamesOnValueChanged(int value)
    {
        ToolBox.GetInstance().GetManager<GameManager>().InterpolationDDL();
        ToolBox.GetInstance().GetManager<GameManager>().DisplayDDL(value, true);
    }

    public void CheckTakeOffParamCondition(Dropdown dropDown)
    {
        MainParameters.Instance.joints.condition = dropDown.value;
    }

    public void CheckTakeOffParam(GameObject panel)
    {
        float value = float.Parse(panel.GetComponentInChildren<InputField>().text);
        if (panel.name == "PanelSomersaultPosition")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
            MainParameters.Instance.joints.takeOffParam.rotation = value;
        }
        else if (panel.name == "PanelTilt")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
            MainParameters.Instance.joints.takeOffParam.tilt = value;
        }
        else if (panel.name == "PanelHorizontalSpeed")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
            MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed = value;
        }
        else if (panel.name == "PanelVerticalSpeed")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.0}", value);
            MainParameters.Instance.joints.takeOffParam.verticalSpeed = value;
        }
        else if (panel.name == "PanelSomersaultSpeed")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
            MainParameters.Instance.joints.takeOffParam.somersaultSpeed = value;
        }
        else if (panel.name == "PanelTwistSpeed")
        {
            panel.GetComponentInChildren<InputField>().text = string.Format("{0:0.000}", value);
            MainParameters.Instance.joints.takeOffParam.twistSpeed = value;
        }
    }
}
