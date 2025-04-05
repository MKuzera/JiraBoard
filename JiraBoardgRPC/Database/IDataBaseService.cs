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
        UserModel GetUser(string email);
        UserModel GetUser(int id);
        int AddUser(string email, string password, string firstName, string lastName);
    }
}
