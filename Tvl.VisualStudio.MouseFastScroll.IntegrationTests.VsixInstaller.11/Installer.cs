// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace Tvl.VisualStudio.MouseFastScroll.IntegrationTests.VsixInstaller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.ExtensionManager;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.Win32;
    using File = System.IO.File;
    using Path = System.IO.Path;

    public static class Installer
    {
        public static void Install(IEnumerable<string> vsixFiles, string rootSuffix)
        {
            var installerAssemblyName = typeof(Installer).Assembly.GetName().Name;
            var visualStudioMajorVersion = int.Parse(installerAssemblyName.Substring(installerAssemblyName.LastIndexOf('.') + 1));
            var installationPath = EnumerateVisualStudioInstancesInRegistry().Single(x => x.Item2.Major == visualStudioMajorVersion).Item1;
            AppDomain.CurrentDomain.AssemblyResolve += HandleAssemblyResolve;

            try
            {
                var vsExeFile = Path.Combine(installationPath, @"Common7\IDE\devenv.exe");

                using (var settingsManager = ExternalSettingsManager.CreateForApplication(vsExeFile, rootSuffix))
                {
                    var extensions = vsixFiles.Select(ExtensionManagerService.CreateInstallableExtension).ToArray();
                    var extensionManager = new ExtensionManagerService(settingsManager);

                    foreach (var extension in extensions)
                    {
                        if (extensionManager.IsInstalled(extension))
                        {
                            extensionManager.Uninstall(extensionManager.GetInstalledExtension(extension.Header.Identifier));
                        }
                    }

                    foreach (var extension in extensions)
                    {
                        extensionManager.Install(extension, perMachine: false);
                    }
                }
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= HandleAssemblyResolve;
            }

            return;

            Assembly HandleAssemblyResolve(object sender, ResolveEventArgs args)
            {
                string path = Path.Combine(installationPath, @"Common7\IDE\PrivateAssemblies", new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(path))
                {
                    return Assembly.LoadFrom(path);
                }

                return null;
            }
        }

        private static IEnumerable<Tuple<string, Version>> EnumerateVisualStudioInstancesInRegistry()
        {
            using (var software = Registry.LocalMachine.OpenSubKey("SOFTWARE"))
            using (var microsoft = software.OpenSubKey("Microsoft"))
            using (var visualStudio = microsoft.OpenSubKey("VisualStudio"))
            {
                foreach (string versionKey in visualStudio.GetSubKeyNames())
                {
                    if (!Version.TryParse(versionKey, out var version))
                    {
                        continue;
                    }

                    string path = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\{versionKey}\Setup\VS", "ProductDir", null) as string;
                    if (string.IsNullOrEmpty(path) || !File.Exists(Path.Combine(path, @"Common7\IDE\devenv.exe")))
                    {
                        continue;
                    }

                    yield return Tuple.Create(path, version);
                }
            }
        }
    }
}
