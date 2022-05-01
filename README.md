# Centipede
<img width="276.48" height="155.52" src="https://github.com/SergeiBak/PersonalWebsite/blob/master/images/centipede.png?raw=true">

## Table of Contents
* [Overview](#Overview)
* [Test The Project!](#test-the-project)
* [Code](#Code)
* [Technologies](#Technologies)
* [Resources](#Resources)
* [Donations](#Donations)

## Overview
This project is a recreation of the classic fixed shooter arcade game known as Centipede, originally released in 1981. This solo project was developed in Unity using C# as part of my minigames series where I utilize various resources to remake simple games in order to further my learning as well as to have fun!   

Centipede consists of a field of mushrooms, in which the Player is constrained to the bottom 'home zone' section of the play area. For each wave, the Player's goal 
is to defeat the centipede which makes its way down the field from the top of the screen. Each centipede segment that is shot turns into a mushroom, and shooting a 
centipede in the middle segment will cause it to split into two separate centipedes. Along the way, the Player will also encounter other enemies such as the spider, each with their own unique purpose/role in the game... Blast away and have fun seeing how long you survive!    
 
## Test The Project!
In order to play this version of Centipede, follow the [link](https://sergeibak.github.io/PersonalWebsite/centipede.html) to a in-browser WebGL build (No download required!).

## Code
A brief description of all of the classes is as follows:
- The `Blaster` class handles player movement as well as the state of the Player/Blaster.
- The `Centipede` class handles the overall state & spawning of the centipede.
- The `CentipedeDeathAnimation` class handles the death explosion animation used by the centipede + flea + scorpion.
- The `CentipedeSegment` class handles the movement + animation logic for each segment of the centipede as well as tracking its state.
- The `Dart` class handles the player input & movement of the dart projectile.
- The `Flea` class handles the logic of the flea enemy, which includes falling from top of screen & spawning mushrooms.
- The `FleaAnimation` class handles the animation of the flea.
- The `GameManager` is the game's main class, and keeps track of the state of the game as well as spawning enemies.
- The `Mushroom` class represents each mushroom on the board, and keeps track of its health as well as whether it is infected.
- The `MushroomField` class keeps track of all active mushrooms, and is responsible for generating/healing them.
- The `MushroomRepairAnim` class is responsible for the repair animation done on a damaged mushroom when resetting a round.
- The `Scorpion` class handles the logic of the scorpion enemy, which includes moving across the screen & infecting mushrooms.
- The `Spider` class handles the logic of the spider enemy, which includes moving across the home zone & eating mushrooms.
- The `SpiderAnimation` class handles the animation of the spider's movement.
- The `SpiderDeathAnimation` class handles the animation of the spider's death, as well as displaying the score earned.
- The `UIManager` class updates the state/color of all of the UI, as well as tracking high score.

## Technologies
- Unity
- Visual Studio
- GitHub
- GitHub Desktop

## Resources
- Credit goes to [Zigurous](https://www.youtube.com/channel/UCyaKsKqYTghxgAqywfefAzg) for the helpful base game tutorial!
- I made use of [Unity PlayerPrefs](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) for saving stats
- Gameplay [reference](https://www.youtube.com/watch?v=V7XEmf02zEM)
- Game rules [guide](https://www.digitpress.com/library/books/book_vmg_centipede.pdf)
- Characters + animation [reference](https://www.retrogamedeconstructionzone.com/2020/08/the-characters-of-centipede.html)

## Donations
This game, like many of the others I have worked on, is completely free and made for fun and learning! If you would like to support what I do, you can donate at my metamask wallet address: ```0x32d04487a141277Bb100F4b6AdAfbFED38810F40```. Thank you very much!
