using UnityEngine;
using System.Collections;

public class SaveFileDto : MonoBehaviour
{
    public class SaveFile
    {
        public string PlayerName { get; set; }
        public RadiantSideDto RadiantSide { get; set; }
        //public DireSideDto DireSide { get; set; }
        public PreferencesDto Preferences { get; set; }
    }
}
