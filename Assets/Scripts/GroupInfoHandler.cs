using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GroupInfoHandler : MonoBehaviour
{
    [Header("Información Grupo")]
    [SerializeField] private GroupInfo groupInfo;

    [Header("Referencias")]
    [SerializeField] private TMP_Text nombreTxtField;
    [SerializeField] private SpriteRenderer logoImage;

    private void Start() {
        logoImage.sprite = groupInfo.logoGI;
        nombreTxtField.text = groupInfo.nameGI;
    }

    public void Execute(){
        Application.OpenURL(groupInfo.webpageGI);
    }
}
