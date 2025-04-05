using System.Diagnostics;

namespace GrpcTests.IntegrationTests.Helpers
{
    internal class ProcessStarter : IDisposable
    {
        private static readonly object _lock = new object();
        private static ProcessStarter? _instance;
        protected Process? _gRPCService = null;
        protected Process? _MiddleStepService = null;
        public static ProcessStarter Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new ProcessStarter();
                        }
                    }
                }
                return _instance;
            }
        }

        private ProcessStarter()
        {
            if (_gRPCService is null)
            {
                _gRPCService = StartProcess("JiraBoardgRPC\\JiraBoardgRPC.csproj");
            }

            if (_MiddleStepService is null)
            {
                _MiddleStepService = StartProcess("MiddleStepService\\MiddleStepService.csproj");
            }
            Task.Delay(10000).GetAwaiter().GetResult();
        }
        private Process StartProcess(string projectPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\"));
            var fullProjectPath = Path.Combine(parentDirectory, projectPath);

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project {fullProjectPath}",
                UseShellExecute = false,
                CreateNoWindow = true
            };

            return Process.Start(startInfo) ?? throw new Exception($"Failed to start {projectPath}");
        }

        public void Dispose()
        {
            _gRPCService?.Kill();
            _gRPCService?.Dispose();
            _MiddleStepService?.Kill();
            _MiddleStepService?.Dispose();
        }
    }
}
