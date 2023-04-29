/// <summary>
///  <inheritdoc/>
/// ISaveInstance exist for instancied component on a scene
/// </summary>
public interface ISaveMonoBehavior : ISave
{
    /// <summary>
    /// Unique ID, required because it distinct different saved instance from the same MonoBehavior
    /// </summary>
    public int SaveID { get; }
}


