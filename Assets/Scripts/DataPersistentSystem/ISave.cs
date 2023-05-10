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

    /// <summary>
    /// Method to implement the behavior when the object is reset
    /// </summary>
    public void OnReset();
    /* EXAMPLE FOR SCRIPTABLE OBJECT
     *
     *public override void OnLoad(string data)
      {
         SlotsInventorySaveData slotsInventoryLoaded = JsonUtility.FromJson<SlotsInventorySaveData>(data);
         slotsInventory = new SlotInventory[slotsInventoryLoaded.slots.Length];
         for (int i = 0; i < slotsInventoryLoaded.slots.Length; i++)
         {
            slotsInventory[i] = new SlotInventory();
            slotsInventory[i].amount = slotsInventoryLoaded.slots[i].value;
            // Get File name of the scriptableObject used in item to get it from Resources Folder
            string resourcesFileName = slotsInventoryLoaded.slots[i].resourcesFileName;
            slotsInventory[i].item = DataPersistentUtility.GetAssetFromResources<ItemScriptableObject>(resourcesFileName);
         }
         EventObjectAdd?.Invoke();
      }
      public override void OnSave(out SaveData saveData)
      {
         SlotsInventorySaveData slotsToSave = new SlotsInventorySaveData();
         slotsToSave.slots = new SlotsInventorySaveData.slot[slotsInventory.Length] ;
         for (int i = 0; i < slotsInventory.Length; i++)
         {
            var item = slotsInventory[i].item;
            if(item != null)
               slotsToSave.slots[i].resourcesFileName = DataPersistentUtility.ExtractAssetName(item);
            
            slotsToSave.slots[i].value = slotsInventory[i].amount;
         }
         saveData = slotsToSave;
      }
     * 
     */
}




