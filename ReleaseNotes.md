# v1.3.23

## Improvements
- Add additional logging messages to better diagnose errors when parsing logs

# v1.3.22
02.12.24

## Features
- Add support for Wing 8 - Mount Balrior

## Bugfixes
- Fix local log parsing for EI 3.0.0 and onward

# v1.3.21
26.10.24

## Improvements
- Update role detection to autumn 2024 balance
- Add Spirit Woods encounter detection

## Bugfixes
- Fix unhanded exception during startup related to EI version checking

# v1.3.20
21.06.24

## Bugfixes
- Fix recent logs being detected and added multiple times

# v1.3.19
14.06.24

## Features
- Add support for End of Dragons and Secrets of the Obscure Fractals
    - Silent Surf - Kanaxai
    - Lonely Tower - Cerus and Deimos
    - Lonely Tower - Eparch

## Bugfixes
- Fix crash when certain characters were present in the path of an .evtc file
- Fix issue with some Encounters not being properly detected by folder name
- Update year of copyright notice to 2024

# v1.3.18
12.09.2023

## Features
- Add support for Secrets of the Obscure Strikes
    - Cosmic Observatory - Dagda
    - Temple of Febe - Cerus
    
## Improvements
- Update role detection to august 2023 balance patch

## Bugfixes
- Update year of copyright notice in the installer to 2023

# v1.3.17
18.05.2023

## Features
- Strikes can now be updated in added to RaidOrga+ via update Termin
- Add support for Old Lion's Court Strike
- Add support for two new RaidOrga+ roles: Alacrity and Quickness

## Improvements
- Add detection for Minister Li CM
- Rework role detection for RaidOrga+ to support multiple roles
- Update role detection for RaidOrga+ to new balance patch
- Update role detection for RaidOrga+ to order the roles consistantly
- Remove banner role from role detection for RaidOrga+
- Add support for rate limiting of dps.report
- DRM's are combined into Primordus and Jormag as areas
- Improve comatibility when GW2 account name differ by capitalization in Game and RaidOrga+

## Bugfixes
- Fix issue with certain characters when parsing log locally
- Fix issue with detecting cerain river logs
- Fix issue with icon of What's New dialog
- Fix issue when unparseable log is included in RO+ update selection

# v1.3.16
15.05.2022

## Features
- Strikes are now grouped by expansion when posting to discord using any per wing formatting

## Bugfixes
- Fix crash when cerain characters were used in a charakter name
- Fix hint text in settings to include RO+ Lieutenants feature

# v1.3.15
25.04.2022

## Features
- New Encounters:
    - Strike Mission: Aetherblade Hideout
    - Strike Mission: Xunlai Jade Junkyard
    - Strike Mission: Kaineng Overlook
    - Strike Mission: Harvest Temple

## Improvements
- Improve memory usage when processing a log

# v1.3.14
21.03.2022

## Improvements
- Add discord icons for EoD specs

# v1.3.13
06.03.2022

## Improvements
- Specter icon added
- Catalyst icon updated

## Bugfixes
- Fix incorrect RaidOrga+ profession abbreviations

# v1.3.12
07.01.2022

## Improvements
- Updating a raid in RaidOrga+ no longer overrides secondary roles
- Added support for the new EoD specs
  - Mechanist
  - Untamed
  - Specter (no icon available)

# v1.3.11
24.09.2021

## Improvements
- Added support for the new EoD specs
  - Bladesworn
  - Catalyst
  - Vindicator

# v1.3.10
19.08.2021

## Improvements
- Added support for the new EoD specs
  - Harbinger
  - Virtuoso
  - Willbender

## Bugfixes
- Fix broken Wing 3 image

# v1.3.9
10.08.2021

## Improvements
- Added support for the new RaidOrga+ Lieutenants feature
- Imporove prformance of loading RaidOrga+ raids
- Only logs with a boss that has a valid RaidOrga+ linking, will be processed on 'Update Termin'

## Bugfixes
- Fixed that a invalid RaidOrga+ login was not properly detected and displayed
- Fixed RaidOrga+ raid date and time not displying correctly
- Fixed new escort logs crashing on upload
- Hopefully fixed a bug when updating a raid in RaidOrga+

# v1.3.8
18.04.2021

## Improvements
- Ordering of players on new Bosses in RO+ is now more stable

## Bugfixes
- Fixed bug when processing certain bosses for RO+

# v1.3.7
17.04.2021

## Bugfixes
- Fixed bug in new normal and CM encounter matching

# v1.3.6
14.04.2021

## Features
- New Encounters:
    - Dragon Response Mission: Caledon Forest
    - Dragon Response Mission: Bloodtide Coast
    - Dragon Response Mission: Fireheart Rise
    
## Improvements
- Thoughness tanks are now overritten on RO+ update
- Bannar role with non warrior profession is now overritten
- If a banner warrior is in set in RO+ and was warrior no additional banner warriors should be added on update RO+
- Improved general programm stability
- Normal and CM missmatch between logs and RO+ can be resolved sometimes

## Bugfixes
- Fixed crash if any RaidOrga+ Termin had no endtime
- Fixed verbosity of logging on startup with no logging args
- Fixed crash wehn trying to update a River or Deimos Encounter in RO+

# v1.3.5
25.02.2021

## Improvements
- Support Heralds can now be detected

## Bugfixes
- Webhook posts with 'הצü' should be possible again

# v1.3.4
31.01.2021

## Features
- RaidOrga+ Integration for RaidLeads<br>
Update all missing bosses, classes, roles and names in seconds
- New Encounters:
    - Strike: Cold War
    - Fractal: Sunqua Peek
    - Dragon Response Mission: Metrika
    - Dragon Response Mission: Brisban
    - Dragon Response Mission: Gendarran
    - Dragon Response Mission: Fields of Ruin
    - Dragon Response Mission: Thunderhead Peaks
    - Dragon Response Mission: Lake Doric
    - Dragon Response Mission: Snowden Drifts
- Parse, Upload, Open local and Open dps.report buttons are now dependent on your selection
- New Logger, Logfiles are now created<br>
Last 30 Logs are saved in %appdata%/LogUploader/Logs/
- New programm arguments for LogLevel (-d, -w, -i, -v | debug, warning, info, verbose)
- Added Support for Alpha and Beta versions
- New crash popup<br>
It was never so easy to get the log
- New static configuration path.<br>
Never loose your config again after updating
- You can now export and import your settings.


## Improvements
- Now Target DPS is shown next to log
- New installer packaging. Installers should no longer be flagged as a virus
- Improved JSON parsing performance
- Added Caching for api.github.com request to improve performance
- EliteInsights update now cancels after 5 min
- Program update now cancels after 5 min
- Loading bar on startup is smother than ever
- Exitcodes are now labeled and logged
- Added badges to the README.md
- Added note for Raidlead requirement for RO+ integration
- Fixed spelling mistakes and improved some lines


## Bugfixes
- Fixed loading bar on startup jumping backwards
- License should now be properly detected by GitHub
- Subgroup header in player perview is not displayed correctly
- Logs with outdated metadata can now be reuploaded or reparsed
- Added localisation for Allow Beta Updates option
- Fixed crash on failed installer download
- Fixed crash when starting multible instances at once

# v1.3.4 Devnotes

## Improvements
- Fixed spelling mistakes and improved some lines
- Added roldetection for Escort
- Ajusted sorting for new Escort and Deimos encounters in RO+

## Bugfixes
- Fixed crash on failed installer download
- Fixed crash when starting multible instances at once

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