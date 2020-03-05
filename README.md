# Dungeon-Crawler-Controller
A grid-based dungeon crawler character controller for use in Unity.

Intended for use on any object without a Rigidbody. Just slap the script and a camera inside the object and you're all set! Script utilizes an internal compass (NSEW) to orientate itself and keep track of which direction it is facing + determine which way to properly move. It moves using pinpoint Vector3 coordinates, so the player will always makes precise, unit-perfect movements.

Because this script does not incorporate Rigidbodies, it sacrifices dynamic environment interactions/movements for accurate grid-like unit movements. As such, it is possible to phase completely through any walls or objects. To remedy this, the character controller has a Raycast feature implemented, where upon player input if there is an object tagged as "Wall" blocking the desired walk direction, the controller will not move. So far the code only supports horizontal control, so vertical elements such as slopes are rendered useless at the moment.
