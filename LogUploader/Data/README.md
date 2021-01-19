# How to personalize your installation
Any modifications are at your own risk and **updates will override the files**, so back up these and check if still valid after update!<br>
Changes made to any files may require a reboot of the program to take effect.

## dataconfig.json
Contains the data about:
- Areas: Wings, Strikes, etc...
- Bosses: The main Boss of an encounter
- Additional Enemies: currently unused
- Misc: Miscellaneous stuff

### Structure
```
{
    "LogUploaderData": {
        "GameAreas": {
            "RaidWings": [
                {}, {}, ...
            ],
            "StrikeMissions": [
                {}, {}, ...
            ],
            "Fractals": [
                {}, {}, ...
            ],
            "WvW": [
                {}
            ],
            "Training": [
                {}, {}, ...
            ],
            "Unknown": [
                {}
            ]
        },
        "Bosses": [
                {}, {}, ...
        ],
        "AddEnemys": [
                {}, {}, ...
        ],
        "Misc": {
            Emotes: {

            }
        }
    }
}
```
<br><br>
RaidWing<br>
```
{
    "ID": 42, //Positive internal ID uniqu within GameAreas/RaidWings
    "NameEN": "My Wing", //English display name
    "NameDE": "Mein Wing", //German display name
    "AvatarURL": "https://somesite.abc/imange.image" //Link to thumbnail image for discord posts
}
```

StrikeMission<br>
Same as RaidWing.

Fractal<br>
Same as RaidWing except ID is replaced with Level

WvW
```
{
    "NameEN": "World versus World", //English display name
    "NameDE": "Welt gegen Welt", //German display name
    "ShortNameEN": "WvW", //English short display name
    "ShortNameDE": "WvW", //German short display name
    "AvatarURL": "https://somesite.abc/image.image" //Link to thumbnail image for discord posts
}
```

Training<br>
Same as WvW

Unknown<br>
Same as WvW

Boss<br>
```
{
    "ID": 15438, //Ingame ID of the boss agent
    "NameEN": "My Chicken", //English display name
    "NameDE": "Mein Hun", //German display name
    "FolderEN": "Chicken", //English ingame name / arc folder name
    "FolderDE": "Hun", //German ingame name / arc folder name
    "EiName": "Chicken", //Internal name of Ei in generated .json files
    "GameAreaName": "RaidWings", //RaidWings, StrikeMissions, Fractals, WvW, Training or Unknown
    "GameAreaID": 1, //Existing id for the referenced section of GameAreas in GameAreaName
    "DiscordEmote": "<:emot:624958226538169953>", //The full emote string :emote: for defaut emotes <:emote:emoteID> for server spcific emotes (generated to typing \:emote: on the respective server)
    "AvatarURL": "https://wiki.guildwars2.com/images/f/fb/Mini_Vale_Guardian.png", //Link to thumbnail image for discord posts
    "RaidOrgaPlusID": 1 //The RaidOrgaPlusID of the Boss. If -1 the Boss will be ignored when updating RO+ raids.
}
```

AddEnemy<br>
```
{
    "ID": 2, //Ingame ID of the enemy agent
    "NameEN": "Add", //English display name
    "NameDE": "Monster", //German display name
    "GameAreaName": "RaidWings", //RaidWings, StrikeMissions, Fractals, WvW, Training or Unknown
    "GameAreaID": 7, //Existing id for the referenced section of GameAreas in GameAreaName
    "Intresting": false // if it is vital/interesting for the encounter
}
```

Misc/Emotes<br>
```
{
    "Kill": "<:kill:635845725569871536>", //The full emote string representing a boss kill :emote: for default emotes <:emote:emoteID> for server specific emotes (generated to typing \:emote: on the respective server)
    "Wipe": "<:wipe:629864583265852294>" //The full emote string representing a wipe :emote: for default emotes <:emote:emoteID> for server specific emotes (generated to typing \:emote: on the respective server)
}
```

## ProfessionData.json
Contains the data about classes and elite specializations

### Structure
```
{
    "Professions": [
        {
            "NameEN": "MyClass", //English display name
            "NameDE": "MeineKlasse", //German display name
            "IconPath": "\\images\\Professions\\MyClass.png", // Path to a small image representing the class. By default these images are located in installationFolder\\images\\Professions\\
            "Emote": "<:emot:624958226538169953>", //The full emote string representing the class :emote: for default emotes <:emote:emoteID> for server specific emotes (generated to typing \:emote: on the respective server)
            "RaidOrgaPlusID": 0 // The RaidOrgaPlus ID of the Class
        }
    ]
}
```

## English.xml and German.xml
Contains all GUI strings in English or German.

## EIconf.conf
The Default config used for EliteInsights for local parsing. Config options that can be controlled through the settings of LogUploader are appended on parsing.