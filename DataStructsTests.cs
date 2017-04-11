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
using System.Windows.Forms;

namespace InterestingTestsProject
{
    public class DataStructsTests
    {
        private readonly Stopwatch _sw = new Stopwatch();

        private int _currentTestNumber = 1;

        private string _resultStr = string.Empty;

        private int _maxItemsCount;

        private GcMemTest _memTest = new GcMemTest();

        //This block must to be global for more correct tests
        int[] _arr;
        List<int> _lst;
        LinkedList<int> _lLst; 

        private void RezNextTest(string testName)
        {
            _resultStr += "\r\n****************************************\r\n" +
                         $"Test #{_currentTestNumber++}: {testName}\r\n\r\n";
        }

        private void RezRangeTime(string subtestName, int rangeMax, TimeSpan elapsed)
        {
            if (subtestName == string.Empty)
            {
                _resultStr += $"Range: 0....{rangeMax}\r\nTime:{elapsed}\r\n\r\n";
                return;
            }

            _resultStr += $"SubTest: {subtestName}\r\nRange: 0....{rangeMax}\r\nTime:{elapsed}\r\n\r\n";
        }

        private void RezItemsAndCallTimes(string subtestName, int itemsCount, long callTimes)
        {

            if (subtestName == string.Empty)
            {
                _resultStr += $"Items: {itemsCount}\r\nCalled times: {callTimes}\r\n";
                return;
            }

            _resultStr += $"SubTest: {subtestName}\r\nItems: {itemsCount}\r\nCalled times: {callTimes}\r\n";
        }
        
        public DataStructsTests(int maxItemsCount = 56000)
        {
            _maxItemsCount = maxItemsCount;
        }

        public string RunTests()
        {
#if DEBUG
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to run this tests in Debug mode? " +
                                                        "(Tests will be not enough correct in this case - in 1,5-3 times slower)", 
                                                        "Sure?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
            {
                return string.Empty;
            }
#endif

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
            
            var maxItemsCount = _maxItemsCount * 100;
            
            var arrFillTime = FillArr(maxItemsCount);
            RezRangeTime("Array Fill", maxItemsCount, arrFillTime);

            int lameHackToMakeFaster = (maxItemsCount > 1000) ? 1000 : 1;

            var arrAppendTime = AppendArray(maxItemsCount / lameHackToMakeFaster);
            arrAppendTime = TimeSpan.FromTicks(arrAppendTime.Ticks * lameHackToMakeFaster);
            RezRangeTime("Array Append", maxItemsCount / 100, arrAppendTime);

            var lstAppendTime = AppendList(maxItemsCount);
            RezRangeTime("List Append", maxItemsCount, lstAppendTime);

            var lLstAppendTime = AppendLinkedList(maxItemsCount);
            RezRangeTime("LinkedList Append", maxItemsCount, lLstAppendTime);
        }

        public void RunPrependTests()
        {
            RezNextTest("Prepend tests");
            
            var maxItemsCount = _maxItemsCount/10;

            var lstPrependTime = PrependList(maxItemsCount);
            RezRangeTime("List Prepend", maxItemsCount, lstPrependTime);

            var lLstPrependTime = PrependLinkedList(maxItemsCount);
            RezRangeTime("LinkedList Prepend", maxItemsCount, lLstPrependTime);
        }

        public void RunInsertTests()
        {
            RezNextTest("Insertion tests");

            var maxItemsCount = _maxItemsCount * 10;

            var lstInsertTime = IsertList(maxItemsCount);
            RezRangeTime("List", maxItemsCount, lstInsertTime);

            var lLstInsertTime = InsertLinkedList(maxItemsCount);
            RezRangeTime("LinkedList", maxItemsCount, lLstInsertTime);
        }

