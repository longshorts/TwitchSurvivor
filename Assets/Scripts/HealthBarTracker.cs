using UnityEngine;
using System.Collections;

public class HealthBarTracker : MonoBehaviour {

    public Character TrackedCharacter;

    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (!TrackedCharacter)
        {
            Debug.LogWarning("Need a tracked character for healthbar: " + name);
            return;
        }
        Vector3 charPos = mainCamera.WorldToScreenPoint(TrackedCharacter.transform.position);
        charPos += (TrackedCharacter.GetComponent<SpriteRenderer>().sprite.rect.height / 2 + 4) * Vector3.up;
        transform.position = charPos;
	}
}
