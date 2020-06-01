using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingTooltip : MonoBehaviour
{
    public Text textTakeOffTitle;
    public Text textTakeOffHorizontalSpeed;
    public Text textTakeOffVerticalSpeed;
    public Text textTakeOffSetupTitle;
    public Text textTakeOffSomersaultPosition;
    public Text textTakeOffInitialPosture;
    public Text textTakeOffSomersaultSpeed;
    public Text textTakeOffTilt;
    public Text textTakeOffTwistSpeed;

    public Dropdown dropDownTakeOffCondition;

    private void Start()
    {
        ChangedLanguage();
        ToolBox.GetInstance().GetManager<UIManager>().SetTooltip();
    }

    private void ChangedLanguage()
    {
        MainParameters.StrucMessageLists languagesUsed = MainParameters.Instance.languages.Used;

        if (MainParameters.Instance.joints.nodes != null)
        {
            MovementF.Instance.DisplayDDL(-1, false);
            MovementF.Instance.InitDropdownDDLNames(-1);
            MovementF.Instance.InitDropdownInterpolation(-1);
            if (MainParameters.Instance.joints.nodes[GraphManager.Instance.ddlUsed].ddlOppositeSide >= 0)
            {
                MovementF.Instance.textCurveName1.text = languagesUsed.leftSide;
                MovementF.Instance.textCurveName2.text = languagesUsed.rightSide;
            }
        }
        /*        GraphSettings.Instance.textVerticalAxisTitle.text = languagesUsed.movementGraphSettingsVerticalTitle;
                GraphSettings.Instance.textVerticalAxisLowerBound.text = languagesUsed.movementGraphSettingsLowerBound;
                GraphSettings.Instance.textVerticalAxisUpperBound.text = languagesUsed.movementGraphSettingsUpperBound;
                GraphSettings.Instance.textHorizontalAxisTitle.text = languagesUsed.movementGraphSettingsHorizontalTitle;
                GraphSettings.Instance.textHorizontalAxisLowerBound.text = languagesUsed.movementGraphSettingsLowerBound;
                GraphSettings.Instance.textHorizontalAxisUpperBound.text = languagesUsed.movementGraphSettingsUpperBound;
                GraphSettings.Instance.toggleGraphSettingsUpdateSimulation.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsUpdateSimulation;
                GraphSettings.Instance.buttonGraphSettingsDefaultValues.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsDefaultValuesButton;
                GraphSettings.Instance.buttonGraphSettingsCancel.GetComponentInChildren<Text>().text = languagesUsed.movementGraphSettingsCancelButton;*/

        textTakeOffTitle.text = languagesUsed.takeOffTitle;
        //		textTakeOffSpeed.text = languagesUsed.takeOffTitleSpeed;
        List<string> dropDownOptions = new List<string>();
        dropDownOptions.Add(languagesUsed.takeOffConditionNoGravity);
        dropDownOptions.Add(languagesUsed.takeOffConditionTrampolining);
        dropDownOptions.Add(languagesUsed.takeOffConditionTumbling);
        dropDownOptions.Add(languagesUsed.takeOffConditionDiving1m);
        dropDownOptions.Add(languagesUsed.takeOffConditionDiving3m);
        dropDownOptions.Add(languagesUsed.takeOffConditionDiving5m);
        dropDownOptions.Add(languagesUsed.takeOffConditionDiving10m);
        dropDownOptions.Add(languagesUsed.takeOffConditionHighBar);
        dropDownOptions.Add(languagesUsed.takeOffConditionUnevenBars);
        dropDownOptions.Add(languagesUsed.takeOffConditionVault);
        dropDownTakeOffCondition.ClearOptions();
        dropDownTakeOffCondition.AddOptions(dropDownOptions);
        textTakeOffInitialPosture.text = languagesUsed.takeOffInitialPosture;
        textTakeOffSomersaultPosition.text = languagesUsed.takeOffSomersaultPosition;
        textTakeOffTilt.text = languagesUsed.takeOffTilt;
        textTakeOffHorizontalSpeed.text = languagesUsed.takeOffHorizontal;
        textTakeOffVerticalSpeed.text = languagesUsed.takeOffVertical;
        textTakeOffSomersaultSpeed.text = languagesUsed.takeOffSomersaultSpeed;
        textTakeOffTwistSpeed.text = languagesUsed.takeOffTwist;

        /*        if (AnimationF.Instance.lineStickFigure != null)
                {
                    AnimationF.Instance.textCurveName1.text = languagesUsed.leftSide;
                    AnimationF.Instance.textCurveName2.text = languagesUsed.rightSide;
                }*/
        dropDownOptions = new List<string>();
        dropDownOptions.Add(languagesUsed.animatorPlaySpeedFast);
        dropDownOptions.Add(languagesUsed.animatorPlaySpeedNormal);
        dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow1);
        dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow2);
        dropDownOptions.Add(languagesUsed.animatorPlaySpeedSlow3);
        //        AnimationF.Instance.dropDownPlaySpeed.ClearOptions();
        //        AnimationF.Instance.dropDownPlaySpeed.AddOptions(dropDownOptions);
    }

    private void Update()
    {
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(1, textTakeOffHorizontalSpeed.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffHorizontal);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(2, textTakeOffVerticalSpeed.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffVertical);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(3, textTakeOffSomersaultPosition.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffSomersaultPosition);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(4, textTakeOffInitialPosture.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffInitialPosture);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(5, textTakeOffSomersaultSpeed.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffSomersaultSpeed);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(6, textTakeOffTilt.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffTilt);
        ToolBox.GetInstance().GetManager<UIManager>().ShowToolTip(7, textTakeOffTwistSpeed.gameObject, MainParameters.Instance.languages.Used.toolTipTakeOffTwist);
    }
}
