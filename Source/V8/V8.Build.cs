using UnrealBuildTool;
using System.IO;
using System;

public class V8 : ModuleRules
{
    protected string ThirdPartyPath
    {
        get { return Path.GetFullPath(Path.Combine(ModuleDirectory, "..", "..", "ThirdParty")); }
    }

    public V8(TargetInfo Target)
    {
        if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            PrivateIncludePaths.AddRange(new string[]
            {
                Path.Combine("/usr/local/opt/v8/include"),
                Path.Combine("/usr/local/opt/v8"),
                Path.Combine("V8", "Private")
            });
        }
        else
        {
            PrivateIncludePaths.AddRange(new string[]
            {
                Path.Combine(ThirdPartyPath, "v8", "include"),
                Path.Combine(ThirdPartyPath, "v8"),
                Path.Combine("V8", "Private")
            });
        }

        PublicIncludePaths.AddRange(new string[]
        {
            Path.Combine("V8", "Public")
        });

        PublicDependencyModuleNames.AddRange(new string[] 
        { 
            "Core", "CoreUObject", "Engine"
        });

        if (UEBuildConfiguration.bBuildEditor)
        {
            PublicDependencyModuleNames.AddRange(new string[]
            {
                "DirectoryWatcher"
            });
        }

        PrivateDependencyModuleNames.AddRange(new string[] 
        { 
            "Sockets"
        });

        if (UEBuildConfiguration.bBuildEditor)
        {
            PrivateDependencyModuleNames.AddRange(new string[] 
            { 
                "UnrealEd"
            });
        }

        bEnableExceptions = true;

        LoadV8(Target);
    }

    private bool LoadV8(TargetInfo Target)
    {
        if ((Target.Platform == UnrealTargetPlatform.Win64) || (Target.Platform == UnrealTargetPlatform.Win32))
        {
            string LibrariesPath = Path.Combine(ThirdPartyPath, "v8", "lib");

            if (Target.Platform == UnrealTargetPlatform.Win64)
            {
                LibrariesPath = Path.Combine(LibrariesPath, "Win64");
            }
            else
            {
                LibrariesPath = Path.Combine(LibrariesPath, "Win32");
            }

            if (Target.Configuration == UnrealTargetConfiguration.Debug)
            {
                LibrariesPath = Path.Combine(LibrariesPath, "Debug");
            }
            else
            {
                LibrariesPath = Path.Combine(LibrariesPath, "Release");
            }

            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_base_0.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_base_1.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_base_2.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_base_3.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_libbase.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_libplatform.lib"));
            PublicAdditionalLibraries.Add(Path.Combine(LibrariesPath, "v8_nosnapshot.lib"));

            Definitions.Add(string.Format("WITH_V8=1"));
            Definitions.Add(string.Format("WITH_V8_FAST_CALL=0"));
            Definitions.Add(string.Format("WITH_JSWEBSOCKET=1"));

            return true;
        }
        else if (Target.Platform == UnrealTargetPlatform.Android)
        {
            string LibrariesPath = Path.Combine(ThirdPartyPath, "v8", "lib", "Android");
            PublicLibraryPaths.Add(Path.Combine(LibrariesPath, "ARMv7"));
            PublicLibraryPaths.Add(Path.Combine(LibrariesPath, "ARM64"));
            PublicLibraryPaths.Add(Path.Combine(LibrariesPath, "x86"));
            PublicLibraryPaths.Add(Path.Combine(LibrariesPath, "x64"));

            PublicAdditionalLibraries.Add("v8_base");
            PublicAdditionalLibraries.Add("v8_libbase");
            PublicAdditionalLibraries.Add("v8_libplatform");
            PublicAdditionalLibraries.Add("v8_nosnapshot");

            Definitions.Add(string.Format("WITH_V8=1"));
            Definitions.Add(string.Format("WITH_V8_FAST_CALL=0"));
            Definitions.Add(string.Format("WITH_JSWEBSOCKET=0"));

            return true;
        }
        else if (Target.Platform == UnrealTargetPlatform.Mac)
        {
            string LibPath = "/usr/local/opt/v8/lib/";
            string DyLibPath = "/usr/local/lib/";
            PublicLibraryPaths.Add("/usr/local/opt/v8/lib");

            PublicAdditionalLibraries.Add(DyLibPath+"libv8.dylib");
            PublicAdditionalLibraries.Add(LibPath+"libv8_base.a");
            PublicAdditionalLibraries.Add(LibPath+"libv8_libbase.a");
            PublicAdditionalLibraries.Add(LibPath+"libv8_libplatform.a");
            PublicAdditionalLibraries.Add(LibPath+"libv8_nosnapshot.a");

            Definitions.Add(string.Format("WITH_V8=1"));
            Definitions.Add(string.Format("WITH_V8_FAST_CALL=0"));
            Definitions.Add(string.Format("WITH_JSWEBSOCKET=0"));

            return true;
        }
        Definitions.Add(string.Format("WITH_V8=0"));
        Definitions.Add(string.Format("WITH_V8_FAST_CALL=0"));
        Definitions.Add(string.Format("WITH_JSWEBSOCKET=0"));
        return false;
    }
}
