# C# Tests Project

Here is implemented some interesting tests for me


# C#  Data structures Tests
Testing of Array, List and Linked List of usage for knowing most important things about this data structures and use it more efficient

Most important is results. So you can see most important information here:

![Alt text](http://image.prntscr.com/image/00dd0851e38f4c5a8e477c589fb1e124.png "Colors meaning")

![Alt text](http://image.prntscr.com/image/d0c6a9b775e24b0d92440c66e9a8e93d.png "Array vs List vs Linked List")

If you want more details, I had collect tham here:

![Alt text](http://image.prntscr.com/image/853a2765b1024acea782ec35ef456d75.png "Array vs List vs Linked List")


Interesting to know:
1. Linked List internally is not a List in .NET. ```LinkedList<T>```. It's even does not implement ```IList<T>```. And that's why there are absent indexes and methods related to indexes.

2. ```LinkedList<T>``` is node-pointer based collection. In .NET it's in doubly linked implementation. This means that prior/next elements have link to current element. And data is fragmented -- different list objects can be located in different places of RAM. Also there will be more memory used for ```LinkedList<T>``` than for ```List<T>``` or Array.

3. ```List<T>``` in .Net is Java's alternative of ```ArraList<T>```. This means that this is array wrapper. So it's allocated in menory as one contiguous block of data. If allocated data size exceeds 85000 bytes, it will be allocated iside of the Large Object Heap. Depending on the size, this can lead to heap fragmentation, a mild form of memory leak. But in the same time if size < 85000 bytes -- this provides a very compact and fast-access representation in memory. 

4. Single contiguous block is preferred for random access performance and memory consumption but for collections that need to change size regularly a structure such as an Array generally need to be copied to a new location whereas a linked list only needs to manage the memory for the newly inserted/deleted nodes. 


# C#  Async Await Tests

Just simple example of usage Async Await methods

Tests result sample in "clean" view:
```
Test#0: Array Fill
Range: 0....5600
Time:00:00:00.0001218

Test#1: Array Append
Range: 0....5600
Time:00:00:00

Test#2: List Append
Range: 0....5600
Time:00:00:00.0001922

Test#3: LinkedList Append
Range: 0....5600
Time:00:00:00.0005505

**********************************************

Test#4: List Prepend
Range: 0....5600
Time:00:00:00

Test#5: LinkedList Prepend
Range: 0....5600
Time:00:00:00.0003995

**********************************************

Test#6: List Insertion
Range: 0....5600
Time:00:00:00.0042980

Test#7: LinkedList Insertion
Range: 0....5600
Time:00:00:00.0008088

**********************************************

Memory usage for 560000 elements:
int[]: 2286 Kb
List<int>: 4195 Kb
LinkedList<int>: 13224 Kb
Memory usage for 56000000 elements:
int[]: 218851 Kb
List<int>: 262245 Kb
LinkedList<int>: 1312603 Kb
**********************************************

Length/Count speed called 5600 times for 5600 elements:
int[]: 00:00:00.0000158
List<int>: 00:00:00.0000156
LinkedList<int>: 00:00:00.0000729

Length/Count speed called 5600000 times for 5600 elements:
int[]: 00:00:00.0158565
List<int>: 00:00:00.0157866
LinkedList<int>: 00:00:00.0165044
**********************************************

Contains speed called 933 times for 933 elements:
int[]: 00:00:00.0008321
List<int>: 00:00:00.0014756
LinkedList<int>: 00:00:00.0015360

```