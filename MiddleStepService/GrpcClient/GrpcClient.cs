using Grpc.Core;
using Grpc.Net.Client;
using JiraBoardgRPC;
using MiddleStepService.Models;

namespace MiddleStepService.GrpcClient
{
    public class GrpcClient
    {
        public static GrpcClient Instance => _instance.Value;

        private static readonly Lazy<GrpcClient> _instance = new Lazy<GrpcClient>(() => new GrpcClient());

        private readonly JiraBoard.JiraBoardClient _client;

        private readonly GrpcChannel _channel;

        private const string GrpcServerUrlAdress = "http://localhost:5107";

        private GrpcClient()
        {
            _channel = GrpcChannel.ForAddress(GrpcServerUrlAdress);
            _client = new JiraBoard.JiraBoardClient(_channel);
        }

        public async Task<string> CreateTaskAsync(NewJiraTask newJiraTask)
        {
            var response = await _client.CreateTaskAsync(newJiraTask);
            string taskID = response.Message;
            return taskID;
        }

        public async Task<DeserializedJiraTask> GetTaskAsync(int taskID)
        {
            var response = await _client.GetTaskAsync(new JiraBoardgRPC.TaskID { TaskId = taskID });
            DeserializedJiraTask deserializedJiraTask = Deserializer.DeserializeJiraTask(response);
            return deserializedJiraTask;
        }

        public async Task<List<DeserializedJiraTask>> GetAllTaskAsync()
        {
            var request = new JiraBoardgRPC.EmptyRequest();

            List<DeserializedJiraTask> deserializedJiraTasks = new List<DeserializedJiraTask>();

            using (var call = _client.GetAllTasks(new JiraBoardgRPC.EmptyRequest()))
            {
                while (await call.ResponseStream.MoveNext())
                {
                    var currentTask = call.ResponseStream.Current;
                    deserializedJiraTasks.Add(Deserializer.DeserializeJiraTask(currentTask));
                }
            }
            return deserializedJiraTasks;
        }

        public async Task<string> UpdateTaskAsync(DeserializedJiraTask deserializedJiraTask)
        {
            JiraTask jiraTask = GRPCSerializer.SerializeJiraTask(deserializedJiraTask);
            var response = await _client.UpdateTaskAsync(jiraTask);
            return response.Message;
        }

        public async Task<string> DeleteTaskAsync(int taskID)
        {
            var response = await _client.DeleteTaskAsync(new JiraBoardgRPC.TaskID { TaskId = taskID });
            return response.Message;
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
