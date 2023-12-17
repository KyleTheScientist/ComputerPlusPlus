using BepInEx;
using BepInEx.Configuration;
using ComputerPlusPlus.Screens;
using ComputerPlusPlus.Tools;
using System;
using System.Reflection;

namespace ComputerPlusPlus
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    [BepInIncompatibility("tonimacaroni.computerinterface")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }
        public ConfigFile Config;

        void Awake()
        {
            Instance = this;
            Logging.Init();
            Config = new ConfigFile(Paths.ConfigPath + "\\" + PluginInfo.Name + ".cfg", true);
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            if (ComputerManager.Instance != null)
                ComputerManager.Instance.enabled = true;
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            if (ComputerManager.Instance != null)
                ComputerManager.Instance.enabled = false;
        }

        public void Setup()
        {
            ComputerManager.Instance = this.gameObject.AddComponent<ComputerManager>();
            ComputerManager.Instance.RegisterScreen(new RoomScreen());
            ComputerManager.Instance.RegisterScreen(new NameScreen());
            ComputerManager.Instance.RegisterScreen(new ColorScreen());
            ComputerManager.Instance.RegisterScreen(new ModsScreen());
            ComputerManager.Instance.RegisterScreen(new AutoJoinScreen());
            ComputerManager.Instance.RegisterScreen(new ThemeScreen());
            ComputerManager.Instance.RegisterScreen(new TurnScreen());
            ComputerManager.Instance.RegisterScreen(new VoiceScreen());
            ComputerManager.Instance.RegisterScreen(new QueueScreen());
            ComputerManager.Instance.RegisterScreen(new GroupScreen());
            ComputerManager.Instance.RegisterScreen(new ItemsScreen());

            foreach (var assembly in GetAssemblies())
            {
                //exclude the executing assembly
                if (assembly == typeof(Plugin).Assembly)
                    continue;
                foreach (var type in GetTypes(assembly))
                {
                    foreach (var iface in GetInterfaces(type))
                    {
                        try
                        {
                            if (iface.FullName == typeof(IScreen).FullName)
                            {
                                var screen = Activator.CreateInstance(type) as IScreen;
                                Logging.Debug($"Registering Screen: {screen.Title} from type {type.Name}");
                                ComputerManager.Instance.RegisterScreen(screen);
                            }
                        }
                        catch (Exception e) { Logging.Exception(e); }
                    }
                }
            }
            ComputerManager.Instance.RegisterScreen(new VersionScreen());
            ComputerManager.Instance.Initialize();
        }

        public Assembly[] GetAssemblies()
        {
            try
            {
                return AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (Exception e)
            {
                Logging.Fatal("Error getting assemblies");
                Logging.Exception(e);
                return null;
            }
        }

        Type[] GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch
            {
                Logging.Fatal($"Error getting types from assembly {assembly.FullName}");
                return new Type[] { };
            }
        }

        Type[] GetInterfaces(Type type)
        {
            try
            {
                return type.GetInterfaces();
            }
            catch
            {
                Logging.Fatal($"Error getting interfaces from type {type.FullName}");
                return new Type[] { };
            }
        }
    }
}
