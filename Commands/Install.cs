using System.Diagnostics;
public static class install
{
    public static string DownloadLocation = "./Downloads/";
    public static async Task InstallModule(this Module module)
    {
        Directory.CreateDirectory(DownloadLocation);
        Console.WriteLine("Downloading ... Please wait !");
        string location = Path.Combine(DownloadLocation, module.Name + "_installer.exe");
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


    }
}