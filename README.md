# Dungeon-Crawler-Controller
A grid-based dungeon crawler character controller for use in Unity.

Intended for use on any object without a Rigidbody. Script utilizes four GameObject Vector3 targets to determine which way to properly move. It uses pinpoint Vector3 coordinates, so the player will always makes precise, unit-perfect movements. For proper set-up, make four (preferably) Empty objects and place them within the object using the Character Controller script, as well as the script's four public GameObject references (for all four directions respectively). Lastly, slap a camera inside the moving object, and you should be all set!

Because this script does not incorporate Rigidbodies, it sacrifices dynamic environment interactions/movements for accurate grid-like unit movements. As such, it is possible to phase completely through any walls or objects. To remedy this, the character controller has a Raycast feature implemented, where upon player input if there is an object tagged as "Wall" blocking the desired walk direction, the controller will not move. It also uses Raycasting for slope/floor detection, so the player is always properly positioned along the floor as it moves.
