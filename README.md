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

![alt tag](https://i.stack.imgur.com/iBz6V.png)


**Interesting to know:**
1. Linked List internally is not a List in .NET. ```LinkedList<T>```. It's even does not implement ```IList<T>```. And that's why there are absent indexes and methods related to indexes.

2. ```LinkedList<T>``` is node-pointer based collection. In .NET it's in doubly linked implementation. This means that prior/next elements have link to current element. And data is fragmented -- different list objects can be located in different places of RAM. Also there will be more memory used for ```LinkedList<T>``` than for ```List<T>``` or Array.

3. ```List<T>``` in .Net is Java's alternative of ```ArraList<T>```. This means that this is array wrapper. So it's allocated in menory as one contiguous block of data. If allocated data size exceeds 85000 bytes, it will be allocated iside of the Large Object Heap. Depending on the size, this can lead to heap fragmentation, a mild form of memory leak. But in the same time if size < 85000 bytes -- this provides a very compact and fast-access representation in memory. 

4. Single contiguous block is preferred for random access performance and memory consumption but for collections that need to change size regularly a structure such as an Array generally need to be copied to a new location whereas a linked list only needs to manage the memory for the newly inserted/deleted nodes. 


Tests result sample in "clean" view:
```

****************************************
Test #1: Fill/Append tests

SubTest: Array Fill
Range: 0....5600000
Time:00:00:00.0113683

SubTest: Array Append
Range: 0....56000
Time:00:00:19.3669000

SubTest: List Append
Range: 0....5600000
Time:00:00:00.0470090

SubTest: LinkedList Append
Range: 0....5600000
Time:00:00:00.8614091

****************************************
Test #2: Prepend tests

SubTest: List Prepend
Range: 0....5600
Time:00:00:23.3610285

SubTest: LinkedList Prepend
Range: 0....5600
Time:00:00:00.0114274

****************************************
Test #3: Insertion tests

SubTest: List
Range: 0....560000
Time:00:00:15.6868562

SubTest: LinkedList
Range: 0....560000
Time:00:00:00.0648320

****************************************
Test #4: Memory usage tests

Memory usage for 560000 elements:
int[]: 2188 Kb (Expected: 2240~ Kb)
List<int>: 4100 Kb
LinkedList<int>: 26246 Kb

Memory usage for 56000000 elements:
int[]: 218750 Kb (Expected: 224000~ Kb)
List<int>: 262144 Kb
LinkedList<int>: 2625000 Kb

****************************************
Test #5: Count() speed tests

SubTest: Length/Count speed 1
Items: 56000
Called times: 56000000
int[]: 00:00:00.0277196
List<int>: 00:00:00.0280081
LinkedList<int>: 00:00:00.0419579

SubTest: Length/Count speed 2
Items: 56000
Called times: 2147483647
int[]: 00:00:01.0658961
List<int>: 00:00:01.0682234
LinkedList<int>: 00:00:01.6018209

****************************************
Test #6: Contains() speed tests

Items: 56000
Called times: 56000
int[]: 00:00:01.5704624
List<int>: 00:00:03.5993226
LinkedList<int>: 00:00:12.3584247

****************************************
Test #7: Foreach() loop speed tests

SubTest: Foreach() speed #1
Items: 56000
Called times: 5600
int[]: 00:00:00.1564547
List<int>: 00:00:00.9241910
LinkedList<int>: 00:00:02.1726847

SubTest: Foreach() speed #2
Items: 56000
Called times: 56000
int[]: 00:00:01.5637290
List<int>: 00:00:09.2038910
LinkedList<int>: 00:00:20.0895703

****************************************
Test #8: For() loop speed tests

SubTest: For() speed #1
Items: 56000
Called times: 5600
int[]: 00:00:00.1562709
List<int>: 00:00:00.3886933

SubTest: For() speed #2
Items: 56000
Called times: 56000
int[]: 00:00:01.5780412
List<int>: 00:00:03.8989484

****************************************
Test #9: Random access speed tests

Items: 56000000
Called times: 56000000
int[]: 00:00:04.1690581
List<int>: 00:00:04.1938929


```

# C#  Async-Await Tests

Just simple example of usage Async Await methods
