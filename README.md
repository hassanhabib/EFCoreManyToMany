# EFCoreManyToMany
A quick demo to show many to many relationship with EF Core 3.1

I used this project to run Perf Tests to see the diff between `Task` and `ValueTask`

Here's my observations:

First Run:

![image](https://user-images.githubusercontent.com/1453985/156908556-e77c319f-fd3d-436b-ad7a-2bc2a6a07f3e.png)

Second Run:

![image](https://user-images.githubusercontent.com/1453985/156908619-bf8088b6-6b8d-479b-836b-2b0ed14f2af8.png)

Third Run:

![image](https://user-images.githubusercontent.com/1453985/156908729-1667bebb-77c6-4edf-b7c8-129abb09190b.png)

Fourth:

![image](https://user-images.githubusercontent.com/1453985/156908944-5243045b-1fde-4890-b5b0-f740bed386ba.png)



I have to say there has been some fluctuations in more runs, but overall `ValueTask` wins almost everytime.

