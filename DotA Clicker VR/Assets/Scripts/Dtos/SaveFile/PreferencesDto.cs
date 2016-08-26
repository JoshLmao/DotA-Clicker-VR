using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PreferencesDto
{
    public double MasterVolume { get; set; }
    public double AmbientVolume { get; set; }
    public double HeroVolume { get; set; }
    public bool MusicEnabled { get; set; }
    public bool AllAudioEnabled { get; set; }
    public double SuperSampleScale { get; set; }
}
