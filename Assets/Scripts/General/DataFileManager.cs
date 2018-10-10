using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

// =================================================================================================================================================================
/// <summary> Accès aux fichiers de données du logiciel AcroVR. </summary>

public class DataFileManager : MonoBehaviour
{
    public static DataFileManager Instance;

    // =================================================================================================================================================================
    /// <summary> Initialisation du script. </summary>

    void Start ()
	{
        Instance = this;
    }

    // =================================================================================================================================================================
    /// <summary> Lecture des angles des articulations (DDL) d'un fichier de données. Les données seront conservé dans une structure de classe MainParameters. </summary>

    public void ReadDataFiles(string fileName)
    {
        // Lecture de tout le fichier de données dans un vecteur de chaîne de caractères

        string[] fileLines = System.IO.File.ReadAllLines(fileName);

        // Initialisation de quelques paramètres

        MainParameters.StrucJoints jointsTemp = new MainParameters.StrucJoints();
        jointsTemp.fileName = fileName;
        jointsTemp.nodes = null;
        jointsTemp.duration = 0;
        jointsTemp.condition = 0;
        jointsTemp.takeOffParam.verticalSpeed = 0;
        jointsTemp.takeOffParam.anteroposteriorSpeed = 0;
        jointsTemp.takeOffParam.somersaultSpeed = 0;
        jointsTemp.takeOffParam.twistSpeed = 0;
        jointsTemp.takeOffParam.tilt = 0;
        jointsTemp.takeOffParam.rotation = 0;

        string[] values;
        int ddlNum = -1;

        // Vérifier chacune des lignes pour localiser et conserver les paramètres désirés

        for (int i = 0; i < fileLines.Length; i++)
        {
            values = Regex.Split(fileLines[i], ":");
            if (values[0].Contains("Duration"))
            {
                jointsTemp.duration = float.Parse(values[1]);
				if (jointsTemp.duration == -999)
                    jointsTemp.duration = MainParameters.Instance.durationDefault;
            }
            else if (values[0].Contains("Condition"))
            {
                jointsTemp.condition = int.Parse(values[1]);
                if (jointsTemp.condition == -999)
                    jointsTemp.condition = MainParameters.Instance.conditionDefault;
            }
            else if (values[0].Contains("VerticalSpeed"))
            {
                jointsTemp.takeOffParam.verticalSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.verticalSpeed == -999)
                    jointsTemp.takeOffParam.verticalSpeed = MainParameters.Instance.takeOffParamDefault.verticalSpeed;
            }
            else if (values[0].Contains("AnteroposteriorSpeed"))
            {
                jointsTemp.takeOffParam.anteroposteriorSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.anteroposteriorSpeed == -999)
                    jointsTemp.takeOffParam.anteroposteriorSpeed = MainParameters.Instance.takeOffParamDefault.anteroposteriorSpeed;
            }
            else if (values[0].Contains("SomersaultSpeed"))
            {
                jointsTemp.takeOffParam.somersaultSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.somersaultSpeed == -999)
                    jointsTemp.takeOffParam.somersaultSpeed = MainParameters.Instance.takeOffParamDefault.somersaultSpeed;
            }
            else if (values[0].Contains("TwistSpeed"))
            {
                jointsTemp.takeOffParam.twistSpeed = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.twistSpeed == -999)
                    jointsTemp.takeOffParam.twistSpeed = MainParameters.Instance.takeOffParamDefault.twistSpeed;
            }
            else if (values[0].Contains("Tilt"))
            {
                jointsTemp.takeOffParam.tilt = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.tilt == -999)
                    jointsTemp.takeOffParam.tilt = MainParameters.Instance.takeOffParamDefault.tilt;
            }
            else if (values[0].Contains("Rotation"))
            {
                jointsTemp.takeOffParam.rotation = float.Parse(values[1]);
                if (jointsTemp.takeOffParam.rotation == -999)
                    jointsTemp.takeOffParam.rotation = MainParameters.Instance.takeOffParamDefault.rotation;
            }
            else if (values[0].Contains("DDL"))
            {
                jointsTemp.nodes = new MainParameters.StrucNodes[fileLines.Length - i - 1];
                ddlNum = 0;
            }
            else if (ddlNum >= 0)
            {
                jointsTemp.nodes[ddlNum].ddl = int.Parse(values[0]);
                jointsTemp.nodes[ddlNum].name = values[1];
                jointsTemp.nodes[ddlNum].T = ExtractDataTQ(values[2]);
                jointsTemp.nodes[ddlNum].Q = ExtractDataTQ(values[3]);
				jointsTemp.nodes[ddlNum].interpolation = MainParameters.Instance.interpolationDefault;
				jointsTemp.nodes[ddlNum].ddlOppositeSide = -1;
				ddlNum++;
            }
        }

		// Conserver les données dans la structure de la classe MainParameters

		MainParameters.Instance.joints = jointsTemp;
    }

    float[] ExtractDataTQ(string values)
    {
        string[] subValues = Regex.Split(values, ",");
        float[] data = new float[subValues.Length];
        for (int i = 0; i < subValues.Length; i++)
            data[i] = float.Parse(subValues[i]);
        return data;
    }

}
