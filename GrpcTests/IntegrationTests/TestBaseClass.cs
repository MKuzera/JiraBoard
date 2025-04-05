using GrpcTests.IntegrationTests.Helpers;
using System.Diagnostics;
using Xunit;

public class TestBaseClass : IAsyncLifetime
{
    ProcessStarter _processes;
    public async Task InitializeAsync()
    {
        ProcessStarter instance = ProcessStarter.Instance;
    }

    public Task DisposeAsync()
    {
        _processes.Dispose();
        return Task.CompletedTask;
    }
}
