using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "VolumetricPostProcessingMaterials", menuName = "Game/VolumetricPostProcessingMaterials")]
public class VolumetricPostProcessingMaterials : UnityEngine.ScriptableObject
{
    //---Your Materials---
    public Material customEffect;
    
    //---Accessing the data from the Pass---
    static VolumetricPostProcessingMaterials _instance;

    public static VolumetricPostProcessingMaterials Instance
    {
        get
        {
            if (_instance != null) return _instance;
            // TODO check if application is quitting
            // and avoid loading if that is the case

            _instance = UnityEngine.Resources.Load("VolumetricPostProcessingMaterials") as VolumetricPostProcessingMaterials;
            return _instance;
        }
    }
}