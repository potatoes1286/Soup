# Potatoes' Soup

If you need to contact me, you can do so on [homebrew.](https://discord.gg/83yTrfr)

Download on [Thunderstore](https://h3vr.thunderstore.io/package/Potatoes/Potatoes_Soup/)

Potatoes' Soup is a much of mixed ingredients to make your H3VR game a lot better!

Full list of additions/changes

##### Base Game Patches
- The size of a laser's red dot is no longer dependant on the distance between the dot and the gun, but on the distance between the dot and the player. (In the base game, the laser pointer being very far away will cause the dot to be very big if you are near the dot.)
- Corrects the Madsen LAR and Panzerschreck sizes to be correct (Not very noticable on the Panzerschreck, but very much so for Madsen. Panzerschreck changed from a bore size of 95mm to 88mm, Madsen sized down from 1230mm to 1080mm)
- BreakActionWeapons no longer break from adding a suppressor
##### Bolt Bracing
- For guns that have a stock, you can brace the stock against your body, pull the bolt back with one hand, and then let go of the gun with your other hand. This frees up one hand while keeping the bolt open with the other.
- Very useful for locking a bolt back on a gun like the SKS to fill it up
- Works on Bolt Action rifles, Closed Bolt guns, and handguns which have a stock
- Does not work with charging handles
- Disables Anton's bolt locking feature
##### Bullet Thumbing
- Holding down the touchpad while racking the slide of a gun will "thumb" the bullet in the chamber if it is not fired, inserting it back into the magazine instead of ejecting it.
- Works with Closed Bolt guns, Bolt Action guns and handguns
- Does not work if using a charging handle
##### Clear Stabilization
- Two-hand stabilizing a gun will now not display your offhand
- Can be disabled in the config
- Distance before hiding offhand can be changed in the config
##### Double Action Revolver Decocking
- Holding down the touchpad while pulling the trigger on a cocked double action will now decock the revolver
##### Bolt Action Manipulation
- Up on the touchpad now cocks the hammer if it is decocked
##### Easy Attaching
- Attachments will now phase through held guns, making it easier to attach attachments
- Can be disabled in the config
##### Better Panels
- Options panel now spawn locked in place
##### Palm Racking
- Holding grip on your empty hand then hovering over a bolt will automatically grab the bolt. This allows you to hold grip then swipe over a bolt and quickly rack it.
- Works on closed bolt guns, open bolt guns, and bolt action guns
##### Ejector Hitting
- Revolver ejector can now be slapped (swipe hand over, similar to MP5 handle) to eject rounds
##### Single Action Counter-Clockwise Rotation
- Single action revolver cylinders can be rotated counter-clockwise with up on the touchpad
##### Better Stabilization
- Two-hand stabilization now works even when holding another item in the other hand
##### Akimbo Reloading
- If you are holding two pistols in your hands, you can simply hover them over magazines in your quickbelt to automatically load them!
- Can be enabled/disabled in config
- This reloading style can be enabled when only holding one handgun in the config (off by default)
##### Bullet Bouncing
- Empty bullets bounce!
- Disableable in config. Bounciness can be modified in config.
##### Alternative Dual-Stage Trigger
- Guns with dual-stage triggers (ex. AUG) now have far less sensitive auto, making single-fire easier.
- Disableable in config.
##### Sensitive Break-Action Weapons
- Break action weapon latches are now twice as sensitive, making opening them far easier.
##### Grenade Hot-Potato
- Grenades (Pinned, capped, sosignades) are now far easier to catch mid-air for re-throwing pleasure.
- Catch range can be modified in config
##### Adaptive Eject Speed
- Bullets not eject from guns faster or slower depending on how fast you rack the slide/bolt.
- Can be disabled in config.

## Building

Download or build [H3VRUtilities](https://github.com/WFIOST/H3VRUtilities) and insert `/monomod/Assembly-CSharp.H3VRUtilities.mm.dll` from H3VRUtilities to `/src/libs` in your Soup project. The rest of the dependencies should be handled by NuGet.
