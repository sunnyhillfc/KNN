# Implementation of K-Nearest Neighbor (k-NN) Algorithm with C#

![Project Image](/screencap.png)

## Description

K Nearest Neighbors algorithm implementation in C#. Running on the IRIS dataset. Runs with self contained .exe, complied on linux with mcs and vs on windows

## How To Use
Clone Repository and run .exe from the same folder. Both folders contain the train and test dataset

#### Re-Compiling on linux
Requires mcs and mono 

```bash
mcs Program.cs KNN.cs 
```
```bash
mono Program.exe
```

#### Windows Compilation
Copy the two .cs files into a new visual studio project (console app, .NET 3.1) 
