# v1.3.4 Dev

## Improvements
- Fixed spelling mistakes and improved some lines

## Bugfixes+
- Fixed crash on failed installer download

# v1.3.3 BETA
27.01.2021

## Features
- Added Dragon Response Mission:
    - Fields of Ruin
    - Thunderhead Peaks
    - Lake Doric
    - Snowden Drifts
- Added reload button for RaidOrga+ raid selection

## Improvements
- New installer packaging. Installers should no longer be flagged as a virus
- Added note for Raidlead requirement for RO+ integration
- Added experimental role detection for Quadim 1

## Bugfixes
- Added localisation for Allow Beta Updates option


# v1.3.2 BETA
19.01.2021

## Bugfixes
- Bug in reading installer download url

# v1.3.1 BETA
19.01.2021

## Bugfixes
- Subgroup header in player perview is not displayed correctly
- Logs with outdated metadata can now be reuploaded or reparsed


# v1.3.0 BETA
19.01.2021

## Features
- New Encounters:
    - Strike: Cold War
    - Fractal: Sunqua Peek
    - Dragon Response Mission: Metrika
    - Dragon Response Mission: Brisban
    - Dragon Response Mission: Gendarran

- New Logger, Logfiles are now created<br>
Last 30 Logs are saved in %appdata%/LogUploader/Logs/
- New programm arguments for LogLevel (-d, -w, -i, -v | debug, warning, info, verbose)
- Added Support for Alpha and Beta versions
- New crash popup<br>
It was never so easy to get the log
- Parse, Upload, Open local and Open dps.report buttons are now dependent on your selection
- New static configuration path.<br>
Never loose your config again after updating
- You can now export and import your settings.
- RaidOrga+ Integration for RaidLeads<br>
Update all missing bosses, classes, roles and names in seconds


## Improvements
- Improved JSON parsing performance
- Exitcodes are now labeled and logged
- Added badges to the README.md
- Added Caching for api.github.com request to improve performance
- EliteInsights update now cancels after 5 min
- Program update now cancels after 5 min
- Loading bar on startup is smother than ever
- Now Target DPS is shown next to log

## Bugfixes
- Fixed loading bar on startup jumping backwards
- License should now be properly detected by GitHub

# v1.2.31
11.11.2020

## Bug fixes
- Fixed nullpointer exception when no webhooks are configured

# v1.2.30
11.11.2020

## Improvements
- Discord-Webhook links are now automatically updated to use the hostname discord.com

# v1.2.29
28.07.2020

## Features
- What's New screen after updates showing these patchnotes
- New formats for Discord-Posts
  - Compact
  - Compact with Classes
  - Per wing with Classes

## Bugfixes
- Timeout issues should be now resolved
- Server-Emotes now work
- More typos sqaushed
- Precautionary introduced fix for renaming of Broken King in german
- Renamed Qadim from W6 to Qadim 1 to improve search

# v1.2.28
16.07.2020

## Features
- New "Compact with Emotes" formating option for discord posts
- Integrated external spellchecking

## Bugfixes
- Now only loading evtc file formats
- Fixed error opening invalid files
- Timout for uploads increased to prevent errors
- Fixed minor visual errors in the user interface

# v1.2.27
20.06.2020

## Bug fixes
- Fixed crash when selecting lots of logs via mouse dragging
- Increased timout for log uploading to accout for slow connections

# v1.2.26
10.06.2020

## Improvements
- Improved Installer
- Added new emotes for Kill and Wipe
- Added Discordpost-Format "Per area with emotes"
- Updated readme

## Bug Fixes
- View License in About
- Programm crash when attempting to post to Discord when no valid logs for were selected

# v1.2.25
08.06.2020

## Improvements
- New error reporting if programm crashes
- About window now features links to this repository and the according release

## Bug Fixes
- Removed ReloadLangXML button
- Discard changes message by settings should no longer be displayed always
- Bossnames should be detected even if use charakter name and or use guild is selected in arcdps

# v1.2.24
06.06.2020

## Features
- improved crash reproting

## Bugfixes
- human readiable error if no EI is installed

# v1.2.23
06.06.2020

## Bug Fixes
- adding Bosses with specific length caused an error
- added more checks if download calls fail

# v1.2.22
05.06.2020

## Features
- Auto Update

# v1.2.21
05.06.2020

The first canidate for beta testing