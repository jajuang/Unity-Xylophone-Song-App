using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
	public Note Note;
	
	public void OnClick()
	{
		RecordManager._instance.StartRecording();
		NotesManager._instance.AddNote(new Note(Note));
	}
}
