using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Note
{
	public int Order;
	public float NoteTime;
	public AudioClip SoundClip;
	public ParticleSystem SparklesParticle;
	public Color Color;

	public Note() { }

	public Note(Note copy)
	{
		this.Order = copy.Order;
		this.NoteTime = copy.NoteTime;
		this.SoundClip = copy.SoundClip;
		this.SparklesParticle = copy.SparklesParticle;
		this.Color = copy.Color;
	}
}