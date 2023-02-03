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
    [SerializeField] private string[] _lineasInvestigacion;
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
        public string[] lineasInvestigacion;
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
    
    private void ParseMetaDataJSON(string metadataJSON){
        infoGrupo = JsonUtility.FromJson<GrupoInvestigacion>(metadataJSON);
        _idGroup = int.Parse(infoGrupo.idGrupo);
        _groupName = infoGrupo.groupName;
        _imageLogoGroup = infoGrupo.imageLogoGroup;
        webpageGroup = infoGrupo.webpageGroup;
        _profesores = infoGrupo.profesores;
        _lineasInvestigacion = infoGrupo.lineasInvestigacion;
    }

    public void ResetButton(){
        // Reseting metadata str
        mTargetMetadata = "";
        // Enabling the scanning
        mCloudRecoBehaviour.enabled = true;

        _scannigImage.color = _TrueColor;
        _resetBtn.gameObject.SetActive(false);
        ChangeContent(false);
    }

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

    //ANIM AND AUDIO FOR FIRST RECOGNITION - BIENVENIDA ESCUELA Y PRESENTACION INTERFAZ
    void FirstRecognitionEvents()
    {
        isFirstRecognitionMade = true;
        //ardillaBehaviour.
        //call  function of the ardillaObject to greet and presentation iu
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
            ardillaBehaviour.setBehaviour();
            //Consider events if it is the first recognition
            if(!isFirstRecognitionMade)
            {
                FirstRecognitionEvents();
            }
        }
    }

    //TEACHER CARDS AND THEIR CANVAS
    public void LoadTeacherCards()
    {
        _contentTeacherCard = new GameObject[_profesores.Length];
        RectTransform canvasRectTransform = _contentCanvaTeachers.GetComponent<RectTransform>();
        VerticalLayoutGroup layoutCanvaTeachers = _contentCanvaTeachers.GetComponent<VerticalLayoutGroup>();

        for(int i=0; i<_contentTeacherCard.Length; i++)
        {
            _contentTeacherCard[i] =  Instantiate(_metadataSO.teacherCard, canvasRectTransform) as GameObject;
            _contentTeacherCard[i].transform.SetParent(layoutCanvaTeachers.transform,false);
        }

        handlerTeacherCard =  new teacherCardHandler();
        handlerTeacherCard.SetInfoTeacherCards(_profesores);
    }

    public void DestroyTeacherCards()
    {
        foreach (GameObject card in _contentTeacherCard)
        {
            Destroy(card);
        }
    }
}