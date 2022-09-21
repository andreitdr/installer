public class Module
{
    public string Name { get; private set; }
    public string Architecture { get; private set; }
    public string DownloadURL { get; init; }
    public ModuleType Type { get; init; }
    public Module(string name, string architecture, string downloadURL, ModuleType type)
    {
        Name = name;
        Architecture = architecture;
        DownloadURL = downloadURL;
        Type = type;
    }

    public static ModuleType GetModuleTypeFromString(string type)
    {
        foreach (var t in Enum.GetNames<ModuleType>())
        {
            //Console.WriteLine(t.ToString());
            if (t.ToLower().Replace('_', ' ') == type.ToLower())
                return (ModuleType)Enum.Parse(typeof(ModuleType), t, true);
        }

        throw new Exception("Type not found " + type);
    }

    public enum ModuleType
    {
        TORRENT_CLIENT, TOOL, GAME_LAUNCHER, BROWSER, CODE_EDITOR,
    }

    public static async Task<Module> GetModule(string name)
    {
        string arch = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString().ToLower();
        var itemList = await ServerCom.ReadTextFromURL("https://raw.githubusercontent.com/Wizzy69/installer/csharp-installer-console/Items");
        foreach (var item in itemList)
        {
            if (item.Length < 2) continue;
            string[] itemData = item.Split(',');
            if (itemData[0] == name && itemData[1] == arch)
            {
                return new Module(itemData[0], itemData[1], itemData[2], GetModuleTypeFromString(itemData[3]));
            }
        }

        throw new Exception($"Module with name {name} not found on architecture {arch}");
    }
}