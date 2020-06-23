using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
	public static ToolTip Instance;
	public GameObject panelToolTip;
	[SerializeField]
	Camera uiCamera = default;
	[SerializeField]
	RectTransform rectTransformCanvas = default;

	int displayToolTipNum;
	Text textToolTip;
	RectTransform rectTransformBackground;

	void Awake ()
	{
		Instance = this;
		displayToolTipNum = 0;

		rectTransformBackground = panelToolTip.transform.Find("background").GetComponent<RectTransform>();
		textToolTip = panelToolTip.transform.Find("text").GetComponent<Text>();
	}

	void Update()
	{
		ShowToolTip(1, Main.Instance.buttonToolTips.gameObject, Main.Instance.buttonToolTips.interactable, MainParameters.Instance.languages.Used.toolTipButtonToolTips);
		ShowToolTip(2, Main.Instance.buttonLanguage.gameObject, Main.Instance.buttonLanguage.interactable, MainParameters.Instance.languages.Used.toolTipButtonLanguage);
		ShowToolTip(3, Main.Instance.buttonQuit, Main.Instance.buttonQuit.GetComponent<Button>().interactable, MainParameters.Instance.languages.Used.toolTipButtonQuit);

		bool enable = MainParameters.Instance.joints.nodes != null && !GraphSettings.Instance.panelGraphSettings.activeSelf && !GraphManager.Instance.panelAddRemoveNode.activeSelf &&
			!GraphManager.Instance.panelCancelChanges.activeSelf && !GraphManager.Instance.panelMoveErrMsg.activeSelf;
		ShowToolTip(10, MovementF.Instance.panelGraph, enable, MainParameters.Instance.languages.Used.toolTipPanelGraph);
		enable = MainParameters.Instance.joints.nodes != null && GraphManager.Instance.panelAddRemoveNode.activeSelf;
		ShowToolTip(11, GraphManager.Instance.buttonAddNode, enable, MainParameters.Instance.languages.Used.toolTipButtonAddNode);
		ShowToolTip(12, GraphManager.Instance.buttonRemoveNode, enable, MainParameters.Instance.languages.Used.toolTipButtonRemoveNode);
		ShowToolTip(13, GraphManager.Instance.buttonCancelChanges1, enable, MainParameters.Instance.languages.Used.toolTipButtonCancelChanges);
		enable = MainParameters.Instance.joints.nodes != null && GraphManager.Instance.panelCancelChanges.activeSelf;
		ShowToolTip(14, GraphManager.Instance.buttonCancelChanges2, enable, MainParameters.Instance.languages.Used.toolTipButtonCancelChanges);
		ShowToolTip(15, MovementF.Instance.dropDownDDLNames.gameObject, MovementF.Instance.dropDownDDLNames.interactable, MainParameters.Instance.languages.Used.toolTipDropDownDDLNames);
		ShowToolTip(16, MovementF.Instance.dropDownInterpolation.gameObject, MovementF.Instance.dropDownInterpolation.interactable, MainParameters.Instance.languages.Used.toolTipDropDownInterpolation);
		ShowToolTip(17, MovementF.Instance.buttonLoad.gameObject, MovementF.Instance.buttonLoad.interactable, MainParameters.Instance.languages.Used.toolTipButtonLoad);
		ShowToolTip(18, MovementF.Instance.buttonSave.gameObject, MovementF.Instance.buttonSave.interactable, MainParameters.Instance.languages.Used.toolTipButtonSave);
		ShowToolTip(19, MovementF.Instance.buttonSymetricLeftRight.gameObject, MovementF.Instance.buttonSymetricLeftRight.interactable, MainParameters.Instance.languages.Used.toolTipButtonSymetricLeftRight);
		ShowToolTip(20, MovementF.Instance.buttonASymetricLeftRight.gameObject, MovementF.Instance.buttonASymetricLeftRight.interactable, MainParameters.Instance.languages.Used.toolTipButtonSymetricLeftRight);
		ShowToolTip(21, MovementF.Instance.buttonGraphSettings.gameObject, MovementF.Instance.buttonGraphSettings.interactable, MainParameters.Instance.languages.Used.toolTipButtonGraphSettings);

		if (GraphSettings.Instance.panelGraphSettings.activeSelf)
		{
			ShowToolTip(30, GraphSettings.Instance.inputFieldVerticalAxiesLowerBound.gameObject, GraphSettings.Instance.inputFieldVerticalAxiesLowerBound.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalLowerBound);
			ShowToolTip(31, GraphSettings.Instance.inputFieldVerticalAxiesUpperBound.gameObject, GraphSettings.Instance.inputFieldVerticalAxiesUpperBound.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalUpperBound);
			ShowToolTip(32, GraphSettings.Instance.verticalAxisSlider.gameObject, GraphSettings.Instance.verticalAxisSlider.GetComponent<Slider>().interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalSlider);
			ShowToolTip(33, GraphSettings.Instance.buttonVerticalLowerBoundMinus.gameObject, GraphSettings.Instance.buttonVerticalLowerBoundMinus.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalLowerMinus10);
			ShowToolTip(34, GraphSettings.Instance.buttonVerticalLowerBoundPlus.gameObject, GraphSettings.Instance.buttonVerticalLowerBoundPlus.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalLowerPlus10);
			ShowToolTip(35, GraphSettings.Instance.buttonVerticalUpperBoundMinus.gameObject, GraphSettings.Instance.buttonVerticalUpperBoundMinus.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalUpperMinus10);
			ShowToolTip(36, GraphSettings.Instance.buttonVerticalUpperBoundPlus.gameObject, GraphSettings.Instance.buttonVerticalUpperBoundPlus.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsVerticalUpperPlus10);
			ShowToolTip(37, GraphSettings.Instance.inputFieldHorizontalAxiesUpperBound.gameObject, GraphSettings.Instance.inputFieldHorizontalAxiesUpperBound.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsHorizontalUpperBound);
			ShowToolTip(38, GraphSettings.Instance.horizontalAxisSlider.gameObject, GraphSettings.Instance.horizontalAxisSlider.GetComponent<Slider>().interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsHorizontalSlider);
			ShowToolTip(39, GraphSettings.Instance.toggleGraphSettingsUpdateSimulation.gameObject, GraphSettings.Instance.toggleGraphSettingsUpdateSimulation.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsUpdateSimulation);
			ShowToolTip(40, GraphSettings.Instance.buttonGraphSettingsDefaultValues.gameObject, GraphSettings.Instance.buttonGraphSettingsDefaultValues.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsDefaultValues);
			ShowToolTip(41, GraphSettings.Instance.buttonGraphSettingsCancel.gameObject, GraphSettings.Instance.buttonGraphSettingsCancel.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsCancel);
			ShowToolTip(42, GraphSettings.Instance.buttonGraphSettingsOK.gameObject, GraphSettings.Instance.buttonGraphSettingsOK.interactable, MainParameters.Instance.languages.Used.toolTipGraphSettingsOK);
		}
		else if (displayToolTipNum >= 30 && displayToolTipNum <= 49)
			HideToolTip();

		ShowToolTip(50, MovementF.Instance.dropDownCondition.gameObject, MovementF.Instance.dropDownCondition.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffCondition);
		ShowToolTip(51, MovementF.Instance.dropDownInitialPosture.gameObject, MovementF.Instance.dropDownInitialPosture.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffInitialPosture);
		ShowToolTip(52, MovementF.Instance.inputFieldSomersaultPosition.gameObject, MovementF.Instance.inputFieldSomersaultPosition.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffSomersaultPosition);
		ShowToolTip(53, MovementF.Instance.inputFieldTilt.gameObject, MovementF.Instance.inputFieldTilt.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffTilt);
		ShowToolTip(54, MovementF.Instance.inputFieldHorizontalSpeed.gameObject, MovementF.Instance.inputFieldHorizontalSpeed.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffHorizontal);
		ShowToolTip(55, MovementF.Instance.inputFieldVerticalSpeed.gameObject, MovementF.Instance.inputFieldVerticalSpeed.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffVertical);
		ShowToolTip(56, MovementF.Instance.inputFieldSomersaultSpeed.gameObject, MovementF.Instance.inputFieldSomersaultSpeed.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffSomersaultSpeed);
		ShowToolTip(57, MovementF.Instance.inputFieldTwistSpeed.gameObject, MovementF.Instance.inputFieldTwistSpeed.interactable, MainParameters.Instance.languages.Used.toolTipTakeOffTwist);

		ShowToolTip(60, AnimationF.Instance.dropDownPlayMode.gameObject, AnimationF.Instance.dropDownPlayMode.interactable, MainParameters.Instance.languages.Used.toolTipDropDownPlayMode);
		ShowToolTip(61, AnimationF.Instance.dropDownPlayView.gameObject, AnimationF.Instance.dropDownPlayView.interactable, MainParameters.Instance.languages.Used.toolTipDropDownPlayView);
		ShowToolTip(62, AnimationF.Instance.dropDownPlaySpeed.gameObject, AnimationF.Instance.dropDownPlaySpeed.interactable, MainParameters.Instance.languages.Used.toolTipDropDownPlaySpeed);
		ShowToolTip(63, AnimationF.Instance.buttonPlay.gameObject, AnimationF.Instance.buttonPlay.interactable, MainParameters.Instance.languages.Used.toolTipButtonPlay);
		ShowToolTip(64, AnimationF.Instance.buttonStop.gameObject, AnimationF.Instance.buttonStop.GetComponent<Button>().interactable, MainParameters.Instance.languages.Used.toolTipButtonStop);
		ShowToolTip(65, AnimationF.Instance.buttonGraph.gameObject, AnimationF.Instance.buttonGraph.interactable, MainParameters.Instance.languages.Used.toolTipButtonGraph);

		if (displayToolTipNum > 0)
		{
			Vector2 localPoint;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(panelToolTip.transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
			panelToolTip.transform.localPosition = localPoint;

			Vector2 anchoredPosition = panelToolTip.transform.GetComponent<RectTransform>().anchoredPosition;
			if (anchoredPosition.x + rectTransformBackground.rect.width > rectTransformCanvas.rect.width)
				anchoredPosition.x = rectTransformCanvas.rect.width - rectTransformBackground.rect.width;
			if (anchoredPosition.y + rectTransformBackground.rect.height > rectTransformCanvas.rect.height)
				anchoredPosition.y = rectTransformCanvas.rect.height - rectTransformBackground.rect.height;
			panelToolTip.transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
		}
	}
	
	public void ShowToolTip (int num, GameObject gameObject, bool enable, string stringToolTip)
	{
		if (displayToolTipNum <= 0 && enable && Main.Instance.toolTipsON && MouseManager.Instance.IsOnGameObject(gameObject))
		{
			panelToolTip.SetActive(true);
			panelToolTip.transform.SetAsLastSibling();

			textToolTip.text = stringToolTip;
			float paddingSize = 4;
			Vector2 backgroundSize = new Vector2(textToolTip.preferredWidth + paddingSize * 2, textToolTip.preferredHeight + paddingSize * 2);
			rectTransformBackground.sizeDelta = backgroundSize;
			displayToolTipNum = num;
		}
		else if (displayToolTipNum == num && (!enable || !MouseManager.Instance.IsOnGameObject(gameObject)))
			HideToolTip();
	}

	public void HideToolTip()
	{
		panelToolTip.SetActive(false);
		displayToolTipNum = 0;
	}
}
