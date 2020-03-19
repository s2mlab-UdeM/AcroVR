using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropDownMenu : MonoBehaviour {
    private Rect DropDownRect;
    private Vector2 ListScrollPos;
    private bool DropdownVisible;
    private int SelectedListItem;
    public class GuiListItem //The class that contains our list items
    {
        public bool Selected;
        public string Name;
        public GuiListItem(bool mSelected, string mName)
        {
            Selected = mSelected;
            Name = mName;
        }
        public GuiListItem(string mName)
        {
            Selected = false;
            Name = mName;
        }
        public void enable()
        {
            Selected = true;
        }
        public void disable()
        {
            Selected = false;
        }
    }

    private List<GuiListItem> MyListOfStuff; //Declare our list of stuff

    void Start()
    {
        DropDownRect = new Rect(0, 0, 160, 28);//We need to manually position our list, because the dropdown will appear over other controls
        DropdownVisible = true;
        SelectedListItem = -1;
        MyListOfStuff = new List<GuiListItem>(); //Initialize our list of stuff
        for (int i = 0; i < 32; i++)//Fill it with some stuff
        {
            MyListOfStuff.Add(new GuiListItem("Item Number" + i.ToString()));
        }
    }

    void OnGUI()
    {
        //Show the dropdown list if required (make sure any controls that should appear behind the list are before this block)
        if (DropdownVisible)
        {
            GUILayout.BeginArea(new Rect(DropDownRect.left, DropDownRect.top + DropDownRect.height, 160, 256), "", "box");
            ListScrollPos = GUILayout.BeginScrollView(ListScrollPos, false, true);
            GUILayout.BeginVertical(GUILayout.Width(120));
            for (int i = 0; i < MyListOfStuff.Count; i++)
            {
                if (!MyListOfStuff[i].Selected  && GUILayout.Button(MyListOfStuff[i].Name))
                {
                    if (SelectedListItem != -1) MyListOfStuff[SelectedListItem].disable();//Turn off the previously selected item
                    SelectedListItem = i;//Set the index for our currrently selected item
                    MyListOfStuff[SelectedListItem].enable();//Turn on the item we clicked
                    DropdownVisible = false; //Hide the list
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        //Draw the dropdown control
        GUILayout.BeginArea(DropDownRect, "", "box");
        GUILayout.BeginHorizontal();
        string SelectedItemCaption = (SelectedListItem == -1) ? "Select an item..." : MyListOfStuff[SelectedListItem].Name;
        string ButtonText = (DropdownVisible) ? "<<" : ">>";
        GUILayout.TextField(SelectedItemCaption);
        DropdownVisible = GUILayout.Toggle(DropdownVisible, ButtonText, "button", GUILayout.Width(32), GUILayout.Height(20));
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
}
