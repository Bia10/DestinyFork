using System;
using System.Windows.Shell;

using Destiny.IO;

namespace Destiny.Shell
{
    public static class Shortcuts
    {
        public static void Apply()
        {
            if (Environment.OSVersion.Version.Major < 6 || Environment.OSVersion.Version.Minor < 1) return;

            JumpList jumpList = new JumpList();

            for (int id = 0; id < Settings.GetInt("Log/JumpLists"); id++)
            {
                jumpList.JumpItems.Add(new JumpTask()
                {
                    ApplicationPath = Application.ExecutablePath + "WvsGame.exe",
                    Title = "Launch Channel " + id,
                    Arguments = id.ToString()
                });
            }

            jumpList.Apply();

            if (System.Windows.Application.Current != null)
            {
                JumpList.SetJumpList(System.Windows.Application.Current, jumpList);
            }

            else if (System.Windows.Application.Current == null)
            {
                var newApplication = new System.Windows.Application();
                JumpList.SetJumpList(newApplication, jumpList);
            }

            else
            {
                Log.SkipLine();
                Tracer.TraceErrorMessage("Application not found!");
            }
        }
    }
}
