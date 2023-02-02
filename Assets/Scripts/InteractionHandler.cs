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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
