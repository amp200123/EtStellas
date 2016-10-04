﻿using System;
using System.IO;

namespace BPA_RPG
{
    public class EventLogger
    {
        StreamWriter file;

        public EventLogger()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            file = File.CreateText("Logs/log_" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now) + ".txt");
        }

        public void Log(Type type, string log)
        {
            file.WriteLine("[" + string.Format("{0:HH:mm:ss.ff}", DateTime.Now) + "] [" + type.ToString() + "] " + log);
            file.Flush();
        }

        public void Log(object sender, string log)
        {
            Log(sender.GetType(), log);
        }
    }
}
