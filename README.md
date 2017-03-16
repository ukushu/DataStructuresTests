# C# Tests Project

Here is implemented some interesting tests for me


# C#  Data structures Tests
Testing of Array, List and Linked List of usage for knowing most important things about this data structures and use it more efficient

**Short results:**

* Array need to use:
  * So often as possible. It's fast and takes smallest RAM range for same amount information.
  * If you know exact count of cells needed
  * If data saved in array < 85000 b
  * If needed high Random Access speed

* List need to use:
  * If needed to add cells to the end of list (often)
  * If needed to add cells in the beginning/middle of the list (NOT OFTEN)
  * If data saved in array < 85000 b
  * If needed high Random Access speed

* LinkedList need to use:
  * If needed to add cells in the beginning/middle/end of the list (often)
  * If needed only sequential access (forward/backward)
  * If you need to save LARGE items, but items count is low.
  * Better do not use for large amount of items, as it's use additional memory for links.


**Detailed results:**

![Alt text](http://image.prntscr.com/image/00dd0851e38f4c5a8e477c589fb1e124.png "Colors meaning")

![Alt text](https://i.stack.imgur.com/WCllu.png "Array vs List vs Linked List")

If you want more details from tests:

![Alt text](https://i.stack.imgur.com/S2KVI.png "Array vs List vs Linked List")


**Interesting to know:**
1. Linked List internally is not a List in .NET. ```LinkedList<T>```. It's even does not implement ```IList<T>```. And that's why there are absent indexes and methods related to indexes.

2. ```LinkedList<T>``` is node-pointer based collection. In .NET it's in doubly linked implementation. This means that prior/next elements have link to current element. And data is fragmented -- different list objects can be located in different places of RAM. Also there will be more memory used for ```LinkedList<T>``` than for ```List<T>``` or Array.

3. ```List<T>``` in .Net is Java's alternative of ```ArraList<T>```. This means that this is array wrapper. So it's allocated in menory as one contiguous block of data. If allocated data size exceeds 85000 bytes, it will be allocated iside of the Large Object Heap. Depending on the size, this can lead to heap fragmentation, a mild form of memory leak. But in the same time if size < 85000 bytes -- this provides a very compact and fast-access representation in memory. 

4. Single contiguous block is preferred for random access performance and memory consumption but for collections that need to change size regularly a structure such as an Array generally need to be copied to a new location whereas a linked list only needs to manage the memory for the newly inserted/deleted nodes. 


Tests result sample in "clean" view:
```

**********************************************
Test #1: Fill/Append tests

SubTest: Array Fill
Range: 0....5600
Time:00:00:00.0005582

SubTest: Array Append
Range: 0....56
Time:00:00:00

SubTest: List Append
Range: 0....5600
Time:00:00:00.0003435

SubTest: LinkedList Append
Range: 0....5600
Time:00:00:00.0010172


**********************************************
Test #2: Prepend tests

SubTest: List Prepend
Range: 0....5600
Time:00:00:00

SubTest: LinkedList Prepend
Range: 0....5600
Time:00:00:00.0006906


**********************************************
Test #3: Insertion tests

SubTest: List
Range: 0....5600
Time:00:00:00.0077296

SubTest: LinkedList
Range: 0....5600
Time:00:00:00.0012719


**********************************************
Test #4: Memory usage tests

Memory usage for 560000 elements:
int[]: 2286 Kb
List<int>: 4195 Kb
LinkedList<int>: 13224 Kb
Memory usage for 56000000 elements:
int[]: 218849 Kb
List<int>: 262243 Kb
LinkedList<int>: 1312601 Kb

**********************************************
Test #5: Count() speed tests

SubTest: Length/Count speed 1
Items: 5600
Called times: 5600
int[]: 00:00:00.0000156
List<int>: 00:00:00.0000163
LinkedList<int>: 00:00:00.0000583

SubTest: Length/Count speed 2
Items: 5600
Called times: 5600000
int[]: 00:00:00.0166288
List<int>: 00:00:00.0164550
LinkedList<int>: 00:00:00.0171840

**********************************************
Test #6: Contains() speed tests

Items: 933
Called times: 933
int[]: 00:00:00.0008534
List<int>: 00:00:00.0033693
LinkedList<int>: 00:00:00.0014628


**********************************************
Test #7: Foreach() loop speed tests

SubTest: Foreach() speed #1
Items: 5600
Called times: 5600
int[]: 00:00:00.0780322
List<int>: 00:00:00.1570124
LinkedList<int>: 00:00:00.2306882

SubTest: Foreach() speed #2
Items: 56000
Called times: 56000
int[]: 00:00:07.8914087
List<int>: 00:00:15.4580335
LinkedList<int>: 00:00:23.3689500

**********************************************
Test #8: For() loop speed tests

SubTest: For() speed #1
Items: 5600
Called times: 5600
int[]: 00:00:00.0819466
List<int>: 00:00:00.1085861

SubTest: For() speed #2
Items: 56000
Called times: 56000
int[]: 00:00:08.2735954
List<int>: 00:00:10.7827043


**********************************************
Test #9: Random access speed tests

Items: 56000000
Called times: 56000000
int[]: 00:00:06.2716167
List<int>: 00:00:06.5015990


```

# C#  Async-Await Tests

Just simple example of usage Async Await methods