        public void RunMemUsageSpeedTests()
        {
            var int32ByteSize = 4;

            CleanGenerics();

            RezNextTest("Memory usage tests");
            
            var elemsCount = _maxItemsCount * 10;

            var memUsageArr = MemUsageArray(elemsCount);
            
            var memUsageList = MemUsageList(elemsCount);
            
            var memUsageLinList = MemUsageLinkedList(elemsCount);
            _lLst = new LinkedList<int>();

            _resultStr += $"Memory usage for {elemsCount} elements:\r\n" +
                          $"int[]: {memUsageArr} Kb (Expected: {int32ByteSize * elemsCount/ 1000}~ Kb)\r\n" +
                          $"List<int>: {memUsageList} Kb\r\n" +
                          $"LinkedList<int>: {memUsageLinList} Kb\r\n\r\n";



            elemsCount = elemsCount * 100;

            memUsageArr = MemUsageArray(elemsCount);
            
            memUsageList = MemUsageList(elemsCount);
            
            memUsageLinList = MemUsageLinkedList(elemsCount);
            
            _resultStr += $"Memory usage for {elemsCount} elements:\r\n" +
                          $"int[]: {memUsageArr} Kb (Expected: {int32ByteSize * elemsCount / 1000}~ Kb)\r\n" +
                          $"List<int>: {memUsageList} Kb\r\n" +
                          $"LinkedList<int>: {memUsageLinList} Kb\r\n";
        }

        

        public void RunCountSpeedTests()
        {
            RezNextTest("Count() speed tests");
            
            var items = _maxItemsCount;
            long tries = _maxItemsCount * 1000;
            
            var countSpedArr = CountSpeedArray(items, tries);
            var countSpedList = CountSpeedList(items, tries);
            var countSpedLinkedList = CountSpeedLinkedList(items, tries);

            RezItemsAndCallTimes("Length/Count speed 1", items, tries);
            _resultStr += $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n\r\n";

            tries = _maxItemsCount * 1000000;
            countSpedArr = CountSpeedArray(items, Int32.MaxValue);
            countSpedList = CountSpeedList(items, Int32.MaxValue);
            countSpedLinkedList = CountSpeedLinkedList(items, Int32.MaxValue);

            RezItemsAndCallTimes("Length/Count speed 2", items, Int32.MaxValue);
            _resultStr += $"int[]: {countSpedArr}\r\n" +
                          $"List<int>: {countSpedList}\r\n" +
                          $"LinkedList<int>: {countSpedLinkedList}\r\n";
        }

