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

    private string _webpageUrl;
    private string _email;
    private string _emailSubject = "Mensaje de saludo";
    private string _emailBody = "Saludos profesor";

    public void SetInfoTeacherCards(Profesor[] infoProfesores)
    {
        GameObject[] cardsInCanva = GameObject.FindGameObjectsWithTag("teacherCard");
        
        for(int i=0; i<cardsInCanva.Length; i++){
            teacherCardHandler handlerCard = cardsInCanva[i].GetComponent<teacherCardHandler>();
            handlerCard.SetInfoCard(infoProfesores[i]);
        }
    }

    private void SetInfoCard(Profesor infoProfesor)
    {
        _nombresTxt.text = infoProfesor.nombres;
        _apellidosTxt.text = infoProfesor.apellidos;
        _cargoTxt.text = infoProfesor.cargo;

        Texture2D imageSearch = SearchImageInAssets("teachers", infoProfesor.profileImg);
        if(imageSearch is not null){
            Sprite spriteProfileImage = Sprite.Create(imageSearch, new Rect(0, 0, imageSearch.width, imageSearch.height), new Vector2(0.5f,0.5f));
            profileImage.sprite = spriteProfileImage;
        }
        _email = infoProfesor.email;
        _webpageUrl = infoProfesor.urlProfile;
    }

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
