using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for all creeps. Should be generic unless playing specific animations
/// </summary>
public class CreepController : MonoBehaviour
{
    bool canSink = false;
    float sinkSpeed = 0.025f;
    Animator m_animator;

	void Start ()
    {
        m_animator = this.GetComponent<Animator>();
        float randomStart = Random.Range(0f, 1f);
        StartCoroutine(StartOffset(randomStart));
	}

    IEnumerator StartOffset(float time)
    {
        yield return new WaitForSeconds(time);

        m_animator.SetTrigger("startAnim");
    }

    void Update ()
    {
	    if(canSink)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -10, transform.position.z), sinkSpeed * Time.deltaTime);
        }
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "ViveController")
        {
            StartCoroutine(SinkIntoFloor());
        }
    }

    IEnumerator SinkIntoFloor()
    {
        yield return new WaitForSeconds(5f);
        canSink = true;
    }
}
