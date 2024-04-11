namespace LiquidSnake.Utils
{
    /// <summary>
    /// Interface implemented by all components and objects that can be reset upon
    /// being notified of a level reset event. This is delegated to the individual 
    /// objects to allow for custom reset protocols that vary between levels and scenes.
    /// </summary>
    public interface IResetteable
    {
        /// <summary>
        /// Restore this object's managed state to its original configuration.
        /// </summary>
        void Reset();
    }
}
