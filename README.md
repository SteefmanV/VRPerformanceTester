# Description #
This project is used as a tool to measure the performance of different object setups in Virtual Reality. 
Tests are done by spawning different grids of different mesh types and measuring their frame/second performance over time. 
The goal of the tests is to analyze the performance of the number of meshes (draw calls) and the number of triangles in a mesh. 

# Create a new Test #
1. To create a test right-click in the folder where you want to place the test
2. Click in the top of the popup on PerformanceTest
3. In the new PerformanceTest object there are different parameters to setup:
 - Test Name - choose a unique name for your test
 - Test Description - A description of the test
 - Test Duration Seconds - number of seconds to perform the test
 - Type - the mesh size
 - Grid Size - The grid size of the objects to test. x15, y15, z15 will spawn a 15x15x15 grid of the mesh type selected above. 

# Add Tests #
After creating a test as explained above, the test can be added to the test queue. 
In the main "Benchmark" scene there's the object "Test Runner". In the TestRunner component inside of the object, multiple tests can be added by placing them in the TestQueue list. 

# Performing a test sequence #
When the application is started the TestRunner will iterate through the TestQueue and will perform the test of each test in there. When the test is done an Excel report will be exported to the main folder of the application. (when the test is done inside Unity it will be exported in the project folder). 

The Excel report contains a summary of all the tests and an in-depth report of each separate test. 

Tip to get the best reliable results: make a build of the project and run it without any other applications running in the background (including Unity). 

# Support #
Unity Version: 2019.3.5f1
The project is only tested with the Oculus Rift. But with small adjustments it should also be usable with other Oculus devices and the HTC Vive.
