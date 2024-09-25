using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFollower : MonoBehaviour
{
    //Fields
    public GameObject CharacterMirror;
    public float CharacterYpos;
    //Value which determines the disance between the Character and the camera
    public float followOffSet;

    //No requirement for Start function
    void Update()
    {
        float characterXpos = CharacterMirror.transform.position.x;
        float cameraXpos = this.transform.position.x;

        // Calculate the new camera position to follow the character
        float newCameraXpos = characterXpos;

        if (characterXpos > cameraXpos + followOffSet)
        {
            newCameraXpos = characterXpos - followOffSet;
        }
        else if (characterXpos < cameraXpos - followOffSet)
        {
            newCameraXpos = characterXpos + followOffSet;
        }

        //Update the camera's position to follow the character's movement
        this.transform.position = new Vector3(newCameraXpos, CharacterYpos, transform.position.z);
    }
}
