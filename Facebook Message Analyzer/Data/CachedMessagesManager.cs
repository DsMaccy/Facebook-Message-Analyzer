﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Message_Analyzer.Data
{
    class CachedMessagesManager
    {
        public static CachedMessagesManager Manager = new CachedMessagesManager();

        private CachedMessagesManager()
        {
        }
    }
}