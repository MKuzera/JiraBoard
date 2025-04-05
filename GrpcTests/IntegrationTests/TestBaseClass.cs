using System.Diagnostics;
using Xunit;

public class TestBaseClass : IAsyncLifetime
{
    protected Process? _gRPCService = null;
    protected Process? _MiddleStepService = null;
    private static readonly object _lockObject = new object();

    public async Task InitializeAsync()
    {
        lock (_lockObject)
        {
            if (_gRPCService is null)
            {
                _gRPCService = StartProcess("JiraBoardgRPC\\JiraBoardgRPC.csproj");
            }

            if (_MiddleStepService is null)
            {
                _MiddleStepService = StartProcess("MiddleStepService\\MiddleStepService.csproj");
            }
        }

        await Task.Delay(10000); 
    }

    public Task DisposeAsync()
    {
        _gRPCService?.Kill();
        _gRPCService?.Dispose();
        _MiddleStepService?.Kill();
        _MiddleStepService?.Dispose();
        return Task.CompletedTask;
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
}
