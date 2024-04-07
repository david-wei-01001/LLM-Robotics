# Reproducing Experiments
- Download and open LLMRoboticsProject in Unity 2022.
- Reset the scene according to the experiment wishing to reproduce


## Bar Cube Experiment
- Open Assets/Prefabs
- load ***Apparatus.prefab*** and ***Target2.prefab*** into the scene
- on the inspector window of ***Apparatus.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***barMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: LRCameraLeft GameObject
    - AB Camera: ABCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - SF Camera: SmartFinalCam
    - Bar: Apparatus GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Click()
- select **Runtime Only**
- drag ***barMove*** GameObject underneath it
- on the right, select ***BarMover.RunBarCube***
- click the run button on the top, and then click the Button to begin the execution


## Bar Disk Experiment
- Open Assets/Prefabs
- load ***Apparatus.prefab*** and ***Disk 1.prefab*** into the scene
- on the inspector window of ***Apparatus.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***barDiskMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: LRCameraLeft GameObject
    - AB Camera: BarDiskUpCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - SF Camera: SmartFinalCam
    - Bar: Apparatus GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Click()
- select **Runtime Only**
- drag ***barDiskMove*** GameObject underneath it
- on the right, select ***BarMover.RunBarDisk***
- click the run button on the top, and then click the Button to begin the execution


## Disk Disk Experiment
- Open Assets/Prefabs
- load ***ApperatusCirc 1.prefab*** and ***Disk 1.prefab*** into the scene
- on the inspector window of ***ApparatusCirc 1.prefab***, add ***BarController.cs*** script as a component
- on the inspector window of ***DiskDiskMove*** GameObject, add ***BarMover.cs*** script as a component
- make sure the following items are assigned correctly
    - Main Camera: Main Camera GameObject
    - LR Camera: DiskDiskCam GameObject
    - AB Camera: DiskDiskUpCamera GameObject
    - LR Camera 1: LRCamera 1 GameObject
    - SF Camera: SmartFinalCam
    - Bar: ApparatusCirc 1 GameObject
- On the inspector window of Scene -> Canvas -> Button, scroll down to On Click()
- select **Runtime Only**
- drag ***DiskDiskMove*** GameObject underneath it
- on the right, select ***BarMover.RunDiskDisk***
- click the run button on the top, and then click the Button to begin the execution


## Further Experiments
- ***BarMover.RunExperiments*** may be selected to run other experiments
- Open Assets/Scripts/barMover.cs, and uncomment any experiment you want to run
- You can also self-define a wrapper function and execute it by
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

 # Left Right Experiment
 You may want to experiment with starting the Apparatus from the left or the right. 
 
 ## Bar Disk Experiment
 Apparatus starting position is:
 - Left: 0.04, 0.64, 0.04
 - Right: 0.18, 0.64, 0.04

## Disk Disk Experiment
 ApparatusCirc 1 starting position is:
 - Left: 0.16, 0.6623, -0.14 
 - Right: 0.3, 0.6623, -0.14
