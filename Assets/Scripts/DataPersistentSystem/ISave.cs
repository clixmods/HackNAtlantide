/// <summary>
/// Interface to give method asked to save and load. 
/// </summary>
public interface ISave
{
  //  public bool IsDynamic { get; }
    /// <summary>
    /// Method to implement the behavior when the object has a load
    /// </summary>
    /// <param name="data">Contains Json data, you can use <see cref="JsonUtility.FromJson()"/>; to interpret it</param>
    public void OnLoad(string data);
    /// <summary>
    /// Method to implement the behavior when the object is saved
    /// </summary>
    /// <param name="saveData">This class will be saved, create a child class if you want have custom data</param>
    public void OnSave(out SaveData saveData);
}




