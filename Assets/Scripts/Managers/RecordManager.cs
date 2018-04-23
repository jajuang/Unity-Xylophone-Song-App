using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
	public static RecordManager _instance;

	public PlayState MyPlayState;
	public float Duration = 5.0f;
	public float TimeSoFar;

	public GameObject LineObj;
	public Transform LineTransform;
	public Transform LineStartPos;
	public Transform LineEndPos;

	private Queue<Note> NotesToPlay;
	private AudioSource MyAudioSource;
	private Transform MyLineStartPos;
	private Transform MyLineEndPos;

	public GameObject PlayButton;
	public GameObject StopButton;

	public List<Object> KeyList;


	void Start()
	{
		_instance = this;
		LineTransform = LineObj.gameObject.transform;
		NotesToPlay = new Queue<Note>();
		MyAudioSource = this.GetComponent<AudioSource>();
		FilterManager._instance.Reverse(false);
	}
	
	public void TogglePlayButton(bool isPlay)
	{
		PlayButton.SetActive(!isPlay);
		StopButton.SetActive(isPlay);
	}

	public void PlayMusic(bool isPlay)
	{
		if (MyPlayState != PlayState.Default || !isPlay) // Already Playing or Recording
		{
			EndPlayOrRecord();
			return;
		}
		else
		{
			MyPlayState = PlayState.Playing;
			TogglePlayButton(true);
			NotesToPlay = new Queue<Note>();
			
			// Disable recording keys
			foreach(GameObject key in KeyList)
			{
				key.GetComponent<Button>().enabled = false;
				StartCoroutine(FadeImageColor(key.GetComponent<Image>(), Color.grey));
			}

			// Populate Notes stack
			if (FilterManager._instance.IsReverse)
			{
				for (var i = NotesManager._instance.NotesList.Count - 1; i >= 0; i--)
				{
					Note note = new Note(NotesManager._instance.NotesList[i]);
					note.NoteTime = Duration - note.NoteTime;
					NotesToPlay.Enqueue(note);
				}
			}
			else
			{
				for (var i = 0; i < NotesManager._instance.NotesList.Count; i++)
				{
					NotesToPlay.Enqueue(NotesManager._instance.NotesList[i]);
				}
			}

			StartCoroutine(MoveLine(true));
			FilterManager._instance.EnableFilters(false);
		}
	}

	public void EndPlayOrRecord()
	{
		TogglePlayButton(false);

		if (MyPlayState == PlayState.Playing)
		{
			// Re-enable recording keys
			foreach (GameObject key in KeyList)
			{
				key.GetComponent<Button>().enabled = true;
				StartCoroutine(FadeImageColor(key.GetComponent<Image>(), Color.white));
			}
		}

		MyPlayState = PlayState.Default;
		TimeSoFar = 0;
		LineTransform.position = MyLineStartPos.position;
		NotesToPlay = new Queue<Note>();

		FilterManager._instance.EnableFilters(true);

		// un-reverse for recordings
		FilterManager._instance.Reverse(FilterManager._instance.ReverseToggleObj.isOn);
	}


	public IEnumerator FadeImageColor(Image image, Color endColor)
	{
		float elapsedTime = 0.0f;
		float totalTime = 0.30f;
		Color startColor = image.color;

		while (elapsedTime < totalTime)
		{
			elapsedTime += Time.deltaTime;
			image.color = Color.Lerp(startColor, endColor, (elapsedTime / totalTime));
			yield return null;
		}
	}

	public void StartRecording()
	{
		if (MyPlayState == PlayState.Recording || MyPlayState == PlayState.Playing) // Already recording
		{
			return;
		}
		
		NotesManager._instance.ClearNotes();

		MyPlayState = PlayState.Recording;
		TogglePlayButton(true);
		FilterManager._instance.QuantizationMs = 0;
		FilterManager._instance.DefaultQuantizationToggle.isOn = true;
		FilterManager._instance.Reverse(false);
		FilterManager._instance.EnableFilters(false);
		StartCoroutine(MoveLine(false));
	}
	
	public IEnumerator MoveLine(bool isPlayMusic)
	{
		// Reset line
		LineObj.transform.position = MyLineStartPos.position;
		TimeSoFar = 0;
		Note currNote = null;

		if (NotesToPlay.Count > 0)
		{
			currNote = NotesToPlay.Dequeue();
		}

		// Move Line
		while ((MyPlayState != PlayState.Default) && (TimeSoFar < Duration))
		{
			LineTransform.position = Vector3.Lerp(MyLineStartPos.position, MyLineEndPos.position, (TimeSoFar / Duration));
			TimeSoFar += Time.deltaTime;

			if (isPlayMusic && currNote != null)
			{
				float currNoteTime = currNote.NoteTime;
				if (FilterManager._instance.QuantizationMs != 0)
				{
					// Quantization
					var quantizationInSec = (FilterManager._instance.QuantizationMs / 1000);
					float denominator = (1 / quantizationInSec);
					currNoteTime = Mathf.Round(currNoteTime * denominator) / denominator;
				}

				if ((currNoteTime <= TimeSoFar))
				{
					NotesManager._instance.PlayNote(currNote);

					// Get next note
					if (NotesToPlay.Count > 0)
					{
						currNote = NotesToPlay.Dequeue();
					}
					else
					{
						currNote = null;
					}
				}
			}
			yield return new WaitForEndOfFrame();
		}
		EndPlayOrRecord();
	}


	public void ReverseLine(bool enable)
	{
		if (enable)
		{
			// Right to left
			MyLineStartPos = LineEndPos;
			MyLineEndPos = LineStartPos;
		}
		else
		{
			// Left to right
			MyLineStartPos = LineStartPos;
			MyLineEndPos = LineEndPos;
		}
	}
}
