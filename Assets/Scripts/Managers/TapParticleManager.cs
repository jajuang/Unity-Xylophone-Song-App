using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapParticleManager : MonoBehaviour
{
	public bool ShowTapParticle = true;
	public ParticleSystem TapParticle;
	public ParticleSystem TapParticleInner;
	public float TapParticleDuration;
	private int TapParticleBurstCount;
	private int TapParticleInnerBurstCount;


	void Start()
	{
		ParticleSystem.EmissionModule TapParticleEmission = TapParticle.emission;
		TapParticleBurstCount = (int)TapParticleEmission.GetBurst(0).count.constant;
		var dur = TapParticle.main;
		TapParticleDuration = dur.duration;
		if (TapParticleInner != null)
		{
			ParticleSystem.EmissionModule TapParticleInnerEmission = TapParticleInner.emission;
			TapParticleInnerBurstCount = (int)TapParticleInnerEmission.GetBurst(0).count.constant;
		}
	}


	void Update()
	{
		if (ShowTapParticle && (Input.GetMouseButtonDown(0))) 
		{
			PlayTapParticle();
		}
	}


	public void PlayTapParticle()
	{
		// Move particle to player touch position
		TapParticle.transform.position = Input.mousePosition + new Vector3(0.1f, 0.1f, 0);
		TapParticle.Emit(TapParticleBurstCount);

		if (TapParticleInner != null)
		{
			TapParticleInner.Emit(TapParticleInnerBurstCount);
		}
	}
}
