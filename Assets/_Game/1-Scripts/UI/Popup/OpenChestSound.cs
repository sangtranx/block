using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChestSound : MonoBehaviour
{
    public void PlaySound()
    {
        AudioController.Instance.Play(AudioName.Sound_Open_Chest);
    }
}
