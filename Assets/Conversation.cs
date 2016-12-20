using UnityEngine;
using System.Collections;

/*
The base class for all conversations.
Each conversation will be individually assigned to conversable NPC's (perhaps other things too).
It will consist of different nodes.
Each node will report back to the UI two things, what the NPC is saying and what they player response choices are.
The UI will accept player input and the conversation node will change based on that.
When moving to a different node, the idea is to have complete scripting freedom, so we can make any kind of event happen, not just dialogue.
For example, if there is a player response choice "Let me see your wares", the conversation will move to the next node, opening the vendor window and closing the dialogue, as well as resetting to the base node.
Each conversation needs to have a base node, which will be the one the conversation defualts to whenever it is initiated. 
If a conversation is stopped, we can either return to the base node, or save the position for later.
*/

/*
Another idea is to do it morrowind style, where instead of this class, it would be a Topic class.
Topics would be like items and they would be held in a dialogue "inventory".
Conversable NPCs would display any topics in their inventory on the UI.
When clicked, each topic can do something different.
Most will be designed to return a string to display in the UI.
Some, like a "Shop" topic, would close the dialog and open a vendor window.
The UI names of the topics could be set up in a RP friendly way, to mimic something actually said by the player.
For example, instead of seeing "Shop" or "Barter", the text would say "Let me see your wares".
The player clicks on that text and it would perform the "Shop" functionality.

This would most likely fit my vision better than branching dialog (as well as being much easier to implement in a solo project).
This way, I could focus on world and lore building, and the things I come up with could be placed in NPC dialog.
Think morrowind!
*/
public class Conversation {

}

/*
My dialogue system is written in C# so posting code might not be terribly helpful. 
However, it uses polymorphism to create a tree structure which is made up of nodes. 
Each node class extends a base node class, and has to implement begin, update and 
	finish methods ( which are called when the node is executed, every frame while 
	it's active and when it's done, respectively) and so each node inserts its own 
	functionality while still adhering to the stucture of the base class. 
Each node can have one or more children and these are returned by the Finish method. 
The Finish method is called when the Update method returns True.
Each node can implement its own system of storing children (in which case you have to cast the node to its actual type, of course) 
	or it can simply use the standard one, which is a list of children, all of which 
	are executed simultaneously after the current node is finished. 
Typically, each node only has one child.
If you want to add, for example, a dialogue menu, that would require its own children. 
So you'd have an array (or list) of options and when one is selected (either through Update or via callbacks setup in Begin) and Finish returns the option which corresponds to the user's choice.
You need to use function pointers or a good messaging system to make this work cleanly.
It's fairly simple, but fairly flexible, and it's easy to add new types of nodes when you decide you want your dialogue system to have features you didn't think of before.
How you actually store the data and/or design the dialogues is really independent of the code and design. 
I create mine in code, but you could read them from XML, or create a visual dialogue editor, but I'm using Unity, which already does 95% of the work for a visual editor so my code for that would be even more useless.
*/