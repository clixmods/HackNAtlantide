namespace UnityEngine.InputSystem
{
    /// <summary>
    /// References a specific <see cref="InputAction"/> in an <see cref="InputActionMap"/>
    /// stored inside an <see cref="InputActionAsset"/>.
    /// </summary>
    /// <remarks>
    /// The difference to a plain reference directly to an <see cref="InputAction"/> object is
    /// that an InputActionReference can be serialized without causing the referenced <see cref="InputAction"/>
    /// to be serialized as well. The reference will remain intact even if the action or the map
    /// that contains the action is renamed.
    ///
    /// References can be set up graphically in the editor by dropping individual actions from the project
    /// browser onto a reference field.
    /// </remarks>
    /// <seealso cref="InputActionProperty"/>
    /// <seealso cref="InputAction"/>
    /// <seealso cref="InputActionAsset"/>
    public class InputActionReferenceEnhanced : InputActionReference
    {
        public Sprite Image;
    }
}