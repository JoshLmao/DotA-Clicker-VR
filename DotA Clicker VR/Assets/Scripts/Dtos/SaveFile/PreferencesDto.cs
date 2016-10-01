using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PreferencesDto
{
    public float MasterVolume { get; set; }
    public float AmbientVolume { get; set; }
    public float HeroVolume { get; set; }
    public bool MusicEnabled { get; set; }
    public bool AllAudioEnabled { get; set; }
    public float SuperSampleScale { get; set; }
}
