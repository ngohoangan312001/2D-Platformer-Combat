using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera camera;
    public Transform followTarget;

    //start position of the paralax game object
    Vector2 startingPosition;

    //start Z value of the paralax game object
    float startingZ;

    // "=>" syntax mean update itseft every frame so it no need to be in update() method
    Vector2 camMoveSinceStart => (Vector2)camera.transform.position - startingPosition ;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (camera.transform.position.z + (zDistanceFromTarget > 0 ? camera.farClipPlane : camera.nearClipPlane));

    // the further the object from the player, the slower ParalaxEffect object will move
    float paralaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //when the target move, move the parallax object the same distance times a multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * paralaxFactor;

        //the X/Y position changes based on target travel speed times the parallax factor, z stay consistent
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
