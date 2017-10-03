namespace WebApp.Services.Tasks
{
    /// <summary>
    /// Giao diện mà cần được thực hiện bởi mỗi công việc
    /// </summary>
    public partial interface ITask
    {
        /// <summary>
        /// Execute task
        /// </summary>
        void Execute();
    }
}
