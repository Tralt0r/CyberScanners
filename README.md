# CyberScanners

Bloons Tower Defense inspired game designed to be more challenging and provide more content for players to enjoy.

https://github.com/Tralt0r/CyberScanners


\--------------------------------------------------------------------------------------------------------------------


3/29/26

Added

\-Bomb Tower - self explodes when enemies in range

\-Tiles that take any space that arent part of the enemy path and give a visual for placable tower spots

\-Visuals showing range of towers when they are selected


\--------------------------------------------------------------------------------------------------------------------



3/11/26



Balances changes to

\-Short Tower - buffed stats and upgrades

\-Ranged Tower - buffed stats and upgrades

\-Wave Progression - per round there is a cost for enemies to spawn each enemies costs different and budget increases per round

\-Wave Patterns - stronger enemies will try to spawn before weaker ones

\-Economy System - more data per round win

\-Enemy wave order - changed the rounds some enemies will spawn on

\-Tower Costs



Added

\-Cannon - shoots projectiles that explode

\-Visual Pathways - image textures for paths

\-Projectiles(Normal \& Explosive)

\-Projectile Pool - optimization for projectiles

\-Upgrade System - towers can be upgraded

\-Enemies inside enemies - enemies can spawn inside enemies

\-Visual Background

\-Selling Towers - sell towers for data

\-Camo Enemies - can only be targeted by certain towers

\-Explosive Enemies - can damage towers in a radius on death

\-Nerf Tower - weakens enemies

\-Orange Material

\-White Material

\-Simple Basic Main Menu





\--------------------------------------------------------------------------------------------------------------------



3/6/26



added 3 enemies



\-fast enemy 3, light HP

\-slow enemy 2, heavy HP

\-regular enemy 1, normal HP



added enemy path for enemies to follow

\-use waypoints to guide enemies and has a line to show pathing using gizmos





added grid placement system for towers

\-uses a grid system with no collision and checks if a tower can be placed displayer by using gizmos



added canvas for UI

\-loss screen



added wave progression system

\-wave system that spawns in more enemies when all previous enemies are dead

\-reward for completing a wave



added debugging materials

\-Red(Enemy)

\-Green(Enemy)

\-Yellow(Tower)
-Blue(Enemy)



added core system

\-core system deciding if the player loses, just tracks if an enemy has reach the end(not this script) and updates the core health



added economy system

\-tracks players money or "data" to use for tower purchases

