using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
using LogUploader.Tools.SequenceHandler;
using LogUploader.Data;
using System.Threading;

namespace LogUploader.Test.Tools
{
    public class SequenceHandlerTest
    {
        private SequenceHandler GetSequenceHandler()
        {
            return new SequenceHandler();
        }

        [Test]
        public void SequenceInitStateTest()
        {
            var sqh = GetSequenceHandler();
            Assert.IsFalse(sqh.Aborted);
            Assert.IsFalse(sqh.Running);
        }

        [Test]
        public void SequenceAddStepTest()
        {
            var sqh = GetSequenceHandler();
            List<ProgressMessage> messages = new List<ProgressMessage>();
            Assert.DoesNotThrow(() => sqh.AddStep(
                async (progress, ct) => await Task.Delay(0),
                "setp1",
                1));
            Assert.Throws<ArgumentNullException>(() => sqh.AddStep(
                null,
                "setp2",
                1));
            Assert.Throws<ArgumentOutOfRangeException>(() => sqh.AddStep(
                async (progress, ct) => await Task.Delay(0),
                null,
                1));
        }

        [Test]
        public async Task SequenceExecuteTest()
        {
            List<int> log = new List<int>();
            var sqh = GetSequenceHandler();
            sqh.AddStep(async (progress, ct) =>
            {
                log.Add(1);
                await Task.Delay(0);
            },
            "step1",
            1, 1);
            sqh.AddStep((progress, ct) =>
            {
                log.Add(3);
            },
            "step3",
            1, 3);
            sqh.AddStep(async (progress, ct) =>
            {
                log.Add(2);
                await Task.Delay(0);
            },
            "step2",
            1, 2);
            await sqh.ExecuteSequence(new CancellationTokenSource(), new SynchronProgress<ProgressMessage>(msg => { }));
            Assert.AreEqual(3, log.Count);
            int next = 1;
            foreach (var elemetn in log)
            {
                Assert.AreEqual(next, elemetn);
                next += 1;
            }
        }

        [Test]
        public async Task SequenceAddOrderExecuteTest()
        {
            List<int> log = new List<int>();
            var sqh = GetSequenceHandler();
            sqh.AddStep(async (progress, ct) =>
            {
                log.Add(1);
                await Task.Delay(0);
            },
            "step1",
            1);
            sqh.AddStep(async (progress, ct) =>
            {
                log.Add(2);
                await Task.Delay(0);
            },
            "step2",
            1);
            sqh.AddStep((progress, ct) =>
            {
                log.Add(3);
            },
            "step3",
            1, 10);
            sqh.AddStep(async (progress, ct) =>
            {
                log.Add(4);
                await Task.Delay(0);
            },
            "step4",
            1, 10);
            await sqh.ExecuteSequence(new CancellationTokenSource(), new SynchronProgress<ProgressMessage>(msg => { }));
            Assert.AreEqual(4, log.Count);
            int next = 1;
            foreach (var elemetn in log)
            {
                Assert.AreEqual(next, elemetn);
                next += 1;
            }
        }

        [Test]
        public void SequenceGetNumberOfStepsTest()
        {
            var sqh = GetSequenceHandler();
            Assert.AreEqual(0, sqh.NumerOfSteps);
            int step1 = sqh.AddStep((p, ct) => { }, "stepA", 1);
            Assert.AreEqual(1, sqh.NumerOfSteps);
            int step2 = sqh.AddStep((p, ct) => { }, "stepB", 1);
            Assert.AreEqual(2, sqh.NumerOfSteps);
            try { sqh.AddStep(null, null, 1); } catch (ArgumentNullException) { } catch (ArgumentOutOfRangeException) { }
            Assert.AreEqual(2, sqh.NumerOfSteps);
            sqh.RemoveStep(step1);
            Assert.AreEqual(1, sqh.NumerOfSteps);

        }

