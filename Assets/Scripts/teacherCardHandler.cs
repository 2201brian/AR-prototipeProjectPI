using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class teacherCardHandler : MonoBehaviour
{
        [Header("Referencias")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text _nombresTxt;
    [SerializeField] private TMP_Text _apellidosTxt;
    [SerializeField] private TMP_Text _cargoTxt;

    private string webpageUrl;
    private string email;
    private string emailSubject = "Mensaje de saludo";
    private string emailBody = "Saludos profesor";

    public void SetInfoTeacherCard()
    {

    }


    public void Execute(string type){
        switch(type)
        {
            case "webpage":
                Application.OpenURL(webpageUrl);
                break;
            case "email":
                Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", email, emailSubject, emailBody));
                break;
        }
        
    }


}
