
## Tricky Towers Mobile Version

### Introduction
As per guidelines this project is a light clone of “Tricky Towers”. It has 2 modes of gameplay one is single and other is multiplayer. In the single player mode the user has to make the highest tower without wasting the lives, as per instructions  the default target height is 40 and default lives are 3. In the multiplayer mode there is an Ai that is moving randomly left and right and building its tower, it is displayed on the left side of the screen, if the Ai achieves the target before the player within the defined lives , it will win and the player will lose. Have used the clean code with event based system &  SOLID principles 

Additionally in the Unity project there is a tab of Game Settings, by selecting it we can change the movement speed ,Tetris Rotation speed, dash speed, the maximum height to win & no of lives the players can have 


### Unity Version
According to the requirements , the unity version of this project is Unity 2021.3.7f1 LTS
### Project Structure
In the context of this Unity game project, an event-based project structure has been adopted to facilitate efficient communication and interaction between various game components. The main scripts are :

**GameEvents**  : Containing necessary events of tetris fall, placed & rotate. Also handing start ,win & lose events 

**GameConfig** : It's basically a scriptable object containing the basic game settings that include tetris move speed, dash speed max height to win & how many lives can a player have 

**InputSystem**: There is a base class of input system, not much work is done in that class, it serves as a base class for PlayerInput and AiInput 

**Tetris**: The script that is attached to tetris itself and handling the movement and necessary events of tetris placed and fall 

**TetrisSpawner** : This script is responsible for randomly spawning tetris blocks 

**ScoreHandler** : This script is responsible for calculating the height of tetris (score )

**UiManager** : Responsible for the Ui elements and their behavior 

**GameManager**: Responsible for changing the game state also managing the lives 

Also there are 2 Editor scripts that are responsible for changing the game configuration variables

### Testing Environment
I tested the game on 3 android devices 
Real me gt master 
Infinix hot 10
Real me 10
The game was working smooth on these devices


### Performance Analysis
This section will include sub-sections for analyzing key performance metrics.
Memory Footprint
There are 7 tetris pieces i have used a single material on them with Gpu instancing 
Changing their color with material property block 
The Batches are reduced between 20-50 in each game mode 



#### Framerate
I recorded 30 Fps On my testing device 


#### Garbage Collection
I have lowered the garbage collection by object pooling (Fly weight)
I spawns a gameobject then makes copy of that spawned objects and add them to pool to lower the GC
I have used a single material for blocks with gpu instancing and only changing the color of blocks by using a material property block

#### Binary Size
The build size is **34MB**

#### Optimization Strategies
Object pooling with spawned objects cloning 

Customized shaders for mobile diffuse shaders

GPU instancing 

Optimized mobile quality settings (Vsnyc count,realtime reflection probe  etc)
Clean code 

