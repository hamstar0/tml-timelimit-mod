# Time Limit

Time Limit does what it says on the tin. Worlds can now be set with time limits to have configurable events take place. For example, to run a timer for 5 minutes that exits the game on completion, type this into the chat or server console:

  `/timerstart 300 exit false`

List of commands:
* 'timerstart <seconds> <action> <loops>' - Begins the timer with the given number of seconds before the given action is executed.
* 'timerallstop' - Aborts all currently running timers.
* 'timerallpause' - Pauses all timers.
* 'timerallresume' - Pauses all timers.

List of actions:

* `none` - No action. Timer exists for show only.
* `exit` - Exits the server/returns to menu.
* `serverclose` - Closes the server.
* `kill` - All players die.
* `hardkill` - All players die as if in hardcore mode (i.e. permadeath).
* `afflict` - All players receive a permanent affliction as specified in the config file's 'Afflictions' (de)buff list.
* `unafflict` - All players recover from permanent afflictions from the (de)buff list.
* (custom action name) - Same as `none`, unless another mod implements the named action via. API.

See the [Mod Helpers](https://forums.terraria.org/index.php?threads/mod-helpers-a-modders-mod-for-mods-and-modding.63670/) Control Panel for configuration options.