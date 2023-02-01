using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static SimpleCloudRecoEventHandler;
using static GroupInfoHandlerCloud;

#if UNITY_EDITOR
    using UnityEditor;
#endif

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

        /*
        Object[] imageSearchRslts = SearchProfileImageInAssets(infoProfesor.profileImg);
        Object imageSearchRslt = imageSearchRslts[0];
        if(imageSearchRslt is not null){
            Texture2D tex = imageSearchRslt as Texture2D;
            Sprite spriteProfileImage = Sprite.Create(imageSearchRslt as Texture2D, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f));
            profileImage.sprite = spriteProfileImage;
        }
        */
        //infoProfesor.email;
        //infoProfesor.urlProfile;
    }
    /*

    public static Object[] SearchProfileImageInAssets(string imageRefName){
        string[] results = AssetDatabase.FindAssets(imageRefName, new[] {"Assets/Arte/Profesor/Images"});
        Object[] imagenObj = new Object[1];

        foreach (string result in results)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(result));
            string imagePath = AssetDatabase.GUIDToAssetPath(result);
            imagenObj[0] = AssetDatabase.LoadAssetAtPath(imagePath, typeof(Object));
        }
        return imagenObj;
    }
    */

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