        [Test]
        public void SequenceRemoveTest()
        {
            var sqh = GetSequenceHandler();
            int step1 = sqh.AddStep((p, ct) => { }, "stepA", 1);
            sqh.RemoveStep(step1);
            Assert.AreEqual(0, sqh.NumerOfSteps);
            IndexOutOfRangeException ex = Assert.Catch<IndexOutOfRangeException>(() => sqh.RemoveStep(step1));
            Assert.IsFalse(string.IsNullOrWhiteSpace(ex.Message), "Invalid remove error message shall not be empty");

            int step2 = sqh.AddStep((p, ct) => { }, "stepB", 1);
            int step3 = sqh.AddStep((p, ct) => { }, "stepC", 1);
            sqh.RemoveStep(step2);
            Assert.AreEqual(1, sqh.NumerOfSteps);
        }

        [Test]
        public async Task SequenceAllEventTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;

            var step1 = sqh.AddStep(async (p, c) => await Task.Delay(0), "StepA", 3);
            var step2 = sqh.AddStep(async (p, c) => await Task.Delay(0), "StepB", 2);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(eventTracker.CallOrder, "s ss se ss se e ");
            Assert.True(eventTracker.Senders.All(sender => sender == sqh));
            Assert.AreEqual(6, eventTracker.NumOfEvents);

