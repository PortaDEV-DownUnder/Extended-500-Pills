﻿using System;
using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Loader;
using YamlDotNet.Serialization;

namespace ExtendedPills.Config;

public class Config : IConfig
{
    [YamlIgnore] public Items ItemConfigs { get; private set; } = null;

    [Description("Whether or not debug messages shoudl be shown.")]
    public bool Debug { get; set; } = true;
    [Description("The folder path where Items configs will be stored.")]
    public string ItemsFolder { get; set; } = Path.Combine(Paths.Configs, "Items");
    [Description("The file name to load role configs from.")]
    public string RolesFile { get; set; } = "global.yml";
    [Description("Whether or not this plugin is enabled.")]
    public bool IsEnabled { get; set; } = true;

    public void LoadConfigs()
    {
        if (!Directory.Exists(ItemsFolder)) Directory.CreateDirectory(ItemsFolder);
        string text = Path.Combine(ItemsFolder, RolesFile);
        if (!File.Exists(text))
        {
            Log.Warn($"The file {text} does not exist. Creating a new one.");
            ItemConfigs = new Items();
            File.WriteAllText(text, Loader.Serializer.Serialize(ItemConfigs));
        }
        else
        {
            ItemConfigs = Loader.Deserializer.Deserialize<Items>(File.ReadAllText(text));
            File.WriteAllText(text, Loader.Serializer.Serialize(ItemConfigs));
        }
    }
}