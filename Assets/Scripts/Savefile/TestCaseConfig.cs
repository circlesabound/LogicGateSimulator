using System;
using System.Collections.Generic;

namespace Assets.Scripts.Savefile
{

    public static class TestCaseConstants
    {
        public const uint MAX_INDEX = 30;
    }

    [Serializable]
    public class TestCaseConfig
    {
        // Encoded as a bitstring
        // Where LSB is value to set 0th
        public int usedInputs;
        public int expectedInputs;
        public int usedOutputs;
        public int expectedOutputs;

        public TestCaseConfig()
        {
            usedInputs = 0;
            expectedInputs = 0;
            usedOutputs = 0;
            expectedOutputs = 0;
        }

        // Returns the expected input for index.
        // -1 if it shouldn't be used, 0 for false, 1 for true.
        public int GetInput(uint index)
        {
            if (index > TestCaseConstants.MAX_INDEX)
            {
                return -1;
            }
            if ((usedInputs & (1 << (int)index)) == 0)
            {
                return -1;
            }
            return (expectedInputs & (1 << (int)index)) >> (int)index;
        }

        // Returns a Dictionary of all expected inputs.
        public Dictionary<uint, bool> GetAllInputs()
        {
            Dictionary<uint, bool> ExpectedInputs = new Dictionary<uint, bool>();
            for (uint i = 0; i <= TestCaseConstants.MAX_INDEX; i++)
            {
                int val = GetInput(i);
                if (val != -1)
                {
                    ExpectedInputs.Add(i, val != 0);
                }
            }
            return ExpectedInputs;
        }

        // Returns the expected output for index.
        // -1 if it shouldn't be used, 0 for false, 1 for true.
        public int GetOutput(uint index)
        {
            if (index > TestCaseConstants.MAX_INDEX)
            {
                return -1;
            }
            if ((usedOutputs & (1 << (int)index)) == 0)
            {
                return -1;
            }
            return (expectedOutputs & (1 << (int)index)) >> (int)index;
        }

        // Returns a Dictionary of all expected outputs.
        public Dictionary<uint, bool> GetAllOutputs()
        {
            Dictionary<uint, bool> ExpectedOutputs = new Dictionary<uint, bool>();
            for (uint i = 0; i <= TestCaseConstants.MAX_INDEX; i++)
            {
                int val = GetOutput(i);
                if (val != -1)
                {
                    ExpectedOutputs.Add(i, val != 0);
                }
            }
            return ExpectedOutputs;
        }

        // Sets an expected input.
        public void SetInput(uint index, bool val)
        {
            if (index > TestCaseConstants.MAX_INDEX)
            {
                return;
            }
            usedInputs |= (1 << (int)index);
            if (val == false)
            {
                expectedInputs &= ((~0) - (1 << (int)index));
            }
            else
            {
                expectedInputs |= (1 << (int)index);
            }
        }

        // Sets an expected output.
        public void SetOutput(uint index, bool val)
        {
            if (index > TestCaseConstants.MAX_INDEX)
            {
                return;
            }
            usedOutputs |= (1 << (int)index);
            if (val == false)
            {
                expectedOutputs &= ((~0) - (1 << (int)index));
            }
            else
            {
                expectedOutputs |= (1 << (int)index);
            }
        }
    }
}
