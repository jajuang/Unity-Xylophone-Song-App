using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesManager : MonoBehaviour
{
	public static NotesManager _instance;
	public List<Note> NotesList;
	public GameObject NotesContainer;

	public GameObject NotesPrefab;
	public Transform StartNotesPos;
	public float yBuffer = 30f;

	private AudioSource MyAudioSource;
	public AudioSource EchoAudioSource;

	public GameObject SparklesUIPrefab;


	void Start()
	{
		_instance = this;
		MyAudioSource = this.GetComponent<AudioSource>();
	}
	
	public void AddNote(Note note)
	{
		if (RecordManager._instance.MyPlayState != PlayState.Recording)
		{
			return;
		}
		
		note.NoteTime = RecordManager._instance.TimeSoFar;

		// Add Gameobject UI
		GameObject noteObj = Instantiate(NotesPrefab);
		noteObj.GetComponent<Image>().color = note.Color;
		noteObj.transform.SetParent(NotesContainer.transform);
		noteObj.transform.localPosition = new Vector3(RecordManager._instance.LineTransform.localPosition.x, (StartNotesPos.transform.localPosition.y - (note.Order * yBuffer)), StartNotesPos.transform.position.z);

		// Add Sparkles
		GameObject sparklesObj = Instantiate(SparklesUIPrefab);
		sparklesObj.transform.SetParent(noteObj.transform);
		sparklesObj.transform.localPosition = new Vector3(0, 0, 0);
		note.SparklesParticle = sparklesObj.GetComponent<ParticleSystem>();

		// Add to list
		NotesList.Add(note);
		PlayNote(note);
	}

	public void ClearNotes()
	{
		NotesList.Clear();
		RecordManager._instance.EndPlayOrRecord();

		// Remove all Notes
		for (int i = 0; i < NotesContainer.transform.childCount; i++)
		{
			// Remove children
			Destroy(NotesContainer.transform.GetChild(i).gameObject);
		}
	}
	
	public void PlayNote(Note note)
	{
		MyAudioSource.PlayOneShot(note.SoundClip);

		// Play echo (but not when recording)
		if (FilterManager._instance.IsEcho && RecordManager._instance.MyPlayState != PlayState.Recording)
		{
			StartCoroutine(PlayEchos(note.SoundClip));
		}

		// TODO: play sparkles
		note.SparklesParticle.Emit(40);
	}

	public IEnumerator PlayEchos(AudioClip soundClip)
	{
		yield return new WaitForSeconds(0.250f);
		EchoAudioSource.PlayOneShot(soundClip);
		yield return new WaitForSeconds(0.250f);
		EchoAudioSource.PlayOneShot(soundClip);
	}

}
