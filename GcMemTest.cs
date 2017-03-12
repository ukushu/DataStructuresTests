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