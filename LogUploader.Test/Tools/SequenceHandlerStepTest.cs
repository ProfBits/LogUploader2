using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using LogUploader.Tools.SequenceHandler;
using LogUploader.Data;
using System.Threading;
using System.Xml.Linq;

namespace LogUploader.Test.Tools
{
    public abstract class SequenceHandlerStepTest
    {
        protected const string DEFALUT_NAME = "Step";
        protected const int DEFALUT_SEQ_NUM = 1;
        protected const int DEFALUT_WEIGTH = 1;
        protected const StepMode DEFALUT_STEP_MODE = StepMode.VITAL;

        internal abstract SequenceStep CreateSequenceStep(string name, int sequenceNumber, int weight, StepMode stepMode);

        [Test, Combinatorial]
        public void CreateStep(
            [Values(-1, 0, 1, 100)] int sequNumber,
            [Values(1, 50)] int weight,
            [Values] StepMode stepMode)
        {
            SequenceStep step = CreateSequenceStep(DEFALUT_NAME, sequNumber, weight, stepMode);

            Assert.AreEqual(DEFALUT_NAME, step.Name);
            Assert.AreEqual(sequNumber, step.SequenceNumber);
            Assert.AreEqual(weight, step.Weight);
            Assert.AreEqual(stepMode == StepMode.VITAL, step.Vital);
            Assert.AreEqual(stepMode == StepMode.CAN_BE_CANCELED, step.IsIndividualCanceable);
        }

        [Test, Combinatorial]
        public void CreateStepInvalidName(
            [Values(null, "", " ", " \n  \t\r")] string name,
            [Values] StepMode stepMode)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => CreateSequenceStep(name, DEFALUT_SEQ_NUM, DEFALUT_WEIGTH, stepMode));
        }

        [Test, Combinatorial]
        public void CreateStepInvalidWeigth(
            [Values(int.MinValue, -1, 0)] int weight,
            [Values] StepMode stepMode)
        {
            Assert.Throws<ArgumentException>(() => CreateSequenceStep(DEFALUT_NAME, DEFALUT_SEQ_NUM, weight, stepMode));
        }

        [Test, Combinatorial]
        public void CreateStepMultibleInvalidArgs(
            [Values(null, "", " ", " \n  \t\r")] string name,
            [Values(int.MinValue, -1, 0)] int weight,
            [Values] StepMode stepMode)
        {
            Exception ex = Assert.Catch<Exception>(() => CreateSequenceStep(name, DEFALUT_SEQ_NUM, weight, stepMode));
            Assert.IsTrue(ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is ArgumentException, "Exception type is invalid");
        }
    }

    public class SequenceHandlerAsyncStepTest : SequenceHandlerStepTest
    {
        internal override SequenceStep CreateSequenceStep(string name, int sequenceNumber, int weight, StepMode stepMode)
        {
            return new AsynchronusStep(async (p, c) => await Task.Delay(0), name, sequenceNumber, weight, stepMode);
        }

        [Test]
        public void CreateStepInvalidCallBack(
            [Values] StepMode stepMode)
        {
            Assert.Throws<ArgumentNullException>(() => new AsynchronusStep(null, DEFALUT_NAME, DEFALUT_SEQ_NUM, DEFALUT_WEIGTH, stepMode));
        }

        [Test, Combinatorial]
        public void CreateAsyncStepMultibleInvalidArgs(
            [Values(null, "", " ", " \n  \t\r")] string name,
            [Values(int.MinValue, -1, 0)] int weight,
            [Values] StepMode stepMode)
        {
            Exception ex = Assert.Catch<Exception>(() => new AsynchronusStep(null, name, DEFALUT_SEQ_NUM, weight, stepMode));
            Assert.IsTrue(ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is ArgumentException, "Exception type is invalid");
        }

        [Test]
        public async Task CreateWithAsyncCallback([Values] StepMode stepMode)
        {
            bool Flag = false;
            AsynchronusStep step = new AsynchronusStep(async (p, c) => await Task.Run(() => Flag = true), DEFALUT_NAME, DEFALUT_SEQ_NUM, DEFALUT_WEIGTH, stepMode);
            Assert.IsNotNull(step);
            await step.Execute(new Progress<ProgressMessage>(), default);
            Assert.IsTrue(Flag, "Flag should have be set by step to true");
        }
    }

    public class SequenceHandlerSyncStepTest : SequenceHandlerStepTest
    {
        internal override SequenceStep CreateSequenceStep(string name, int sequenceNumber, int weight, StepMode stepMode)
        {
            return new SynchronusStep((p, c) => Task.Delay(0).Wait(), name, sequenceNumber, weight, stepMode);
        }

        [Test]
        public void CreateStepInvalidCallBack(
            [Values] StepMode stepMode)
        {
            Assert.Throws<ArgumentNullException>(() => new SynchronusStep(null, DEFALUT_NAME, DEFALUT_SEQ_NUM, DEFALUT_WEIGTH, stepMode));
        }

        [Test, Combinatorial]
        public void CreateAsyncStepMultibleInvalidArgs(
            [Values(null, "", " ", " \n  \t\r")] string name,
            [Values(int.MinValue, -1, 0)] int weight,
            [Values] StepMode stepMode)
        {
            Exception ex = Assert.Catch<Exception>(() => new SynchronusStep(null, name, DEFALUT_SEQ_NUM, weight, stepMode));
            Assert.IsTrue(ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is ArgumentException, "Exception type is invalid");
        }

        [Test]
        public async Task CreateWithAsyncCallback([Values] StepMode stepMode)
        {
            bool Flag = false;
            SynchronusStep step = new SynchronusStep((p, c) => { Flag = true; }, DEFALUT_NAME, DEFALUT_SEQ_NUM, DEFALUT_WEIGTH, stepMode);
            Assert.IsNotNull(step);
            await step.Execute(new Progress<ProgressMessage>(), default);
            Assert.IsTrue(Flag, "Flag should have be set by step to true");
        }
    }
}
