public class SandboxApplication : Application<SandboxApplication.Starter>
{
    public record Starter(IConsoleReader Console) : IApplicationStarter
    {
        public async ValueTask<Failable> Start()
        {
            await Task.CompletedTask;
            return Failable.Success();
        }

        private async void Test()
        {
            while (true)
            {
                await Task.Delay(1000);
            }
        }
    }
}