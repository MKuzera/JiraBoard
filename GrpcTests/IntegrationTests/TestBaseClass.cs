using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xunit;

public class TestBaseClass : IAsyncLifetime, IDisposable
{
    protected Process? _gRPCService = null;

    protected Process? _MiddleStepService = null;

    public async Task InitializeAsync()
    {
        if (_gRPCService is null)
        {
            _gRPCService = StartProcess("JiraBoardgRPC\\JiraBoardgRPC.csproj");
            await Task.Delay(5000); 
        }

        if (_MiddleStepService is null)
        {
            _MiddleStepService = StartProcess("MiddleStepService\\MiddleStepService.csproj");
            await Task.Delay(5000);
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public void Dispose()
    {
        _gRPCService?.Kill();
        _gRPCService?.Dispose();

        _MiddleStepService?.Kill();
        _MiddleStepService?.Dispose();
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
