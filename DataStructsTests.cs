/*
 * May be interesting to implement some additional tests in future:
 
    Note that if you're prepending a lot (as you're essentially doing in the last example) or deleting the first entry, a linked list will nearly always be significantly faster, as there is no searching or moving/copying to do. A List would require moving everything up a spot to accommodate the new item, making prepending an O(N) operation.
    
    So worst case, if we are adding the first (zeroth) element each time, then the blit has to move everything each time.

    count is cached in both list classes. Possibly it's not linear, its constant time.

    The memory footprint after the Clear is considerably different, as the List<T> keeps its size, while the LinkedList does not.

    check clear and RemoveAll speed and compare 
     */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InterestingTestsProject
{
    public class DataStructsTests
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private int _currentTestNumber = 1;

        private string _resultStr = string.Empty;

        private int _maxItemsCount;

        private GcMemTest _memTest = new GcMemTest();

        private void RezNextTest(string testName)
        {
            _resultStr += string.Format("\r\n**********************************************\r\n" +
                                        "Test #{0}: {1}\r\n\r\n", _currentTestNumber++, testName);
        }

        private void RezRangeTime(string subtestName, int rangeMax, TimeSpan elapsed)
        {
            if (subtestName == string.Empty)
            {
                _resultStr += string.Format($"Range: 0....{rangeMax}\r\nTime:{elapsed}\r\n\r\n");
                return;
            }

            _resultStr += string.Format($"SubTest: {subtestName}\r\nRange: 0....{rangeMax}\r\nTime:{elapsed}\r\n\r\n");
        }

        private void RezCalledTimesForElems(string subtestName, int callTimes, int elemsCount)
        {

            if (subtestName == string.Empty)
            {
                _resultStr += string.Format($"Items: {elemsCount}\r\nCalled times: {callTimes}\r\n");
                return;
            }

            _resultStr += string.Format($"SubTest: {subtestName}\r\nItems: {elemsCount}\r\nCalled times: {callTimes}\r\n");
        }
        
        public DataStructsTests(int maxItemsCount = 5600)
        {
            _maxItemsCount = maxItemsCount;
        }

        public string RunTests()
        {
            RunAppendTests();

            RunPrependTests();

            RunInsertTests();

            RunMemUsageSpeedTests();

            RunCountSpeedTests();

            RunContainsSpeedTests();

            RunForeachLoopSpeedTests();

            RunForLoopSpeedTests();

            RunRandomAccessSpeedTests();

            return _resultStr;
        }

        public void RunAppendTests()
        {
            RezNextTest("Fill/Append tests");

            var arrFillTime = FillArr(_maxItemsCount);
            RezRangeTime("Array Fill", _maxItemsCount, arrFillTime);

            var arrAppendTime = AppendArray(_maxItemsCount / 100);
            arrAppendTime = TimeSpan.FromMilliseconds(arrAppendTime.Milliseconds * 100);// lame hack to make it faster
            RezRangeTime("Array Append", _maxItemsCount / 100, arrAppendTime);

            var lstAppendTime = AppendList(_maxItemsCount);
            RezRangeTime("List Append", _maxItemsCount, lstAppendTime);

            var lLstAppendTime = AppendLinkedList(_maxItemsCount);
            RezRangeTime("LinkedList Append", _maxItemsCount, lLstAppendTime);
        }

        public void RunPrependTests()
        {
            RezNextTest("Prepend tests");

            var lstPrependTime = PrependList(_maxItemsCount / 100);
            lstPrependTime = TimeSpan.FromMilliseconds(lstPrependTime.Milliseconds * 100);// lame hack to make it faster
            RezRangeTime("List Prepend", _maxItemsCount, lstPrependTime);

            var lLstPrependTime = PrependLinkedList(_maxItemsCount);
            RezRangeTime("LinkedList Prepend", _maxItemsCount, lLstPrependTime);
        }

        public void RunInsertTests()
        {
            RezNextTest("Insertion tests");

            var lstInsertTime = IsertList(_maxItemsCount);
            RezRangeTime("List", _maxItemsCount, lstInsertTime);

            var lLstInsertTime = InsertLinkedList(_maxItemsCount);
            RezRangeTime("LinkedList", _maxItemsCount, lLstInsertTime);
        }

        public void RunMemUsageSpeedTests()
        {
            RezNextTest("Memory usage tests");

            var elemsCount = 560000;
            var memUsageArr = MemUsageArray(elemsCount);
            var memUsageList = MemUsageList(elemsCount);
            var memUsageLinList = MemUsageLinkedList(elemsCount);

            _resultStr += $"Memory usage for {elemsCount} elements:\r\n" +
                          $"int[]: {memUsageArr} Kb\r\n" +
                          $"List<int>: {memUsageList} Kb\r\n" +
                          $"LinkedList<int>: {memUsageLinList} Kb\r\n";

            elemsCount = 56000000;
            memUsageArr = MemUsageArray(elemsCount);
            memUsageList = MemUsageList(elemsCount);
            memUsageLinList = MemUsageLinkedList(elemsCount);

            _resultStr += $"Memory usage for {elemsCount} elements:\r\n" +
                          $"int[]: {memUsageArr} Kb\r\n" +
                          $"List<int>: {memUsageList} Kb\r\n" +
                          $"LinkedList<int>: {memUsageLinList} Kb\r\n";
            GC.Collect();
        }

        public void RunCountSpeedTests()
        {
            RezNextTest("Count() speed tests");

            var countSpedArr = CountSpeedArray(_maxItemsCount, _maxItemsCount);
            var countSpedList = CountSpeedList(_maxItemsCount, _maxItemsCount);
            var countSpedLinkedList = CountSpeedLinkedList(_maxItemsCount, _maxItemsCount);

            RezCalledTimesForElems("Length/Count speed 1", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n\r\n";

            countSpedArr = CountSpeedArray(_maxItemsCount, _maxItemsCount * 1000);
            countSpedList = CountSpeedList(_maxItemsCount, _maxItemsCount * 1000);
            countSpedLinkedList = CountSpeedLinkedList(_maxItemsCount, _maxItemsCount * 1000);

            RezCalledTimesForElems("Length/Count speed 2", _maxItemsCount * 1000, _maxItemsCount);
            _resultStr += $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n";
        }

        public void RunContainsSpeedTests()
        {
            RezNextTest("Contains() speed tests");
            int divider = 6;//too large time fix

            var containsSpedArr = ContainsSpeedArray(_maxItemsCount / divider, _maxItemsCount / divider);
            var containsSpedList = ContainsSpeedList(_maxItemsCount / divider, _maxItemsCount / divider);
            var containsSpedLinkedList = ContainsSpeedLinkedList(_maxItemsCount / divider, _maxItemsCount / divider);

            RezCalledTimesForElems("", _maxItemsCount / divider, _maxItemsCount / divider);
            _resultStr += $"int[]: {containsSpedArr}\r\n" +
                          $"List<int>: {containsSpedList}\r\n" +
                          $"LinkedList<int>: {containsSpedLinkedList}\r\n\r\n";
        }

        public void RunForeachLoopSpeedTests()
        {
            RezNextTest("Foreach() loop speed tests");

            var foreachSpeedArray = ForeachSpeedArray(_maxItemsCount, _maxItemsCount);
            var foreachSpeedList = ForeachSpeedList(_maxItemsCount, _maxItemsCount);
            var foreachSpeedLinkedList = ForeachSpeedLinkedList(_maxItemsCount, _maxItemsCount);

            RezCalledTimesForElems("Foreach() speed #1", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {foreachSpeedArray}\r\n" +
                          $"List<int>: {foreachSpeedList}\r\n" +
                          $"LinkedList<int>: {foreachSpeedLinkedList}\r\n\r\n";

            foreachSpeedArray = ForeachSpeedArray(_maxItemsCount * 10, _maxItemsCount * 10);
            foreachSpeedList = ForeachSpeedList(_maxItemsCount * 10, _maxItemsCount * 10);
            foreachSpeedLinkedList = ForeachSpeedLinkedList(_maxItemsCount * 10, _maxItemsCount * 10);

            RezCalledTimesForElems("Foreach() speed #2", _maxItemsCount * 10, _maxItemsCount * 10);
            _resultStr += $"int[]: {foreachSpeedArray}\r\n" +
                          $"List<int>: {foreachSpeedList}\r\n" +
                          $"LinkedList<int>: {foreachSpeedLinkedList}\r\n";
        }

        public void RunForLoopSpeedTests()
        {
            RezNextTest("For() loop speed tests");
            var forSpeedArray = ForSpeedArray(_maxItemsCount, _maxItemsCount);
            var forSpeedList = ForSpeedList(_maxItemsCount, _maxItemsCount);

            RezCalledTimesForElems("For() speed #1", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {forSpeedArray}\r\n" +
                          $"List<int>: {forSpeedList}\r\n\r\n";

            forSpeedArray = ForSpeedArray(_maxItemsCount * 10, _maxItemsCount * 10);
            forSpeedList = ForSpeedList(_maxItemsCount * 10, _maxItemsCount * 10);

            RezCalledTimesForElems("For() speed #2", _maxItemsCount * 10, _maxItemsCount * 10);
            _resultStr += $"int[]: {forSpeedArray}\r\n" +
                          $"List<int>: {forSpeedList}\r\n\r\n";

        }

        public void RunRandomAccessSpeedTests()
        {
            RezNextTest("Random access speed tests");
            var raSpeedArray = RandomAccessSpeedArray(_maxItemsCount * 10000, _maxItemsCount * 10000);
            var raSpeedList = RandomAccessSpeedList(_maxItemsCount * 10000, _maxItemsCount * 10000);

            RezCalledTimesForElems("", _maxItemsCount * 10000, _maxItemsCount * 10000);
            _resultStr += $"int[]: {raSpeedArray}\r\n" +
                          $"List<int>: {raSpeedList}\r\n\r\n";
        }


        #region #1 FillAndAppend
        private TimeSpan FillArr(int maxItemsCount)
        {
            _sw.Restart();
                GenerateFilledIntArr(maxItemsCount);
            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan AppendArray(int maxInteger)
        {
            int[] arr = new int[1];

            _sw.Restart();

                for (int i = 0; i < maxInteger; i++)
                {
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[i] = i;
                }

            _sw.Stop();
            
            return _sw.Elapsed;
        }
        
        private TimeSpan AppendList(int maxInteger)
        {
            _sw.Restart();
                GenerateIntList(maxInteger);
            _sw.Stop();
            
            return _sw.Elapsed;
        }
        
        private TimeSpan AppendLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            _sw.Restart();
                GenerateIntLinkedList(maxInteger);
            _sw.Stop();
            
            return _sw.Elapsed;
        }
        #endregion

        #region #2 Prepend

        private TimeSpan PrependList(int maxInteger)
        {
            List<int> Lst = new List<int>();

            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                Lst.Insert(0, i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan PrependLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                lLst.AddFirst(i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

        #region #3 Insertion

        private TimeSpan IsertList(int maxInteger)
        {
            List<int> lst = new List<int>();

            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                lst.Insert(lst.Count/2, i);
                //lst.Insert(2, 2);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan InsertLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            _sw.Restart();

            lLst.AddLast(1);
            lLst.AddLast(2);

            var temp = lLst.Find(2);

            for (int i = 2; i < maxInteger; i++)
            {
                lLst.AddAfter(temp, i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

        #region TODO: Removal Tests

        #endregion

        #region TODO: Ram memory usage

        private long MemUsageArray(int maxItemsCount)
        {
            _memTest.Restart();

            int[] arr = new int[maxItemsCount];
            
            return _memTest.FinishMemTest();
        }

        private long MemUsageList(int maxItemsCount)
        {
            _memTest.Restart();

            List<int> lst = GenerateIntList(maxItemsCount);

            return _memTest.FinishMemTest();
        }

        private long MemUsageLinkedList(int maxItemsCount)
        {
            _memTest.Restart();

            LinkedList<int> lLst = GenerateIntLinkedList(maxItemsCount);
            
            return _memTest.FinishMemTest();
        }
        #endregion
        
        #region TODO: Random access speed


        #endregion

        #region Count; Length speed

        private TimeSpan CountSpeedArray(int itemsCount, int tries)
        {
            int count;
            int[] arr = new int[itemsCount];
            
            _sw.Restart();
            
                for (int i = 0; i < tries; i++)
                {
                    count = arr.Length;
                }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan CountSpeedList(int itemsCount, int tries)
        {
            int count;
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    count = lst.Count;
                }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan CountSpeedLinkedList(int itemsCount, int tries)
        {
            int count;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                count = lLst.Count;
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        #endregion

        #region Contains() speed

        private TimeSpan ContainsSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    arr.Contains(i);
                }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ContainsSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    lst.Contains(i);
                }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ContainsSpeedLinkedList(int itemsCount, int tries)
        {
            int count;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    lLst.Contains(i);
                }

            _sw.Stop();

            return _sw.Elapsed;
        }

        #endregion

        #region Foreach() Speed
        private TimeSpan ForeachSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in arr)
                {

                }
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ForeachSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in lst)
                {

                }
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ForeachSpeedLinkedList(int itemsCount, int tries)
        {
            int count;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in lLst)
                {

                }
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        #endregion

        #region For() Speed
        private TimeSpan ForSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                for (int j=0; j < arr.Length;j++)
                {

                }
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ForSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                for (int j = 0; j < lst.Count; j++)
                {

                }
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

        #region Generate
        private int[] GenerateFilledIntArr(int itemsCount)
        {
            int[] arr = new int[itemsCount];

            for (int i = 0; i < itemsCount; i++)
            {
                arr[i] = i;
            }

            return arr;
        }

        private List<int> GenerateIntList(int itemsCount)
        {
            List<int> lst = new List<int>();

            for (int i = 0; i < itemsCount; i++)
            {
                lst.Add(i);
            }

            return lst;
        }

        private LinkedList<int> GenerateIntLinkedList(int itemsCount)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            for (int i = 0; i < itemsCount; i++)
            {
                lLst.AddLast(i);
            }

            return lLst;
        }
        #endregion

        #region
        private TimeSpan RandomAccessSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);
            int index, tmp;

            Random rnd = new Random();

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                index = rnd.Next(itemsCount);
                tmp = arr[index];
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan RandomAccessSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);
            int index, tmp;

            Random rnd = new Random();

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                index = rnd.Next(itemsCount);
                tmp = lst[index];
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

    }
}
