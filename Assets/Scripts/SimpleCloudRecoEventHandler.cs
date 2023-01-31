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
    [SerializeField] private Profesor[] _profesores;
    [SerializeField] private string[] _lineasInvestigacion;
    [Space]
    [SerializeField] private Transform _contentParent;
    [SerializeField] private MetaDataSO _metadataSO;

    [Header("UI")]
    [SerializeField] private UnityEngine.UI.Image _scannigImage;
    [SerializeField] private Color _TrueColor;
    [SerializeField] private Color _FalseColor;
    [Space]
    [SerializeField] private Button _resetBtn;

    private GameObject _contentArdilla;
    private GameObject _contentSalon;
    private GameObject _contentProfesores;
    GroupInfoHandlerCloud handlerInfoSalon;

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
        public Profesor[] profesores;
        public string[] lineasInvestigacion;
    }

    private GrupoInvestigacion infoGrupo;

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

        Debug.LogFormat("<color=green>METADATA:</color> {0}",mTargetMetadata);
        ParseMetaDataJSON(mTargetMetadata);
        ChangeContent(true);

         mCloudRecoBehaviour.EnableObservers(cloudRecoSearchResult, ImageTargetTemplate.gameObject);
    }
    
    private void ParseMetaDataJSON(string metadataJSON){
        infoGrupo = JsonUtility.FromJson<GrupoInvestigacion>(metadataJSON);
        _idGroup = int.Parse(infoGrupo.idGrupo);
        _groupName = infoGrupo.groupName;
        _imageLogoGroup = infoGrupo.imageLogoGroup;
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
           _contentArdilla = Instantiate(_metadataSO.ardilla, _contentParent);
           _contentSalon = Instantiate(_metadataSO.salon, _contentParent);
           _contentProfesores = Instantiate(_metadataSO.profesores, _contentParent);
           handlerInfoSalon = _contentSalon.GetComponent<GroupInfoHandlerCloud>();
           handlerInfoSalon.SetInfoGroup(_groupName,_imageLogoGroup);
           //falta inicalizar clase para el llamado
           //SetInfoGroup(_groupName,_imageLogoGroup):
        } 
        else 
        {
            Destroy(_contentArdilla);
            Destroy(_contentSalon);
            Destroy(_contentProfesores);
        }
    }


    /*
    Traido de GroupInfoHandler
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
    */


    /*
    void OnGUI() {
      // Display current 'scanning' status
      GUI.Box (new Rect(100,100,200,50), mIsScanning ? "Scanning" : "Not scanning");
      // Display metadata of latest detected cloud-target
      GUI.Box (new Rect(100,200,200,50), "Metadata: " + mTargetMetadata);
      // If not scanning, show button
      // so that user can restart cloud scanning
      if (!mIsScanning) {
          if (GUI.Button(new Rect(100,300,200,50), "Restart Scanning")) {
          // Reset Behaviour
          mCloudRecoBehaviour.enabled = true;
          mTargetMetadata="";
          }
      }
    }*/
}