        public void RunContainsSpeedTests()
        {
            RezNextTest("Contains() speed tests");
            //int divider = 6;//too large time fix

            var containsSpedArr = ContainsSpeedArray(_maxItemsCount, _maxItemsCount);
            var containsSpedList = ContainsSpeedList(_maxItemsCount, _maxItemsCount);
            var containsSpedLinkedList = ContainsSpeedLinkedList(_maxItemsCount, _maxItemsCount);

            RezItemsAndCallTimes("", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {containsSpedArr}\r\n" +
                          $"List<int>: {containsSpedList}\r\n" +
                          $"LinkedList<int>: {containsSpedLinkedList}\r\n\r\n";
        }

        public void RunForeachLoopSpeedTests()
        {
            RezNextTest("Foreach() loop speed tests");

            var foreachSpeedArray = ForeachSpeedArray(_maxItemsCount, _maxItemsCount/10);
            var foreachSpeedList = ForeachSpeedList(_maxItemsCount, _maxItemsCount / 10);
            var foreachSpeedLinkedList = ForeachSpeedLinkedList(_maxItemsCount, _maxItemsCount / 10);

            RezItemsAndCallTimes("Foreach() speed #1", _maxItemsCount, _maxItemsCount / 10);
            _resultStr += $"int[]: {foreachSpeedArray}\r\n" +
                          $"List<int>: {foreachSpeedList}\r\n" +
                          $"LinkedList<int>: {foreachSpeedLinkedList}\r\n\r\n";

            foreachSpeedArray = ForeachSpeedArray(_maxItemsCount, _maxItemsCount);
            foreachSpeedList = ForeachSpeedList(_maxItemsCount, _maxItemsCount);
            foreachSpeedLinkedList = ForeachSpeedLinkedList(_maxItemsCount, _maxItemsCount);

            RezItemsAndCallTimes("Foreach() speed #2", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {foreachSpeedArray}\r\n" +
                          $"List<int>: {foreachSpeedList}\r\n" +
                          $"LinkedList<int>: {foreachSpeedLinkedList}\r\n";
        }

        public void RunForLoopSpeedTests()
        {
            RezNextTest("For() loop speed tests");
            var forSpeedArray = ForSpeedArray(_maxItemsCount, _maxItemsCount / 10);
            var forSpeedList = ForSpeedList(_maxItemsCount, _maxItemsCount / 10);

            RezItemsAndCallTimes("For() speed #1", _maxItemsCount, _maxItemsCount / 10);
            _resultStr += $"int[]: {forSpeedArray}\r\n" +
                          $"List<int>: {forSpeedList}\r\n\r\n";

            forSpeedArray = ForSpeedArray(_maxItemsCount, _maxItemsCount);
            forSpeedList = ForSpeedList(_maxItemsCount, _maxItemsCount);

            RezItemsAndCallTimes("For() speed #2", _maxItemsCount, _maxItemsCount);
            _resultStr += $"int[]: {forSpeedArray}\r\n" +
                          $"List<int>: {forSpeedList}\r\n\r\n";

        }

        public void RunRandomAccessSpeedTests()
        {
            RezNextTest("Random access speed tests");
            var raSpeedArray = RandomAccessSpeedArray(_maxItemsCount * 1000, _maxItemsCount * 1000);
            var raSpeedList = RandomAccessSpeedList(_maxItemsCount * 1000, _maxItemsCount * 1000);

            RezItemsAndCallTimes("", _maxItemsCount * 1000, _maxItemsCount * 1000);
            _resultStr += $"int[]: {raSpeedArray}\r\n" +
                          $"List<int>: {raSpeedList}\r\n\r\n";
        }
        
        #region #1 FillAndAppend
        private TimeSpan FillArr(int maxItemsCount)
        {
            _sw.Restart();
                _arr = GenerateFilledIntArr(maxItemsCount);
            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan AppendArray(int maxInteger)
        {
            _arr = new int[1];

            _sw.Restart();

                for (int i = 0; i < maxInteger; i++)
                {
                    Array.Resize(ref _arr, _arr.Length + 1);
                    _arr[i] = i;
                }

            _sw.Stop();
            
            return _sw.Elapsed;
        }
        
        private TimeSpan AppendList(int maxInteger)
        {
            _sw.Restart();
                _lst = GenerateIntList(maxInteger);
            _sw.Stop();
            
            return _sw.Elapsed;
        }
        
        private TimeSpan AppendLinkedList(int maxInteger)
        {
            _sw.Restart();
                _lLst = GenerateIntLinkedList(maxInteger);
            _sw.Stop();
            
            return _sw.Elapsed;
        }
        #endregion

        #region #2 Prepend

        private TimeSpan PrependList(int maxInteger)
        {
            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                _lst.Insert(0, i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan PrependLinkedList(int maxInteger)
        {
            _lLst = new LinkedList<int>();

            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                _lLst.AddFirst(i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

        #region #3 Insertion

        private TimeSpan IsertList(int maxInteger)
        {
            _lst = new List<int>();

            _sw.Restart();

            for (int i = 0; i < maxInteger; i++)
            {
                _lst.Insert(_lst.Count/2, i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan InsertLinkedList(int maxInteger)
        {
            _lLst = new LinkedList<int>();

            _sw.Restart();

            _lLst.AddLast(1);
            _lLst.AddLast(2);

            var tmp = _lLst.Find(2);

            for (int i = 2; i < maxInteger; i++)
            {
                _lLst.AddAfter(tmp, i);
            }

            _sw.Stop();

            return _sw.Elapsed;
        }
        #endregion

        #region TODO: Removal Tests

        #endregion

        #region TODO: Ram memory usage

        private void CleanGenerics()
        {
            _arr = null;
            _lst = null;
            _lLst = null;

            GC.Collect();
        }

        private long MemUsageArray(int maxItemsCount)
        {
            _arr = new int[0];

            _memTest.Restart();

            _arr = new int[maxItemsCount];

            var result = _memTest.FinishMemTest();

            CleanGenerics();

            return result;
        }

        private long MemUsageList(int maxItemsCount)
        {
            _lst = new List<int>(0);

            _memTest.Restart();

            _lst = GenerateIntList(maxItemsCount);

            var result = _memTest.FinishMemTest();

            CleanGenerics();

            return result;
        }

        private long MemUsageLinkedList(int maxItemsCount)
        {
            _lLst = new LinkedList<int>();

            _memTest.Restart();

            _lLst = GenerateIntLinkedList(maxItemsCount);

            var result = _memTest.FinishMemTest();

            CleanGenerics();

            return result;
        }
        #endregion
        
        #region Count; Length speed

        private TimeSpan CountSpeedArray(int itemsCount, long tries)
        {
            int tmp = 0;
            int[] arr = new int[itemsCount];
            
            _sw.Restart();
            
                for (long i = 0; i < tries; i++)
                {
                    tmp = arr.Length;
                }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan CountSpeedList(int itemsCount, long tries)
        {
            int tmp = 0;
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    tmp = lst.Count;
                }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan CountSpeedLinkedList(int itemsCount, long tries)
        {
            int tmp = 0;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                tmp = lLst.Count;
            }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        #endregion

        #region Contains() speed

        private TimeSpan ContainsSpeedArray(int itemsCount, int tries)
        {
            bool tmp = false;
            int[] arr = GenerateFilledIntArr(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    tmp = arr.Contains(i);
                }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan ContainsSpeedList(int itemsCount, int tries)
        {
            bool tmp = false;
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    tmp = lst.Contains(i);
                }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan ContainsSpeedLinkedList(int itemsCount, int tries)
        {
            bool tmp = false;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

                for (int i = 0; i < tries; i++)
                {
                    tmp = lLst.Contains(i);
                }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        #endregion

        #region Foreach() Speed
        private TimeSpan ForeachSpeedArray(int itemsCount, int tries)
        {
            int tmp = 0;
            int[] arr = GenerateFilledIntArr(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in arr)
                {
                    tmp = i1;
                }
            }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan ForeachSpeedList(int itemsCount, int tries)
        {
            int tmp = 0;
            List<int> lst = GenerateIntList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in lst)
                {
                    tmp = i1;
                }
            }

            Console.Write(tmp);

            _sw.Stop();

            return _sw.Elapsed;
        }

        private TimeSpan ForeachSpeedLinkedList(int itemsCount, int tries)
        {
            int tmp = 0;
            LinkedList<int> lLst = GenerateIntLinkedList(itemsCount);

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                foreach (var i1 in lLst)
                {
                    tmp = i1;
                }
            }

            Console.Write(tmp);

            _sw.Stop();

            return _sw.Elapsed;
        }

        #endregion

        #region For() Speed
        private TimeSpan ForSpeedArray(int itemsCount, int tries)
        {
            int[] arr = GenerateFilledIntArr(itemsCount);
            int tmp = 1;

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                for (int j=0; j < arr.Length;j++)
                {
                    tmp = arr[j];
                }
            }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }

        private TimeSpan ForSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);
            int tmp = 0;

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                for (int j = 0; j < lst.Count; j++)
                {
                    tmp = lst[j];
                }
            }

            _sw.Stop();

            Console.Write(tmp);

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

        #region RandomAccessSpeed
        private TimeSpan RandomAccessSpeedArray(int itemsCount, int tries)
        {
            _arr = GenerateFilledIntArr(itemsCount);
            int index, tmp = 0;

            Random rnd = new Random();

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                index = rnd.Next(itemsCount);
                tmp = _arr[index];
            }
            _sw.Stop();

            Console.Write(tmp);
            //need to use this value for being sure that release optimizator will not cut the code

            return _sw.Elapsed;
        }

        private TimeSpan RandomAccessSpeedList(int itemsCount, int tries)
        {
            List<int> lst = GenerateIntList(itemsCount);
            int index, tmp = 0;

            Random rnd = new Random();

            _sw.Restart();

            for (int i = 0; i < tries; i++)
            {
                index = rnd.Next(itemsCount);
                tmp = lst[index];
            }

            _sw.Stop();

            Console.Write(tmp);

            return _sw.Elapsed;
        }
        #endregion
    }
}
