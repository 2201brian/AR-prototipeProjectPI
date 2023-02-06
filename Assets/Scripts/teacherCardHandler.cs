using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SimpleCloudRecoEventHandler;
using static GroupInfoHandlerCloud;

public class teacherCardHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image profileImage;
    [SerializeField] private TMP_Text _nombresTxt;
    [SerializeField] private TMP_Text _apellidosTxt;
    [SerializeField] private TMP_Text _cargoTxt;

    [Header("Info Docente")]
    [SerializeField] private Sprite _spriteProfileImage;
    [SerializeField] private string _webpageUrl;
    [SerializeField] private string _email;
    private string _emailSubject = "Mensaje de saludo";
    private string _emailBody = "Saludos profesor";
 
    //This function sets the teacher info to the card 
    public void SetInfoCard(Profesor infoProfesor)
    {
        _nombresTxt.text = infoProfesor.nombres;
        _apellidosTxt.text = infoProfesor.apellidos;
        _cargoTxt.text = infoProfesor.cargo;

        Texture2D imageSearch = SearchImageInAssets("teachers", infoProfesor.profileImg);
        if(imageSearch is not null){
            _spriteProfileImage = Sprite.Create(imageSearch, new Rect(0, 0, imageSearch.width, imageSearch.height), new Vector2(0.5f,0.5f));
            profileImage.sprite = _spriteProfileImage;
        }
        _email = infoProfesor.email;
        _webpageUrl = infoProfesor.urlProfile;
    }

    //this function executes de mail order or opens the profile url of the teacher, 
    //this function is attached to the OnClick events of the buttons in the teacherCard Prefab
    public void Execute(string type){
        switch(type)
        {
            case "webpage":
                Application.OpenURL(_webpageUrl);
                break;
            case "email":
                Application.OpenURL(string.Format("mailto:{0}?subject={1}&body={2}", _email, _emailSubject, _emailBody));
                break;
        }
        
    }


}
