using UnityEngine;
using System.Collections;

public class tagChildren : MonoBehaviour {

    [SerializeField]
    private string newTag = "Untagged";

	// Use this for initialization
	void Start () {
        foreach (Transform t in gameObject.GetComponentsInChildren<Transform>(true))
        {
            t.gameObject.tag = newTag;
        }
    }
}
