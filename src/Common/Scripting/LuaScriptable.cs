using MoonSharp.Interpreter;

using System;
using System.Threading;

using Destiny.IO;

namespace Destiny.Scripting
{
    public abstract class LuaScriptable
    {
        private readonly string mPath;
        private readonly Script mScript;
        private readonly bool mUseThread;

        protected LuaScriptable(string path, bool useThread = false)
        {
            mPath = path;
            mScript = new Script();
            mUseThread = useThread;
        }

        public void Execute()
        {
            if (mUseThread)
            {
                try
                {
                    new Thread(new ThreadStart(() => mScript.DoFile(mPath))).Start();
                }

                catch (Exception ex)
                {
                    Log.SkipLine();
                    Tracer.TraceErrorMessage(ex, "Script thread could not be created!");
                    Log.SkipLine();
                }
            }
            else
            {
                mScript.DoFile(mPath);
            }
        }

        protected void Expose(string key, object value)
        {
            mScript.Globals[key] = value;
        }
    }
}