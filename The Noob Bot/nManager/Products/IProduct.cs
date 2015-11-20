namespace nManager.Products
{
    /// <summary>
    /// Product interface
    /// </summary>
    public interface IProduct
    {
        #region Methods

        /// <summary>
        /// Initialize Product.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Dispose Product
        /// </summary>
        void Dispose();

        /// <summary>
        /// Start Product.
        /// </summary>
        void Start();

        /// <summary>
        /// RemoteStart Product.
        /// </summary>
        void RemoteStart(string[] args);

        /// <summary>
        /// Stop Product.
        /// </summary>
        void Stop();

        /// <summary>
        /// Settings Product.
        /// </summary>
        void Settings();

        /// <summary>
        /// Gets if Product is started.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if product is started, else <c>false</c>.
        /// </value>
        bool IsStarted { get; }

        #endregion Methods
    }
}