            Assert.AreEqual(2, eventTracker.StartArgs.TotalStepCount);
            Assert.AreNotEqual(null, eventTracker.StartArgs.Cancel);
            Assert.AreEqual(2, eventTracker.EndArgs.TotalStepCount);
            Assert.AreEqual(0, eventTracker.EndArgs.Errors.Count);

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepStartArgsB.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsB.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsB.Cancel);
            Assert.AreEqual(step2, eventTracker.StepStartArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepStartArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepStartArgsB.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsB.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsB.Error);
            Assert.AreEqual(step2, eventTracker.StepEndArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepEndArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepEndArgsB.CurrStepNum);
        }

        [Test]
        public async Task SequenceRunWithVitalUnhandledExceptionTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;
            bool stepBRan = false;

            var exception = new NotImplementedException("This is expteted");
            var step1 = sqh.AddStep(async (p, c) => await Task.Run(() => throw exception), "StepA", 3);
            var step2 = sqh.AddStep(async (p, c) => await Task.Run(() => stepBRan = true), "StepB", 2);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(eventTracker.CallOrder, "s ss se e ");
            Assert.True(eventTracker.Senders.All(sender => sender == sqh));
            Assert.AreEqual(4, eventTracker.NumOfEvents);

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.IsNotNull(eventTracker.StepEndArgsA.Error);
            Assert.IsInstanceOf<NotImplementedException>(eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(exception, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.IsNull(eventTracker.StepStartArgsB);
            Assert.IsNull(eventTracker.StepEndArgsB);
            Assert.IsFalse(stepBRan);

            Assert.AreEqual(2, eventTracker.EndArgs.TotalStepCount);
            Assert.AreEqual(1, eventTracker.EndArgs.Errors.Count);
            Assert.AreEqual(exception, eventTracker.EndArgs.Errors.FirstOrDefault());
        }

        [Test]
        public async Task SequenceRunWithNonVitalUnhandledExceptionTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;
            bool stepBRan = false;

            var exception = new NotImplementedException("This is expteted");
            var step1 = sqh.AddStep(async (p, c) => await Task.Run(() => throw exception), "StepA", 3, StepMode.CAN_NOT_BE_CANCELED);
            var step2 = sqh.AddStep(async (p, c) => await Task.Run(() => stepBRan = true), "StepB", 2);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(eventTracker.CallOrder, "s ss se ss se e ");
            Assert.True(eventTracker.Senders.All(sender => sender == sqh));
            Assert.AreEqual(6, eventTracker.NumOfEvents);

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.IsNotNull(eventTracker.StepEndArgsA.Error);
            Assert.IsInstanceOf<NotImplementedException>(eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(exception, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.IsNotNull(eventTracker.StepStartArgsB);
            Assert.IsNotNull(eventTracker.StepEndArgsB);
            Assert.IsTrue(stepBRan);

            Assert.AreEqual(2, eventTracker.EndArgs.TotalStepCount);
            Assert.AreEqual(1, eventTracker.EndArgs.Errors.Count);
            Assert.AreEqual(exception, eventTracker.EndArgs.Errors.FirstOrDefault());
        }

        [Test]
        public async Task SequenceCancelTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;
            bool stepBRan = false;

            var step1 = sqh.AddStep(async (p, c) => await Task.Run(() => eventTracker.StartArgs.Cancel()), "StepA", 3);
            var step2 = sqh.AddStep(async (p, c) => await Task.Run(() => stepBRan = true), "StepB", 2);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(eventTracker.CallOrder, "s ss se e ");
            Assert.True(eventTracker.Senders.All(sender => sender == sqh));
            Assert.AreEqual(4, eventTracker.NumOfEvents);

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.IsNull(eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.IsNull(eventTracker.StepStartArgsB);
            Assert.IsNull(eventTracker.StepEndArgsB);
            Assert.IsFalse(stepBRan);

            Assert.AreEqual(2, eventTracker.EndArgs.TotalStepCount);
            Assert.IsInstanceOf<OperationCanceledException>(eventTracker.EndArgs.Errors.FirstOrDefault());
        }

        [Test]
        public async Task SequenceStepCancelTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepStart += (sender, arg) => { if (arg.CanBeCanceled) arg.Cancel(); };
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;
            bool stepBRan = false;

            var step1 = sqh.AddStep(async (p, c) => await Task.Run(() => { c.WaitHandle.WaitOne(500); if (c.IsCancellationRequested) throw new OperationCanceledException("StepA caneled"); }), "StepA", 3, StepMode.CAN_BE_CANCELED);
            var step2 = sqh.AddStep(async (p, c) => await Task.Run(() => stepBRan = true), "StepB", 2);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(eventTracker.CallOrder, "s ss se ss se e ");
            Assert.True(eventTracker.Senders.All(sender => sender == sqh));
            Assert.AreEqual(6, eventTracker.NumOfEvents);

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsTrue(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNotNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.IsNotNull(eventTracker.StepEndArgsA.Error);
            Assert.IsInstanceOf<OperationCanceledException>(eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.IsNotNull(eventTracker.StepStartArgsB);
            Assert.IsNotNull(eventTracker.StepEndArgsB);
            Assert.IsTrue(stepBRan);

            Assert.AreEqual(2, eventTracker.EndArgs.TotalStepCount);
            Assert.IsInstanceOf<OperationCanceledException>(eventTracker.EndArgs.Errors.FirstOrDefault());
        }

        [Test]
        public async Task SequenceStepModeVitalTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;

            var step1 = sqh.AddStep((p, c) => Task.Delay(0).Wait(), "StepA", 3, StepMode.VITAL);
            var step2 = sqh.AddStep(async (p, c) => await Task.Delay(0), "StepB", 2, StepMode.VITAL);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepStartArgsB.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsB.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsB.Cancel);
            Assert.AreEqual(step2, eventTracker.StepStartArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepStartArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepStartArgsB.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsB.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsB.Error);
            Assert.AreEqual(step2, eventTracker.StepEndArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepEndArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepEndArgsB.CurrStepNum);
        }

        [Test]
        public async Task SequenceStepModeCanNotBeCancelTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;

            var step1 = sqh.AddStep((p, c) => Task.Delay(0).Wait(), "StepA", 3, StepMode.CAN_NOT_BE_CANCELED);
            var step2 = sqh.AddStep(async (p, c) => await Task.Delay(0), "StepB", 2, StepMode.CAN_NOT_BE_CANCELED);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepStartArgsB.TotalStepCount);
            Assert.IsFalse(eventTracker.StepStartArgsB.CanBeCanceled);
            Assert.IsNull(eventTracker.StepStartArgsB.Cancel);
            Assert.AreEqual(step2, eventTracker.StepStartArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepStartArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepStartArgsB.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsB.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsB.Error);
            Assert.AreEqual(step2, eventTracker.StepEndArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepEndArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepEndArgsB.CurrStepNum);
        }

        [Test]
        public async Task SequenceStepModeCanBeCancelTest()
        {
            EventTracker eventTracker = new EventTracker();

            var sqh = GetSequenceHandler();
            sqh.SequenceStart += eventTracker.Track;
            sqh.SequenceStepStart += eventTracker.Track;
            sqh.SequenceStepCompleted += eventTracker.Track;
            sqh.SequenceEnd += eventTracker.Track;

            var step1 = sqh.AddStep((p, c) => Task.Delay(0).Wait(), "StepA", 3, StepMode.CAN_BE_CANCELED);
            var step2 = sqh.AddStep(async (p, c) => await Task.Delay(0), "StepB", 2, StepMode.CAN_BE_CANCELED);

            await sqh.ExecuteSequence(new CancellationTokenSource(), new Progress<ProgressMessage>());

            Assert.AreEqual(2, eventTracker.StepStartArgsA.TotalStepCount);
            Assert.IsTrue(eventTracker.StepStartArgsA.CanBeCanceled);
            Assert.IsNotNull(eventTracker.StepStartArgsA.Cancel);
            Assert.AreEqual(step1, eventTracker.StepStartArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepStartArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepStartArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepStartArgsB.TotalStepCount);
            Assert.IsTrue(eventTracker.StepStartArgsB.CanBeCanceled);
            Assert.IsNotNull(eventTracker.StepStartArgsB.Cancel);
            Assert.AreEqual(step2, eventTracker.StepStartArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepStartArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepStartArgsB.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsA.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsA.Error);
            Assert.AreEqual(step1, eventTracker.StepEndArgsA.StepId);
            Assert.AreEqual("StepA", eventTracker.StepEndArgsA.StepName);
            Assert.AreEqual(0, eventTracker.StepEndArgsA.CurrStepNum);

            Assert.AreEqual(2, eventTracker.StepEndArgsB.TotalStepCount);
            Assert.AreEqual(null, eventTracker.StepEndArgsB.Error);
            Assert.AreEqual(step2, eventTracker.StepEndArgsB.StepId);
            Assert.AreEqual("StepB", eventTracker.StepEndArgsB.StepName);
            Assert.AreEqual(1, eventTracker.StepEndArgsB.CurrStepNum);
        }

        [Test]
        public async Task SequenceExecuteFlagsRunningTest()
        {
            bool flagRunningState = false;
            var sqh = GetSequenceHandler();
            sqh.AddStep(async (progress, ct) => await Task.Run(() => flagRunningState = sqh.Running), "step1", 1, 1);
            Assert.IsFalse(sqh.Running);
            await sqh.ExecuteSequence(new CancellationTokenSource(), new SynchronProgress<ProgressMessage>(msg => { }));
            Assert.IsTrue(flagRunningState);
            Assert.IsFalse(sqh.Running);
        }

        [Test]
        public async Task SequenceExecuteFlagsAbortCacleTest()
        {
            bool flagAbortedStateStart = false;
            bool flagAbortedStateStepStart = false;
            bool flagAbortedStateStepEnd = false;
            bool flagAbortedStateEnd = false;
            var sqh = GetSequenceHandler();
            Action cancel = null;
            sqh.SequenceStart += (sender, args) => { flagAbortedStateStart = (sender as SequenceHandler).Aborted; cancel = args.Cancel; };
            sqh.SequenceStepStart += (sender, args) => { flagAbortedStateStepStart = (sender as SequenceHandler).Aborted; cancel(); };
            sqh.SequenceStepCompleted += (sender, args) => flagAbortedStateStepEnd = (sender as SequenceHandler).Aborted;
            sqh.SequenceEnd += (sender, args) => flagAbortedStateEnd = (sender as SequenceHandler).Aborted;
            sqh.AddStep(async (progress, ct) => await Task.Delay(0), "step1", 1, 1);
            sqh.AddStep(async (progress, ct) => await Task.Delay(0), "step2", 1, 1);
            Assert.IsFalse(sqh.Aborted);
            await sqh.ExecuteSequence(new CancellationTokenSource(), new SynchronProgress<ProgressMessage>(msg => { }));
            Assert.IsFalse(flagAbortedStateStart);
            Assert.IsFalse(flagAbortedStateStepStart);
            Assert.IsFalse(flagAbortedStateStepEnd);
            Assert.IsTrue(flagAbortedStateEnd);
            Assert.IsTrue(sqh.Aborted);
        }

        [Test]
        public async Task SequenceExecuteFlagsAbortExeptionTest()
        {
            bool flagAbortedStateStart = false;
            bool flagAbortedStateStepStart = false;
            bool flagAbortedStateStepEnd = false;
            bool flagAbortedStateEnd = false;
            var sqh = GetSequenceHandler();
            sqh.SequenceStart += (sender, args) => flagAbortedStateStart = (sender as SequenceHandler).Aborted;
            sqh.SequenceStepStart += (sender, args) => flagAbortedStateStepStart = (sender as SequenceHandler).Aborted;
            sqh.SequenceStepCompleted += (sender, args) => flagAbortedStateStepEnd = (sender as SequenceHandler).Aborted;
            sqh.SequenceEnd += (sender, args) => flagAbortedStateEnd = (sender as SequenceHandler).Aborted;
            sqh.AddStep((progress, ct) => throw new Exception("This shall error"), "step1", 1, 1);
            sqh.AddStep(async (progress, ct) => await Task.Delay(0), "step2", 1, 1);
            Assert.IsFalse(sqh.Aborted);
            await sqh.ExecuteSequence(new CancellationTokenSource(), new SynchronProgress<ProgressMessage>(msg => { }));
            Assert.IsFalse(flagAbortedStateStart);
            Assert.IsFalse(flagAbortedStateStepStart);
            Assert.IsTrue(flagAbortedStateStepEnd);
            Assert.IsTrue(flagAbortedStateEnd);
            Assert.IsTrue(sqh.Aborted);
        }

        private class EventTracker
        {
            public List<object> Senders { get; private set; } = new List<object>();
            public string CallOrder { get; private set; } = "";
            public int NumOfEvents { get; private set; } = 0;
            public SequenceStartEventArgs StartArgs { get; private set; }
            public SequenceEndEventArgs EndArgs { get; private set; }
            private bool ArgsStartASet = false;
            public SequenceStepStartEventArgs StepStartArgsA { get; private set; }
            private bool ArgsEndASet = false;
            public SequenceStepCompletedEventArgs StepEndArgsA { get; private set; }
            public SequenceStepStartEventArgs StepStartArgsB { get; private set; }
            public SequenceStepCompletedEventArgs StepEndArgsB { get; private set; }

            private void Track(object sender)
            {
                Senders.Add(sender);
                NumOfEvents += 1;
            }

            internal void Track(object sender, SequenceStartEventArgs args)
            {
                Track(sender);
                CallOrder += "s ";
                StartArgs = args;
            }

            internal void Track(object sender, SequenceEndEventArgs args)
            {
                Track(sender);
                CallOrder += "e ";
                EndArgs = args;
            }

            internal void Track(object sender, SequenceStepStartEventArgs args)
            {
                Track(sender);
                CallOrder += "ss ";
                if (!ArgsStartASet)
                    StepStartArgsA = args;
                else
                    StepStartArgsB = args;
                ArgsStartASet = true;
            }

            internal void Track(object sender, SequenceStepCompletedEventArgs args)
            {
                Track(sender);
                CallOrder += "se ";
                if (!ArgsEndASet)
                    StepEndArgsA = args;
                else
                    StepEndArgsB = args;
                ArgsEndASet = true;
            }
        }
    } 
}
