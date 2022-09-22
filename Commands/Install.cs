using System.Diagnostics;
public static class install
{
    public static string DownloadLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Installer";
    public static async Task InstallModule(this Module module)
    {
        Console.WriteLine("Downloading ... Please wait !");
        string location = Path.Combine(DownloadLocation, Path.GetTempFileName() + ".exe");
        await ServerCom.DownloadFileAsync(module.DownloadURL, location);
        Console.WriteLine();
        Process p = new Process();
        p.StartInfo = new ProcessStartInfo()
        {
            FileName = location,
            UseShellExecute = true,
            Verb = "runas"
        };

        p.Start();
        await p.WaitForExitAsync();

        File.Delete(location);

    }
}