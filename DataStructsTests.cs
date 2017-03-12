using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InterestingTestsProject
{
    public class DataStructsTests
    {
        private string _testRezTemplate = "Test#{0}: {1}\r\nRange: {2}....{3}\r\nTime:{4}\r\n\r\n";
        
        private readonly Stopwatch Sw = new Stopwatch();
        
        private int _currentTestNumber;

        private string _resultStr = string.Empty;

        private int _maxItemsCount;

        private GcMemTest _memTest = new GcMemTest();

        private void UpdateRezStr(string testName, int rangeMax, TimeSpan elapsed)
        {
            _resultStr += string.Format(_testRezTemplate, _currentTestNumber++, testName, 0, rangeMax, elapsed);
        }

        private void AddSeparatorLine()
        {
            _resultStr += "**********************************************\r\n\r\n";
        }

        public DataStructsTests(int maxItemsCount = 5600)
        {
            _maxItemsCount = maxItemsCount;
        }

        public string RunTests()
        {
            // TestSet 1: FilAndAppend
            var arrFillTime = FillArr(_maxItemsCount);
            UpdateRezStr("Array Fill", _maxItemsCount, arrFillTime);

            var arrAppendTime = AppendArray(_maxItemsCount / 100);
            arrAppendTime = TimeSpan.FromMilliseconds(arrAppendTime.Milliseconds * 100);// lame hack to make it faster
            UpdateRezStr("Array Append", _maxItemsCount, arrAppendTime);

            var lstAppendTime = AppendList(_maxItemsCount);
            UpdateRezStr("List Append", _maxItemsCount, lstAppendTime);

            var lLstAppendTime = AppendLinkedList(_maxItemsCount);
            UpdateRezStr("LinkedList Append", _maxItemsCount, lLstAppendTime);

            AddSeparatorLine();


            // TestSet 2: Prepend
            var lstPrependTime = PrependList(_maxItemsCount / 100);
            lstPrependTime = TimeSpan.FromMilliseconds(lstPrependTime.Milliseconds * 100);// lame hack to make it faster
            UpdateRezStr("List Prepend", _maxItemsCount, lstPrependTime);

            var lLstPrependTime = PrependLinkedList(_maxItemsCount);
            UpdateRezStr("LinkedList Prepend", _maxItemsCount, lLstPrependTime);

            AddSeparatorLine();


            // TestSet 3: Insertion
            var lstInsertTime = IsertList(_maxItemsCount);
            UpdateRezStr("List Insertion", _maxItemsCount, lstInsertTime);

            var lLstInsertTime = InsertLinkedList(_maxItemsCount);
            UpdateRezStr("LinkedList Insertion", _maxItemsCount, lLstInsertTime);

            AddSeparatorLine();


            //TestSet 3: Insertion

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
            AddSeparatorLine();
            GC.Collect();
            

            //TestSet 5: Count speed
            var countSpedArr = CountSpeedArray(_maxItemsCount, _maxItemsCount);
            var countSpedList = CountSpeedList(_maxItemsCount, _maxItemsCount);
            var countSpedLinkedList = CountSpeedLinkedList(_maxItemsCount, _maxItemsCount);

            _resultStr += $"Length/Count speed called {_maxItemsCount} times for {_maxItemsCount} elements:\r\n" +
                          $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n\r\n";

            countSpedArr = CountSpeedArray(_maxItemsCount, _maxItemsCount*1000);
            countSpedList = CountSpeedList(_maxItemsCount, _maxItemsCount * 1000);
            countSpedLinkedList = CountSpeedLinkedList(_maxItemsCount, _maxItemsCount * 1000);

            _resultStr += $"Length/Count speed called {_maxItemsCount * 1000} times for {_maxItemsCount} elements:\r\n" +
                          $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n";
            
            AddSeparatorLine();
            

            //TestSet 6: Contains speed
            int divider = 6;//too large time fix

            var containsSpedArr = ContainsSpeedArray(_maxItemsCount/ divider, _maxItemsCount / divider);
            var containsSpedList = ContainsSpeedList(_maxItemsCount / divider, _maxItemsCount / divider);
            var containsSpedLinkedList = ContainsSpeedLinkedList(_maxItemsCount / divider, _maxItemsCount / divider);

            _resultStr += $"Contains speed called {_maxItemsCount / divider} times for {_maxItemsCount / divider} elements:\r\n" +
                          $"int[]: {containsSpedArr}\r\n" +
                          $"List<int>: {containsSpedList}\r\n" +
                          $"LinkedList<int>: {containsSpedLinkedList}\r\n\r\n";

            //containsSpedArr = ContainsSpeedArray(_maxItemsCount, _maxItemsCount * 1000);
            //containsSpedList = ContainsSpeedList(_maxItemsCount, _maxItemsCount * 1000);
            //containsSpedLinkedList = ContainsSpeedLinkedList(_maxItemsCount, _maxItemsCount * 1000);

            //_resultStr += $"Length/Count speed called {_maxItemsCount * 1000} times for {_maxItemsCount} elements:\r\n" +
            //              $"int[]: {containsSpedArr}\r\n" +
            //              $"List<int>: {containsSpedList}\r\n" +
            //              $"LinkedList<int>: {containsSpedLinkedList}\r\n";

            AddSeparatorLine();


            return _resultStr;
        }

        #region #1 FillAndAppend
        private TimeSpan FillArr(int maxItemsCount)
        {
            Sw.Restart();
                GenerateFilledIntArr(maxItemsCount);
            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan AppendArray(int maxInteger)
        {
            int[] arr = new int[1];

            Sw.Restart();

                for (int i = 0; i < maxInteger; i++)
                {
                    Array.Resize(ref arr, arr.Length + 1);
                    arr[i] = i;
                }

            Sw.Stop();
            
            return Sw.Elapsed;
        }
        
        private TimeSpan AppendList(int maxInteger)
        {
            Sw.Restart();
                GenerateIntList(maxInteger);
            Sw.Stop();
            
            return Sw.Elapsed;
        }
        
        private TimeSpan AppendLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            Sw.Restart();
                GenerateIntLinkedList(maxInteger);
            Sw.Stop();
            
            return Sw.Elapsed;
        }
        #endregion

        #region #2 Prepend

        private TimeSpan PrependList(int maxInteger)
        {
            List<int> Lst = new List<int>();

            Sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                Lst.Insert(0, i);
            }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan PrependLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            Sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                lLst.AddFirst(i);
            }

            Sw.Stop();

            return Sw.Elapsed;
        }
        #endregion

        #region #3 Insertion

        private TimeSpan IsertList(int maxInteger)
        {
            List<int> lst = new List<int>();

            Sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                lst.Insert(lst.Count/2, i);
                //lst.Insert(2, 2);
            }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan InsertLinkedList(int maxInteger)
        {
            LinkedList<int> lLst = new LinkedList<int>();

            Sw.Restart();

            lLst.AddLast(1);
            lLst.AddLast(2);

            var temp = lLst.Find(2);

            for (int i = 2; i < maxInteger; i++)
            {
                lLst.AddAfter(temp, i);
            }

            Sw.Stop();

            return Sw.Elapsed;
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
            
            Sw.Restart();
            
                for (int i = 0; i < tries; i++)
                {
                    count = arr.Length;
                }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan CountSpeedList(int itemsCount, int tries)
        {
            int count;
            List<int> lst = GenerateIntList(itemsCount);

            Sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    count = lst.Count;
                }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan CountSpeedLinkedList(int itemsCount, int tries)
        {
            int count;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            Sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                count = lLst.Count;
            }

            Sw.Stop();

            return Sw.Elapsed;
        }

        #endregion

        #region Contains() speed

        private TimeSpan ContainsSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);

            Sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    arr.Contains(i);
                }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan ContainsSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);

            Sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    lst.Contains(i);
                }

            Sw.Stop();

            return Sw.Elapsed;
        }

        private TimeSpan ContainsSpeedLinkedList(int itemsCount, int tries)
        {
            int count;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            Sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    lLst.Contains(i);
                }

            Sw.Stop();

            return Sw.Elapsed;
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

    }
}
