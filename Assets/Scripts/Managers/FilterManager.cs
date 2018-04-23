using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterManager : MonoBehaviour
{
	public static FilterManager _instance;

	public bool IsEcho;
	public bool IsReverse;
	public float QuantizationMs;

	public Toggle EchoToggleObj;
	public Toggle ReverseToggleObj;
	public ToggleGroup QuantizationToggleGroup;
	public List<Toggle> QuantizationIndvToggles;
	public Toggle DefaultQuantizationToggle;

	public List<GameObject> IntervalsList; // Assumes that the Intervals are in order


	void Awake()
	{
		_instance = this;
	}

	public void Echo(bool enable)
	{
		IsEcho = enable;
	}

	public void Reverse(bool enable)
	{
		IsReverse = enable;
		RecordManager._instance.ReverseLine(enable);
	}

	public void EnableFilters(bool isEnabled)
	{
		EchoToggleObj.interactable = isEnabled;
		ReverseToggleObj.interactable = isEnabled;

		foreach (Toggle toggle in QuantizationIndvToggles)
		{
			toggle.interactable = isEnabled;
		}
	}
	
	public void SetActiveQuantization()
	{
		float.TryParse(QuantizationToggleGroup.ActiveToggles().FirstOrDefault().name, out QuantizationMs);
		if (RecordManager._instance.MyPlayState != PlayState.Recording)
		{
			MoveNotesViaQuantization(QuantizationMs);
		}
	}

	public void MoveNotesViaQuantization(float ms)
	{
		var quantizationInSec = (QuantizationMs / 1000);
		float denominator = (1 / quantizationInSec);

		for (int i = 0; i < NotesManager._instance.NotesContainer.transform.childCount; i++)
		{
			// NOTE: we assume the same number and order of gameobjects as in the noteslist (which it should be)
			GameObject noteGO = NotesManager._instance.NotesContainer.transform.GetChild(i).gameObject;
			Note note = NotesManager._instance.NotesList[i];

			int minIntervalBetween = (int) Math.Truncate(note.NoteTime);
			int maxIntervalBetween = minIntervalBetween + 1;

			float nearestFraction = 0;
			if (QuantizationMs == 0)
			{
				// original "none" position
				nearestFraction = (note.NoteTime - minIntervalBetween);
			}
			else
			{
				// round to nearest fraction
				nearestFraction = Mathf.Round((note.NoteTime - minIntervalBetween) * denominator) / denominator;
			}
			
			if (minIntervalBetween == 5)
			{
				noteGO.transform.position = new Vector3(IntervalsList[5].transform.position.x, noteGO.transform.position.y, noteGO.transform.position.z);
			}
			else
			{
				// Move note gameobject to fraction postion
				// min position + difference of min and max positions for accurate unity x position + fraction
				float xPos = IntervalsList[minIntervalBetween].transform.position.x + ((IntervalsList[maxIntervalBetween].transform.position.x - IntervalsList[minIntervalBetween].transform.position.x ) * nearestFraction);				
				noteGO.transform.position = new Vector3(xPos, noteGO.transform.position.y, noteGO.transform.position.z);
			}
		}
	}

}
