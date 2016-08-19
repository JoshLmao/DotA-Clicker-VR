using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {

    float scrollSpeed = -0.25f;
    Vector2 scrollVector = new Vector2(0, 1);
    Material[] mats;
    Material Wings;
    Material Tail;

    void Start()
    {
        SkinnedMeshRenderer[] rend = transform.Find("phoenix_bird_reference").GetComponents<SkinnedMeshRenderer>();
        mats = rend[0].sharedMaterials;

        foreach (Material mat in mats)
        {
            if (mat.name == "phoenix_tailfx_color_psd_e1ffa9a8_mip0")
            {
                Tail = mat;
            }
            else if (mat.name == "phoenix_fx_color_psd_b5d2d605_mip0")
            {
                Wings = mat;
            }
        }
    }
	
	void Update ()
    {
        Wings.SetTextureOffset("_MainTex", scrollVector * Time.time * scrollSpeed);
        Tail.SetTextureOffset("_MainTex", scrollVector * Time.time * scrollSpeed);
    }

    void OnApllicationQuit()
    {
        Wings.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
        Tail.SetTextureOffset("_MainTex", new Vector2(0f, 0f));
    }
}
