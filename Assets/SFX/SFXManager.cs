using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundEffects", menuName = "SFXManager",order =1)]
public class SFXManager : ScriptableObject
{
    public AudioMixer mixer;
    public AudioMixerGroup EnvironmentalSounds, Music, PlayerSounds, EnemySounds;
    public AudioClip EnemyAlert, spring, metalSlam, sillyImpact, spit,schlorp,thump,uff,ceramic1,ceramic2,ceramic3;
}
