using UnityEngine;

public class OptionsManager : MonoBehaviour
{

    public static OptionsManager instance;

    #region xSensitivity
    [SerializeField] private InputSliderSync xSensitivityHolder;
    [SerializeField] private float _xSensitivity;
    private const string xSensitivityKey = "xSensitivity";
    public float xSensitivity => _xSensitivity;


    /// <summary>
    /// Set xSensitivity value
    /// </summary>
    /// <param name="xSensitivity"></param>
    public void SetXSensitivity(float xSensitivity)
    {
        xSensitivityHolder.SetValue(xSensitivity);
        _xSensitivity = xSensitivity;
    }

    /// <summary>
    /// Get xSensitivity from xSensitivityHolder
    /// </summary>
    public void FetchXSensitivity()
    {
        _xSensitivity = xSensitivityHolder.curVal;
    }

    /// <summary>
    /// Saves xSensitivity to loadable file
    /// </summary>
    public void SaveXSensitivity()
    {
        PlayerPrefs.SetFloat(xSensitivityKey, xSensitivity);
    }

    /// <summary>
    /// Loads xSensitivity
    /// </summary>
    public void LoadXSensitivity()
    {
        if (PlayerPrefs.HasKey(xSensitivityKey))
            SetXSensitivity(PlayerPrefs.GetFloat(xSensitivityKey));
    }

    #endregion xSensitivity

    #region ySensitivity
    [SerializeField] private InputSliderSync ySensitivityHolder;
    [SerializeField] private float _ySensitivity;
    private const string ySensitivityKey = "ySensitivity";
    public float ySensitivity => _ySensitivity;


    /// <summary>
    /// Set ySensitivity value
    /// </summary> 
    /// <param name="ySensitivity"></param>
    public void SetYSensitivity(float ySensitivity)
    {
        ySensitivityHolder.SetValue(ySensitivity);
        _ySensitivity = ySensitivity;
    }

    /// <summary>
    /// Get ySensitivity from ySensitivityHolder
    /// </summary>
    public void FetchYSensitivity()
    {
        _ySensitivity = ySensitivityHolder.curVal;
    }

    /// <summary>
    /// Saves ySensitivity to loadable file
    /// </summary>
    public void SaveYSensitivity()
    {
        PlayerPrefs.SetFloat(ySensitivityKey, ySensitivity);
    }

    /// <summary>
    /// Loads ySensitivity
    /// </summary>
    public void LoadYSensitivity()
    {
        if (PlayerPrefs.HasKey(ySensitivityKey))
            SetYSensitivity(PlayerPrefs.GetFloat(ySensitivityKey));
    }

    #endregion ySensitivity

    #region masterVolume
    [SerializeField] private InputSliderSync masterVolumeHolder;
    [SerializeField] private float _masterVolume;
    private const string masterVolumeKey = "masterVolume";
    public float masterVolume => _masterVolume;


    /// <summary>
    /// Set masterVolume value
    /// </summary> 
    /// <param name="masterVolume"></param>
    public void SetMasterVolume(float masterVolume)
    {
        masterVolumeHolder.SetValue(masterVolume);
        _masterVolume = masterVolume;
    }

    /// <summary>
    /// Get masterVolume from masterVolumeHolder
    /// </summary>
    public void FetchMasterVolume()
    {
        _masterVolume = masterVolumeHolder.curVal;
    }

    /// <summary>
    /// Saves masterVolume to loadable file
    /// </summary>
    public void SaveMasterVolume()
    {
        PlayerPrefs.SetFloat(masterVolumeKey, masterVolume);
    }

    /// <summary>
    /// Loads masterVolume
    /// </summary>
    public void LoadMasterVolume()
    {
        if (PlayerPrefs.HasKey(masterVolumeKey))
            SetMasterVolume(PlayerPrefs.GetFloat(masterVolumeKey));
    }

    #endregion masterVolume

    #region musicVolume
    [SerializeField] private InputSliderSync musicVolumeHolder;
    [SerializeField] private float _musicVolume;
    private const string musicVolumeKey = "musicVolume";
    public float musicVolume => _musicVolume;


    /// <summary>
    /// Set musicVolume value
    /// </summary> 
    /// <param name="musicVolume"></param>
    public void SetMusicVolume(float musicVolume)
    {
        musicVolumeHolder.SetValue(musicVolume);
        _musicVolume = musicVolume;
    }

    /// <summary>
    /// Get musicVolume from musicVolumeHolder
    /// </summary>
    public void FetchMusicVolume()
    {
        _musicVolume = musicVolumeHolder.curVal;
    }

    /// <summary>
    /// Saves musicVolume to loadable file
    /// </summary>
    public void SaveMusicVolume()
    {
        PlayerPrefs.SetFloat(musicVolumeKey, musicVolume);
    }

    /// <summary>
    /// Loads musicVolume
    /// </summary>
    public void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey))
            SetMusicVolume(PlayerPrefs.GetFloat(musicVolumeKey));
    }

    #endregion musicVolume 

    #region sfxVolume
    [SerializeField] private InputSliderSync sfxVolumeHolder;
    [SerializeField] private float _sfxVolume;
    private const string sfxVolumeKey = "sfxVolume";
    public float sfxVolume => _sfxVolume;


    /// <summary>
    /// Set sfxVolume value
    /// </summary> 
    /// <param name="sfxVolume"></param>
    public void SetSfxVolume(float sfxVolume)
    {
        sfxVolumeHolder.SetValue(sfxVolume);
        _sfxVolume = sfxVolume;
    }

    /// <summary>
    /// Get sfxVolume from sfxVolumeHolder
    /// </summary>
    public void FetchSfxVolume()
    {
        _sfxVolume = sfxVolumeHolder.curVal;
    }

    /// <summary>
    /// Saves sfxVolume to loadable file
    /// </summary>
    public void SaveSfxVolume()
    {
        PlayerPrefs.SetFloat(sfxVolumeKey, sfxVolume);
    }

    /// <summary>
    /// Loads sfxVolume
    /// </summary>
    public void LoadSfxVolume()
    {
        if (PlayerPrefs.HasKey(sfxVolumeKey))
            SetSfxVolume(PlayerPrefs.GetFloat(sfxVolumeKey));
    }

    #endregion sfxVolume



    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two OptionsManagers detected deleting second");
            Destroy(this);
        }

        // xSensitivityHolder.onValueChanged.AddListener(FetchXSensitivity);
        // ySensitivityHolder.onValueChanged.AddListener(FetchYSensitivity); 
        xSensitivityHolder.onValueChanged.AddListener((float _) => { FetchXSensitivity(); SaveXSensitivity(); });
        ySensitivityHolder.onValueChanged.AddListener((float _) => { FetchYSensitivity(); SaveYSensitivity(); });
        masterVolumeHolder.onValueChanged.AddListener((float _) => { FetchMasterVolume(); SaveMasterVolume(); });
        musicVolumeHolder.onValueChanged.AddListener((float _) => { FetchMusicVolume(); SaveMusicVolume(); });
        sfxVolumeHolder.onValueChanged.AddListener((float _) => { FetchSfxVolume(); SaveSfxVolume(); });

        LoadAll();
    }
    private void LoadAll()
    {
        LoadXSensitivity();
        LoadYSensitivity();
        LoadMasterVolume();
        LoadMusicVolume();
        LoadSfxVolume();
    }
}
