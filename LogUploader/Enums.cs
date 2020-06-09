﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader
{
    public enum eLanguage
    {
        [CombBoxView("English")]
        EN,
        [CombBoxView("German")]
        DE
    }
    
    public enum eDiscordPostFormat
    {
        [ObjectName("per Boss", "pro Boss")]
        PerBoss,
        [ObjectName("per Area", "pro Gebiet")]
        PerArea,
        [ObjectName("per Try Detaild", "pro Versuch ausführlich")]
        PerTryDetaild,
        [ObjectName("per Area with emotes", "pro Gebiet mit Emotes")]
        PerAreaEmotes
    }
}
