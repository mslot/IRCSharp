IRCSharp
========
IRCSharp is a modular IRC robot. Through a simple contract it is possible to extend the robot  with new commands, at runtime without rebooting.

Quick start
----------
See [Quick start guide] (https://github.com/mslot/IRCSharp/wiki/QuickStart). This readme has not been updated yet with examples on how to use the MessageQueue API, but I am in the process of writing one. If you are interested in seeing how it is done, then take a look in the MSMQTestProgram. Remember to install MessageQueue.

Known bugs
----------
This is an beta (second beta) release. So there is many UNKNOWN bugs. Be aware that this bot does not yet handle:

+ Netsplits
+ Mass querying the bot (and flooding the channel)

Tested
------
I have written some unit tests to support the two parsers. I have only tests the minimum required so I can get the bot up and running. If bugs is detected, let me know and I will patch the bot and update the tests.
The bot is only tested on Quakenet.

The MessageQueue has NOT been tested enough yet, so this part is still pretty alpha. More to come on this.

Next up
-------
I think that the bot, for now, is feature complete. It has all the features that I want, so now it all comes down to refactoring. I have refactored some code, from the first beta to the second beta, but not added more features to it. The next thing up is code cleaning, removing all those TODO's. I am also in the process of writing some good basic tutorials on how to extend the bot.

License
-------
 "THE BEER-WARE LICENSE" (Revision 42):
 <msl0t> wrote this file. As long as you retain this notice you can do whatever you want with this stuff. If we meet some day, and you think this stuff is worth it, you can buy me a beer in return.

Martin Slot.