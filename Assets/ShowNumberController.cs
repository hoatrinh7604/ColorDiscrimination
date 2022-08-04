using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowNumberController : MonoBehaviour
{
    [SerializeField] Image image;

    public void SetInfo(Color col)
    {
        image.color = col;
    }
}
