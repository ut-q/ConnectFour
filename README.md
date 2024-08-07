WELCOME TO CONNECT FOUR: GAME OF THE YEAR EDITION

This is a quick implementation of Connect Four, with some extra features in there, just for fun...

I had a lot of fun implementing this, I'm a better Connect Four player than I was after finishing this for sure, but I'm still not good at all :)

My primary C# IDE is Jetbrains Rider, so I used that here as well.

The game has two game modes: classic and pop out. Classic is the classic Connect Four we all love. Pop Out is a mode where you can also remove disks from the bottom if the bottom disk is yours. 

I implemented the AI using minimax search and alpha-beta pruning. 

My main reference was this: http://blog.gamesolver.org/solving-connect-four/01-introduction/

I also used some ideas from this thread: https://softwareengineering.stackexchange.com/questions/263514/why-does-this-evaluation-function-work-in-a-connect-four-game-in-java

Given the limited time, I didn't implement some of the optimizations such as transposition tables (dynamic programming) and packing the board/move info into smaller chunks to save memory. The performance is still pretty good though. There are 4 AI difficulty levels, the only difference between them is the minimax tree search depth. Honestly, I can barely beat the easiest one. The easy difficulty has only 1 depth and it takes a few milliseconds to execute in classic mode. The "Pro" difficulty uses 7 depth in the search and execution roughly takes around 300 milliseconds per move. I used precalculated matrices as part of the evaluation function. I hardcoded the 6x7 board evaluation matrix but a simple tool can calculate matrices for other board sizes. I didn't implement it since it was out of the scope of the task.

The entire endeavor took me about 7 relaxed hours:

Planning: 20 minutes

Human vs Human Connect Four: 2 hours

AI Implementation: 2.5 hours

Game Modes: 1.5 hours

Polish: 40 minutes

A screenshot of my Miro planning board can be seen here: https://github.com/ut-q/ConnectFour/blob/master/Miro%20Planning.png 
