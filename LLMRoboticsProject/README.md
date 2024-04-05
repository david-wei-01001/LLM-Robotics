# Reproducing Experiments
- Download and open LLMRoboticsProject in Unity 2022.
- Reset the scene according to the experiment wishing to reproduce


## Bar Cube Experiment
- Open Assets/Prefabs
- load ***Apperatus.prefab*** and ***Target2.prefab*** into the scene
- on the inspector window of ***Apperatus.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***barMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: LRCameraLeft GameObject
    - AB Camera: ABCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - Bar: Apperatus GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Clisk()
- select **Runtime Only**
- drag ***barMove*** GameObject underneath it
- on the right, select ***BarMover.RunBarCube***
- click the run buton on the top, and then click Button to begin the execution


## Bar Disk Experiment
- Open Assets/Prefabs
- load ***Apperatus.prefab*** and ***Disk 1.prefab*** into the scene
- on the inspector window of ***Apperatus.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***barDiskMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: LRCameraLeft GameObject
    - AB Camera: BarDiskUpCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - Bar: Apperatus GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Clisk()
- select **Runtime Only**
- drag ***barDiskMove*** GameObject underneath it
- on the right, select ***BarMover.RunBarDisk***
- click the run buton on the top, and then click Button to begin the execution


## Disk Disk Experiment
- Open Assets/Prefabs
- load ***ApperatusCirc 1.prefab*** and ***Disk 1.prefab*** into the scene
- on the inspector window of ***ApperatusCirc 1.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***DiskDiskMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: DiskDiskCam GameObject
    - AB Camera: DiskDiskUpCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - Bar: ApperatusCirc 1 GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Clisk()
- select **Runtime Only**
- drag ***DiskDiskMove*** GameObject underneath it
- on the right, select ***BarMover.RunDiskDisk***
- click the run buton on the top, and then click Button to begin the execution


## Further Experiments
- ***BarMover.RunExperiments*** may be selected to run other experiments
- Open Assets/Scripts/barMover.cs, and uncomment any experiment you want to run
- You can also self-define a wrapper function and execute by
    - Enclose your own code within:
        ```csharp
        private IEnumerator YourWrapper()
        {
        // Your code goes here
        }
        ```
    - change ***RunExperiments()*** to:
        ```csharp
        public void RunExperiments()
        {
            StartCoroutine(YourWrapper(Your Param));
        }
        ```
    - select ***BarMover.RunExperiments*** in Scene -> Canvas -> Button -> OnClick()
