using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using UnityEditor;


public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    CloudRecoBehaviour mCloudRecoBehaviour;
    bool mIsScanning = false;
    string mTargetMetadata = "";

    
    [Header ("Vuforia")]
    public ImageTargetBehaviour ImageTargetTemplate;

    [Header("Metadata")]
    [SerializeField] private int _idGroup;
    [SerializeField] private string _groupName;
    [SerializeField] private string _imageLogoGroup;
    [SerializeField] public string webpageGroup;
    [SerializeField] private Profesor[] _profesores;
    [Space]
    [Header("Content parent")]
    [SerializeField] private Transform _contentParent;
    [Header("Prefabs")]
    [SerializeField] private MetaDataSO _metadataSO;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image _scannigImage;
    [SerializeField] private Color _TrueColor;
    [SerializeField] private Color _FalseColor;
    [Space]
    [SerializeField] private Button _resetBtn;

    //PREFABS TO USE
    private GameObject _contentArdilla;
    private GameObject _contentSalon;
    private GameObject _contentCanvaTeachers;
    private GameObject[] _contentTeacherCard;

    //HANDLERS COMPONENTS OF THE PREFABS
    GroupInfoHandlerCloud handlerInfoSalon;
    teacherCardHandler handlerTeacherCard;

    [System.Serializable]
    public struct Profesor
    {
        public string nombres;
        public string apellidos;
        public string cargo;
        public string email;
        public string urlProfile;
        public string profileImg;
    }

    [System.Serializable]
    public struct GrupoInvestigacion
    {
        public string idGrupo;
        public string groupName;
        public string imageLogoGroup;
        public string webpageGroup;
        public Profesor[] profesores;
    }

    private GrupoInvestigacion infoGrupo;
    private bool isFirstRecognitionMade;
    private GameObject ardillaObj;
    private ardillaAnimationBehaviour ardillaBehaviour;

    // Register cloud reco callbacks
    void Awake()
    {
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    private void Start() {
        _scannigImage.color = _FalseColor;
        _resetBtn.gameObject.SetActive(false);
    }

    public void OnInitialized(CloudRecoBehaviour cloudRecoBehaviour)
    {
        Debug.Log("Cloud Reco initialized");
    }

    public void OnInitError(CloudRecoBehaviour.InitError initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnUpdateError(CloudRecoBehaviour.QueryError updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());

    }

    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;

        _scannigImage.color = mIsScanning ? _TrueColor : _FalseColor;

        if (scanning)
        {
            // Clear all known targets
        } else {
            _resetBtn.gameObject.SetActive(true);
        }
    }
     // Here we handle a cloud target recognition event
    public void OnNewSearchResult(CloudRecoBehaviour.CloudRecoSearchResult cloudRecoSearchResult )
    {
        // Store the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;

        // Stop the scanning by disabling the behaviour
        mCloudRecoBehaviour.enabled = false;

        //Debug.LogFormat("<color=green>METADATA:</color> {0}",mTargetMetadata);
        ParseMetaDataJSON(mTargetMetadata);
        ChangeContent(true);
        //Calls to the object in scene and its handlers to play the anims and audios
        SetArdillaHandlers();

        mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
    }
    
    //This function parse the information received from the metadata file of a target
    private void ParseMetaDataJSON(string metadataJSON){
        infoGrupo = JsonUtility.FromJson<GrupoInvestigacion>(metadataJSON);
        _idGroup = int.Parse(infoGrupo.idGrupo);
        _groupName = infoGrupo.groupName;
        _imageLogoGroup = infoGrupo.imageLogoGroup;
        webpageGroup = infoGrupo.webpageGroup;
        _profesores = infoGrupo.profesores;
    }

    //This function enables the image tracking behaviour of vuforia and destroy the content loaded
    public void ResetButton(){
        // Reseting metadata str
        mTargetMetadata = "";
        // Enabling the scanning
        mCloudRecoBehaviour.enabled = true;

        _scannigImage.color = _TrueColor;
        _resetBtn.gameObject.SetActive(false);
        ChangeContent(false);
    }

    //This function is responsible of managing the load or delete of the visual content (3D caracter, the classroom prefab)
    private void ChangeContent(bool show){
        if(show)
        {
            //LOAD PREFABS
           _contentArdilla = Instantiate(_metadataSO.ardilla, _contentParent);
           _contentSalon = Instantiate(_metadataSO.salon, _contentParent);
           _contentCanvaTeachers = Instantiate(_metadataSO.profesores, _contentParent);

           //LOAD CONTENT
           //load salon info 
           handlerInfoSalon = _contentSalon.GetComponent<GroupInfoHandlerCloud>();
           handlerInfoSalon.SetInfoGroup(_groupName,_imageLogoGroup);           
        } 
        else 
        {
            Destroy(_contentArdilla);
            Destroy(_contentSalon);
            Destroy(_contentCanvaTeachers);
        }
    }

    //Call to play lineas de investigacion
    public void playLineasInvestigacion()
    {
        ardillaBehaviour.tellSpeech("lineas",_idGroup);
    }

    //ANIM AND AUDIO FOR FIRST RECOGNITION - BIENVENIDA ESCUELA Y PRESENTACION INTERFAZ
    private void FirstRecognitionEvents()
    {
        isFirstRecognitionMade = true;
        //ardillaBehaviour.
        //call to function of the ardilla object to greet and present UI
        ardillaBehaviour.doGreetAnim();
    }

    //Calls to the object in scene and its handlers to play the anims and audios
    private void SetArdillaHandlers()
    {
        GameObject[] ardillaObjSrch = GameObject.FindGameObjectsWithTag("ardillaObject");
        if(ardillaObjSrch.Length != 0)
        {
            ardillaObj = ardillaObjSrch[0];
            ardillaBehaviour = ardillaObj.GetComponent<ardillaAnimationBehaviour>();
            ardillaBehaviour.setBehaviour(_idGroup);
            //Consider events if it is the first recognition
            if(!isFirstRecognitionMade)
            {
                FirstRecognitionEvents();
            } else
            {
                ardillaBehaviour.tellSpeech("bienvenida",_idGroup);
            }
        }
    }

    //This function creates the instances of the TEACHER CARDS in the canvas and pass the info of each teacher to the respective card
    public void LoadTeacherCards()
    {
        _contentTeacherCard = new GameObject[_profesores.Length];//warning
        RectTransform canvasRectTransform = _contentCanvaTeachers.GetComponent<RectTransform>();
        VerticalLayoutGroup layoutCanvaTeachers = _contentCanvaTeachers.GetComponent<VerticalLayoutGroup>();

        for(int i=0; i<_contentTeacherCard.Length; i++)
        {
            _contentTeacherCard[i] =  Instantiate(_metadataSO.teacherCard, canvasRectTransform) as GameObject;
            _contentTeacherCard[i].transform.SetParent(layoutCanvaTeachers.transform,false);
            teacherCardHandler cardHandler = _contentTeacherCard[i].GetComponent<teacherCardHandler>();
            cardHandler.SetInfoCard(_profesores[i]);
        }

        //handlerTeacherCard =  new teacherCardHandler();
        //handlerTeacherCard.SetInfoTeacherCards(_profesores);
    }

    //Destroy the teacher cards that have been loaded to the canvas
    public void DestroyTeacherCards()
    {
        foreach (GameObject card in _contentTeacherCard)
        {
            Destroy(card);
        }
    }
}