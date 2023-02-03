using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public enum tipoFuncionalidad
    {
        Profesores,
        LineasInvestigacion,
        WebPage
    }

    [Header("Funcionalidad")]
    [SerializeField] private tipoFuncionalidad _typeF;
    
    private GameObject cloudRecognitionObj;
    private SimpleCloudRecoEventHandler screh;
    private bool enabledStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        enabledStatus = false;
        GameObject[] cloudRecognitionSrch = GameObject.FindGameObjectsWithTag("screh");
        if(cloudRecognitionSrch is not null)
        {
            cloudRecognitionObj = cloudRecognitionSrch[0];
            screh = cloudRecognitionObj.GetComponent<SimpleCloudRecoEventHandler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown() {
        switch (_typeF)
        {
            case tipoFuncionalidad.Profesores:
                if(enabledStatus)
                {
                    screh.DestroyTeacherCards();
                    enabledStatus = false;
                } else 
                {
                    screh.LoadTeacherCards();
                    enabledStatus = true;
                }
                break;

            case tipoFuncionalidad.LineasInvestigacion:
                screh.playLineasInvestigacion();
                break;

            case tipoFuncionalidad.WebPage:
                try
                {
                    Application.OpenURL(screh.webpageGroup);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                break;
        }
    }
}