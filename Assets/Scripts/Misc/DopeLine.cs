using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class DopeLine
{
    private int m_HierarchyNodeID;
 
    public Rect position;
    public System.Type objectType;
    public bool tallMode;
    public bool hasChildren;
    public bool isMasterDopeline;

    public DopeLine(Rect pos)
    {
        position = pos;
    }

    public int hierarchyNodeID
    {
        get
        {
            return m_HierarchyNodeID;
        }
    }
}
