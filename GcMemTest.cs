using System;

namespace InterestingTestsProject
{
    /// <summary>
    /// Class for doing memory tests. Main idea: check weight of app before and after creation of some object
    /// </summary>
    public class GcMemTest
    {
        private long _kbAtExecution;
        
        public void Restart()
        {
            // Needed for being sure that GC will not clean some information due to test run.
            GC.Collect();
            
            _kbAtExecution = GC.GetTotalMemory(false) / 1024;
        }

        /// <summary>
        /// Finish memory test
        /// </summary>
        /// <returns>difference between test start and test finish in Kb</returns>
        public long FinishMemTest()
        {
            return GC.GetTotalMemory(true) / 1024;
        }
    }
}


/*
 * May be interesting to implement some additional tests in future:
 
    Note that if you're prepending a lot (as you're essentially doing in the last example) or deleting the first entry, a linked list will nearly always be significantly faster, as there is no searching or moving/copying to do. A List would require moving everything up a spot to accommodate the new item, making prepending an O(N) operation.
    
    So worst case, if we are adding the first (zeroth) element each time, then the blit has to move everything each time.

    count is cached in both list classes. Possibly it's not linear, its constant time.

    The memory footprint after the Clear is considerably different, as the List<T> keeps its size, while the LinkedList does not.

    check clear and RemoveAll speed and compare 
     */
