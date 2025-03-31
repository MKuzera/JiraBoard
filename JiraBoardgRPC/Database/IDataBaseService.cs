namespace JiraBoardgRPC.FakeDataBase
{
    public interface IDataBaseService
    {
        void AddTask(JiraTask task);
        bool DeleteTask(int id);
        bool UpdateTask(JiraTask updatedTask);
        JiraTask? GetTaskById(int id);
        IEnumerable<JiraTask> GetAllTasks();
        int GetLastId();
    }
}
