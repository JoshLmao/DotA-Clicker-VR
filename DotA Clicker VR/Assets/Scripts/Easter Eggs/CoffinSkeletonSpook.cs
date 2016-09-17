using UnityEngine;
using System.Collections;

public class CoffinSkeletonSpook : MonoBehaviour {

    Animator m_coffinAnimator, m_skeletonAnimator;
    bool canScare = true;

    void Start()
    {
        m_coffinAnimator = transform.Find("Coffin_Animatronic").GetComponent<Animator>();
        m_skeletonAnimator = transform.Find("Skeleton_animatronic").GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.name == "Camera (eye)" && canScare)
        {
            Debug.Log("Getting spooked");
            int count = Random.Range(1, 3);
            m_coffinAnimator.SetTrigger("doAnim" + count);
            m_skeletonAnimator.SetTrigger("doAnim" + count);

            canScare = false;
            StartCoroutine(WaitForSeconds(15f));
        }
    }

    IEnumerator WaitForSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        canScare = true;
    }
}
