using UnityEngine;
using System.Collections;
using Deform;
public class MeshDeformerInput : MonoBehaviour
{

    public float force = 10f, finalForce, returnSpringForce;
    public float forceOffset = 0.1f;
    private MeshDeformer deformer;
    private Vector3 pointOfContact;

    public LatticeDeformer latice;

    private void Awake()
    {
        latice.GetControlPoint(0, 0, 0);
    }
    // void Update () {
    // 	if (Input.GetMouseButton(0)) {
    // 	HandleInput();
    // 	}
    // }

    // void HandleInput () {
    // 	Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
    // 	RaycastHit hit;

    // 	if (Physics.Raycast(inputRay, out hit)) {
    // 		MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
    // 		if (deformer) {
    // 			Vector3 point = hit.point;
    // 			point += hit.normal * forceOffset;
    // 			deformer.AddDeformingForce(point, force);
    // 		}
    // 	}
    // }

    private void OnCollisionEnter(Collision collision)
    {
        deformer = collision.gameObject.GetComponent<MeshDeformer>();
        if (deformer)
        {
            ContactPoint hit = collision.contacts[0];
            pointOfContact = hit.point;
            pointOfContact += hit.normal * forceOffset;
            StartCoroutine(Deform(deformer, pointOfContact));
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!deformer)
        {
            print("Deformer is null");
            return;
        }
        if (other.gameObject.tag == "Finish")
        {
            deformer.springForce = returnSpringForce;
            force = finalForce;
            deformer = null;
        }
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Deform(deformer, pointOfContact));
        }
        if (other.gameObject.tag == "Respawn")
        {
            force = 30; StartCoroutine(Deform(deformer, pointOfContact));
        }
    }


    private IEnumerator Deform(MeshDeformer _deformer, Vector3 _point)
    {
        for (int i = 0; i < force; i++)
        {
            _deformer.AddDeformingForce(_point, i);
            yield return new WaitForSeconds(0.01f);
        }
    }
}