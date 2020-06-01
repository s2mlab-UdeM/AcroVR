using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///		UI In-Game menu logic
/// </summary>

public class UIManager : MonoBehaviour
{
    /*    [SerializeField]
        private GameObject uiGO;


        void Start()
        {
            FindGameObjectsWithTag();


        }

        public void FindGameObjectsWithTag()
        {
            if (uiGO == null)
            {
                uiGO = GameObject.FindGameObjectWithTag("UI");

            }

        }*/

    public GameObject panelToolTip;
    GameObject panelToolTipPrefab;
    RectTransform rectTransformBackground;
    Text textToolTip;
    int displayToolTipNum = 0;

    bool tooltipOn = false;

    public void SetTooltip()
    {
        panelToolTipPrefab = (GameObject)Resources.Load("ToolTipCanvas", typeof(GameObject));
        panelToolTipPrefab = Instantiate(panelToolTipPrefab);
        panelToolTip = panelToolTipPrefab.transform.Find("PanelToolTip").gameObject;
        rectTransformBackground = panelToolTip.transform.Find("background").GetComponent<RectTransform>();
        textToolTip = panelToolTip.transform.Find("text").GetComponent<Text>();

        panelToolTip.SetActive(false);
    }

    private int currentTab = 1;

    public void SetCurrentTab(int _num)
    {
        currentTab = _num;
    }

    public int GetCurrentTab()
    {
        return currentTab;
    }

    public void ShowToolTip(int num, GameObject gameObject, string stringToolTip)
    {
        if (tooltipOn)
        {
            if (IsOnGameObject(gameObject))
            {
                panelToolTip.SetActive(true);
                panelToolTip.transform.SetAsLastSibling();

                textToolTip.text = stringToolTip;
                float paddingSize = 4;
                Vector2 backgroundSize = new Vector2(textToolTip.preferredWidth + paddingSize * 2, textToolTip.preferredHeight + paddingSize * 2);
                rectTransformBackground.sizeDelta = backgroundSize;

                Vector2 localPoint = Input.mousePosition;
                panelToolTip.transform.position = localPoint;

                displayToolTipNum = num;
            }
            else if (displayToolTipNum == num)
            {
                HideToolTip();
            }
        }
    }

    public void HideToolTip()
    {
        panelToolTip.SetActive(false);
        displayToolTipNum = 0;
    }

    public bool IsOnGameObject(GameObject gameObject)
    {
        Vector2 inputMousePos = Input.mousePosition;
        Vector3[] menuPos = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetWorldCorners(menuPos);
        Vector3[] gameObjectPos = new Vector3[2];
        gameObjectPos[0] = RectTransformUtility.WorldToScreenPoint(Camera.main, menuPos[0]);
        gameObjectPos[1] = RectTransformUtility.WorldToScreenPoint(Camera.main, menuPos[2]);
        return (gameObject.activeSelf && inputMousePos.x >= gameObjectPos[0].x && inputMousePos.x <= gameObjectPos[1].x && inputMousePos.y >= gameObjectPos[0].y && inputMousePos.y <= gameObjectPos[1].y);
    }

    public void SetTooltip(bool _flag)
    {
        tooltipOn = _flag;
    }
}