using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonNumberController : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI textValue;
    [SerializeField] string value;
    [SerializeField] int index;

    private void Start()
    {
        button.onClick.AddListener(OnPressButton);
    }

    public void SetInfo(Color col)
    {
        button.image.color = col;
    }

    public void OnPressButton()
    {
        GamePlayController.Instance.OnPressHandle(index);
        //button.interactable = false;
    }
}
