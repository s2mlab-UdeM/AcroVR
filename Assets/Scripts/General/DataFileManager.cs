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
				jointsTemp.nodes[ddlNum].interpolation = MainParameters.Instance.interpolationDefault;
				int indexTQ = 2;
				if (values.Length > 5)
				{
					string[] subValues;
					subValues = Regex.Split(values[2], ",");
					if (subValues[0].Contains(MainParameters.InterpolationType.CubicSpline.ToString()))
						jointsTemp.nodes[ddlNum].interpolation.type = MainParameters.InterpolationType.CubicSpline;
					else
						jointsTemp.nodes[ddlNum].interpolation.type = MainParameters.InterpolationType.Quintic;
					jointsTemp.nodes[ddlNum].interpolation.numIntervals = int.Parse(subValues[1]);
					jointsTemp.nodes[ddlNum].interpolation.slope[0] = float.Parse(subValues[2]);
					jointsTemp.nodes[ddlNum].interpolation.slope[1] = float.Parse(subValues[3]);
					indexTQ++;
				}
				jointsTemp.nodes[ddlNum].T = ExtractDataTQ(values[indexTQ]);
				jointsTemp.nodes[ddlNum].Q = ExtractDataTQ(values[indexTQ + 1]);
				jointsTemp.nodes[ddlNum].ddlOppositeSide = -1;
				ddlNum++;
            }
        }

		// Conserver les données dans la structure de la classe MainParameters

		MainParameters.Instance.joints = jointsTemp;
    }

	// =================================================================================================================================================================
	/// <summary> Conserver les angles des articulations (DDL) dans un fichier de données, ainsi que les autres paramètres nécessaires (paramètres de décollage, ...). </summary>

	public void WriteDataFiles(string fileName)
	{
		string fileLines = string.Format(
			"Duration: {0}{1}Condition: {2}{3}VerticalSpeed: {4:0.000}{5}AnteroposteriorSpeed: {6:0.000}{7}SomersaultSpeed: {8:0.000}{9}TwistSpeed: {10:0.000}{11}Tilt: {12:0.000}{13}Rotation: {14:0.000}{15}{16}",
			MainParameters.Instance.joints.duration, System.Environment.NewLine,
			MainParameters.Instance.joints.condition, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.verticalSpeed, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.somersaultSpeed, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.twistSpeed, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.tilt, System.Environment.NewLine,
			MainParameters.Instance.joints.takeOffParam.rotation, System.Environment.NewLine, System.Environment.NewLine);

		fileLines = string.Format("{0}Nodes{1}DDL, name, interpolation (type, numIntervals, slopes), T, Q{2}", fileLines, System.Environment.NewLine, System.Environment.NewLine);

		for (int i = 0; i < MainParameters.Instance.joints.nodes.Length; i++)
		{
			fileLines = string.Format("{0}{1}:{2}:{3},{4},{5:0.000000},{6:0.000000}:", fileLines, i + 1, MainParameters.Instance.joints.nodes[i].name, MainParameters.Instance.joints.nodes[i].interpolation.type,
				MainParameters.Instance.joints.nodes[i].interpolation.numIntervals, MainParameters.Instance.joints.nodes[i].interpolation.slope[0], MainParameters.Instance.joints.nodes[i].interpolation.slope[1]);
			for (int j = 0; j < MainParameters.Instance.joints.nodes[i].T.Length; j++)
			{
				if (j < MainParameters.Instance.joints.nodes[i].T.Length - 1)
					fileLines = string.Format("{0}{1:0.000000},", fileLines, MainParameters.Instance.joints.nodes[i].T[j]);
				else
					fileLines = string.Format("{0}{1:0.000000}:", fileLines, MainParameters.Instance.joints.nodes[i].T[j]);
			}
			for (int j = 0; j < MainParameters.Instance.joints.nodes[i].Q.Length; j++)
			{
				if (j < MainParameters.Instance.joints.nodes[i].Q.Length - 1)
					fileLines = string.Format("{0}{1:0.000000},", fileLines, MainParameters.Instance.joints.nodes[i].Q[j]);
				else
					fileLines = string.Format("{0}{1:0.000000}:{2}", fileLines, MainParameters.Instance.joints.nodes[i].Q[j], System.Environment.NewLine);
			}
		}

		System.IO.File.WriteAllText(fileName, fileLines);
	}

	// =================================================================================================================================================================
	/// <summary> Extraire les données T ou Q (nombres sépérés par des virgules) de la ligne de texte spécifié. </summary>

	float[] ExtractDataTQ(string values)
    {
        string[] subValues = Regex.Split(values, ",");
        float[] data = new float[subValues.Length];
        for (int i = 0; i < subValues.Length; i++)
            data[i] = float.Parse(subValues[i]);
        return data;
    }

}
