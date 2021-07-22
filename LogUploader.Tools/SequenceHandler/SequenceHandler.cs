using LogUploader.Data;
using LogUploader.Tools.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogUploader.Tools.SequenceHandler
{
    public class SequenceHandler
    {
        private const int DEFAULT_FIRST_SEQUENCE_NUMBER = 1;
        private List<SequenceStep> Steps = new List<SequenceStep>();
        public bool Running { get; private set; } = false;
        public bool Aborted { get; private set; } = false;
        public int NumerOfSteps { get => Steps.Count; }

        public SequenceHandler()
        {

        }

        #region Events

        public event SequenceStartEventHandler SequenceStart;
        public delegate void SequenceStartEventHandler(object sender, SequenceStartEventArgs e);
        protected virtual void RaiseSequenceStartEvent(int totalStepCount, int currStepNum, Action cancel)
        {
            SequenceStart?.Invoke(this, new SequenceStartEventArgs(totalStepCount, currStepNum, cancel));
        }

        public event SequenceStepStartEventHandler SequenceStepStart;
        public delegate void SequenceStepStartEventHandler(object sender, SequenceStepStartEventArgs e);
        protected virtual void RaiseSequenceStepStartEvent(int totalStepCount, int currStepNum, string stepName,
                                                           int stepId)
        {
            SequenceStepStart?.Invoke(this, new SequenceStepStartEventArgs(totalStepCount, currStepNum, stepName, stepId));
        }
        protected virtual void RaiseSequenceStepStartEvent(int totalStepCount, int currStepNum, string stepName,
                                                          int stepId, Action cancel)
        {
            if (null == cancel) throw new ArgumentNullException($"{nameof(cancel)} action cannot be null");
            SequenceStepStart?.Invoke(this, new SequenceStepStartEventArgs(totalStepCount, currStepNum, stepName, stepId, cancel));
        }

        public event SequenceStepCompletedEventHandler SequenceStepCompleted;
        public delegate void SequenceStepCompletedEventHandler(object sender, SequenceStepCompletedEventArgs e);
        protected virtual void RaiseSequenceStepCompletedEvent(int totalStepCount, int currStepNum, string stepName, int stepId, Exception error)
        {
            SequenceStepCompleted?.Invoke(this, new SequenceStepCompletedEventArgs(totalStepCount, currStepNum, stepName, stepId, error));
        }

        public event SequenceEndEventHandler SequenceEnd;
        public delegate void SequenceEndEventHandler(object sender, SequenceEndEventArgs e);
        protected virtual void RaiseSequenceEndEvent(int totalStepCount, int currStepNum, IReadOnlyList<Exception> errors)
        {
            SequenceEnd?.Invoke(this, new SequenceEndEventArgs(totalStepCount, currStepNum, errors));
        }

        #endregion
        #region Modify sequence

        public int AddStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> action, string name,
                           int weight, StepMode stepMode = StepMode.VITAL)
        {
            int sequenceNumber = GetNextSequenceNumber();
            return AddStep(action, name, weight, sequenceNumber, stepMode);
        }

        public int AddStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> action, string name,
                           int weight, int sequenceNumber, StepMode stepMode = StepMode.VITAL)
        {
            SequenceStep step = new AsynchronusStep(action, name, sequenceNumber, weight, stepMode);
            Steps.Add(step);

            return step.ID;
        }

        public int AddStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                           int weight, StepMode stepMode = StepMode.VITAL)
        {
            int sequenceNumber = GetNextSequenceNumber();
            return AddStep(action, name, weight, sequenceNumber, stepMode);
        }

        public int AddStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                           int weight, int sequenceNumber, StepMode stepMode = StepMode.VITAL)
        {
            SequenceStep step = new SynchronusStep(action, name, sequenceNumber, weight, stepMode);
            Steps.Add(step);

            return step.ID;
        }

        internal void RemoveStep(int stepID)
        {
            if (Steps.Where(step => step.ID == stepID).Count() != 1)
                throw new IndexOutOfRangeException($"Sequence id {stepID} does not exist in this Sequece");
            Steps.RemoveAll(step => step.ID == stepID);
        }

        private int GetNextSequenceNumber()
        {
            return Steps.Any() ? Steps.Max(step => step.SequenceNumber) + 1 : DEFAULT_FIRST_SEQUENCE_NUMBER;
        }

        #endregion
        #region Run sequence

        public async Task ExecuteSequence(CancellationTokenSource cts, IProgress<ProgressMessage> progress)
        {
            Running = true;
            Aborted = false;

            Queue<SequenceStep> orderdSequence = GetOrderedSequence(Steps);
            SequenceState sequenceState = new SequenceState(GetTotalSequenceWeight(Steps), orderdSequence.Count, cts.Token);

            RaiseSequenceStartEvent(sequenceState.NumberOfSteps, sequenceState.CurrentStepNumber, () => { cts?.Cancel(); cts = null; });
            List<Exception> exceptions = new List<Exception>();

            while (orderdSequence.Any() && !Aborted)
            {
                SequenceStep step = orderdSequence.Dequeue();

                Exception stepException = await ExecuteStep(step, sequenceState, progress);

                if (stepException != null)
                    exceptions.Add(stepException);


                if (sequenceState.SequenceCancellationToken.IsCancellationRequested && orderdSequence.Any())
                {
                    exceptions.Add(new OperationCanceledException($"Sequece canceled by user at step {sequenceState.CurrentStepNumber} {step.Name}"));
                    Aborted = true;
                    break;
                }

                sequenceState.Update(step);
            }
            RaiseSequenceEndEvent(sequenceState.NumberOfSteps, sequenceState.CurrentStepNumber, exceptions);
            Running = false;
        }

        private async Task<Exception> ExecuteStep(SequenceStep step, SequenceState sequenceState, IProgress<ProgressMessage> progress)
        {
            using (StepData stepData = SetUpStep(step, sequenceState))
            {
                Exception stepException = await RunStep(step, sequenceState, stepData, progress);
                return stepException;
            }
        }

        private StepData SetUpStep(SequenceStep step, SequenceState sequenceState)
        {
            Func<ProgressMessage, ProgressMessage> pMsgConverter = step.GetProgressMessageConverter(sequenceState.RunningWeight, sequenceState.TotalWeight);
            CancellationTokenSource stepCTS = null;
            if (step.IsIndividualCanceable)
            {
                stepCTS = CancellationTokenSource.CreateLinkedTokenSource(sequenceState.SequenceCancellationToken);
                RaiseSequenceStepStartEvent(sequenceState.NumberOfSteps, sequenceState.CurrentStepNumber, step.Name, step.ID, () => stepCTS?.Cancel());
            }
            else
                RaiseSequenceStepStartEvent(sequenceState.NumberOfSteps, sequenceState.CurrentStepNumber, step.Name, step.ID);
            return new StepData(pMsgConverter, stepCTS);
        }

        private async Task<Exception> RunStep(SequenceStep step, SequenceState sequenceState, StepData stepData, IProgress<ProgressMessage> progress)
        {
            if (step == null) throw new ArgumentNullException($"{step} cannot be null");
            if (sequenceState == null) throw new ArgumentNullException($"{sequenceState} cannot be null");
            if (stepData == null) throw new ArgumentNullException($"{stepData} cannot be null");

            Exception stepException = null;

            try
            {
                await step.Execute(new Progress<ProgressMessage>((pMsg) => progress?.Report(stepData.ProgressMessageConverter(pMsg))), stepData.StepCancellationTokenSource?.Token ?? sequenceState.SequenceCancellationToken);
            }
            catch (Exception e)
            {
                stepException = e;
                LogStepExecutionException(step, e);
                Aborted = Aborted || step.Vital;
            }
            finally
            {
                RaiseSequenceStepCompletedEvent(sequenceState.NumberOfSteps, sequenceState.CurrentStepNumber, step.Name, step.ID, stepException);
            }

            return stepException;
        }

        private int GetTotalSequenceWeight(IEnumerable<SequenceStep> steps)
        {
            return steps.Sum(step => step.Weight);
        }

        private Queue<SequenceStep> GetOrderedSequence(IEnumerable<SequenceStep> steps)
        {
            return new Queue<SequenceStep>(steps.OrderBy(step => step.SequenceNumber));
        }

        private static void LogStepExecutionException(SequenceStep step, Exception e)
        {
            if (e is OperationCanceledException)
                Logger.Warn($"Sequence setp Failed.\nName: {step.Name}\nVital: {step.Vital}\nSingleCancelAllowed: {step.IsIndividualCanceable}");
            else
                Logger.Error($"Sequence setp Failed.\nName: {step.Name}\nVital: {step.Vital}\nSingleCancelAllowed: {step.IsIndividualCanceable}");

            Exception ex = e;
            do
            {
                Logger.LogException(e);
                ex = e.InnerException;
            } while (ex != null);
        }

        private class SequenceState
        {
            public int TotalWeight { get; }
            public int NumberOfSteps { get; }
            public int RunningWeight { get; private set; } = 0;
            public int CurrentStepNumber { get; private set; } = 0;
            public CancellationToken SequenceCancellationToken { get; }

            public SequenceState(int totalWeight, int numberOfSteps, CancellationToken sequenceCancellationToken)
            {
                if (totalWeight <= 0) throw new ArgumentOutOfRangeException($"{nameof(totalWeight)} has to be greater than 0");
                if (numberOfSteps <= 0) throw new ArgumentOutOfRangeException($"{nameof(numberOfSteps)} has to be greater than 0");

                TotalWeight = totalWeight;
                NumberOfSteps = numberOfSteps;
                SequenceCancellationToken = sequenceCancellationToken;
            }

            public void Update(SequenceStep step)
            {
                if (step == null) throw new ArgumentNullException($"{nameof(step)} cannot be null");

                RunningWeight += step.Weight;
                CurrentStepNumber += 1;
            }
        }

        private class StepData : IDisposable
        {
            public Func<ProgressMessage, ProgressMessage> ProgressMessageConverter { get; }
            public CancellationTokenSource StepCancellationTokenSource { get; }

            public StepData(Func<ProgressMessage, ProgressMessage> progressMessageConverter, CancellationTokenSource stepCancellationTokenSource)
            {
                ProgressMessageConverter = progressMessageConverter ?? throw new ArgumentNullException(nameof(progressMessageConverter));
                StepCancellationTokenSource = stepCancellationTokenSource;
            }

            public void Dispose()
            {
                ((IDisposable)StepCancellationTokenSource)?.Dispose();
            }
        }

        #endregion
    }

    #region SequenceStep definitions

    internal abstract class SequenceStep
    {
        private static int currentId = 0;

        public string Name { get; }
        public int SequenceNumber { get; }
        public int Weight { get; }
        public bool Vital { get; }
        public bool IsIndividualCanceable { get; }
        public int ID { get; } = ++currentId;

        public SequenceStep(string name, int sequenceNumber, int weight, StepMode stepMode)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentOutOfRangeException($"{nameof(name)} cannot be null or whitespace");
            if (weight < 1) throw new ArgumentException($"{nameof(weight)} cannot be negative or null");

            Name = name;
            SequenceNumber = sequenceNumber;
            Weight = weight;
            Vital = stepMode == StepMode.VITAL;
            IsIndividualCanceable = stepMode == StepMode.CAN_BE_CANCELED;
        }

        public abstract Task Execute(IProgress<ProgressMessage> progress, CancellationToken ct);
        public virtual Func<ProgressMessage, ProgressMessage> GetProgressMessageConverter(int runningWeight, int totalWeight)
        {
            double startPercent = (double)runningWeight / totalWeight;
            double stepPercentFactor = (double)Weight / totalWeight;
            double percentConverter(double p) => startPercent + (p * stepPercentFactor);

            if (string.IsNullOrEmpty(Name))
                return (ProgressMessage pMsg) => new ProgressMessage(percentConverter(pMsg.Percent), pMsg.Message);
            else
                return (ProgressMessage pMsg) => new ProgressMessage(percentConverter(pMsg.Percent), $"{Name} - {pMsg.Message}");
        }

    }

    internal class AsynchronusStep : SequenceStep
    {
        readonly Func<IProgress<ProgressMessage>, CancellationToken, Task> Action;

        public AsynchronusStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> asyncAction, string name, int sequenceNumber, int weight, StepMode stepMode)
            : base(name, sequenceNumber, weight, stepMode)
        {
            Action = asyncAction ?? throw new ArgumentNullException($"{nameof(asyncAction)} cannot be null");
        }

        public override async Task Execute(IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            await Action(progress, ct);
        }
    }

    internal class SynchronusStep : SequenceStep
    {
        readonly Action<IProgress<ProgressMessage>, CancellationToken> Action;

        public SynchronusStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                              int sequenceNumber, int weight, StepMode stepMode)
            : base(name, sequenceNumber, weight, stepMode)
        {
            Action = action ?? throw new ArgumentNullException($"{nameof(action)} cannot be null");
        }

        public override async Task Execute(IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            await Task.Run(() => Action(progress, ct));
        }
    }

    public enum StepMode
    {
        VITAL,
        CAN_BE_CANCELED,
        CAN_NOT_BE_CANCELED
    }

    #endregion

    #region Sequence eventArgs

    public class BasicSequenceEventArgs : EventArgs
    {
        public BasicSequenceEventArgs(int totalStepCount, int currStepNum) : base()
        {
            TotalStepCount = totalStepCount;
            CurrStepNum = currStepNum;
        }

        public int TotalStepCount { get; }
        public int CurrStepNum { get; }
    }

    public class SequenceEventArgs : BasicSequenceEventArgs
    {
        public string StepName { get; }
        public int StepId { get; }
        public SequenceEventArgs(int totalStepCount, int currStepNum, string stepName, int stepId)
            : base(totalStepCount, currStepNum)
        {
            StepName = stepName;
            StepId = stepId;
        }
    }

    public class SequenceStartEventArgs : BasicSequenceEventArgs
    {
        public Action Cancel { get; }

        public SequenceStartEventArgs(int totalStepCount, int currStepNum, Action cancel)
            : base(totalStepCount, currStepNum)
        {
            Cancel = cancel ?? throw new ArgumentNullException(nameof(cancel));
        }
    }

    public class SequenceStepStartEventArgs : SequenceEventArgs
    {
        public bool CanBeCanceled { get; }
        public Action Cancel { get; }

        public SequenceStepStartEventArgs(int totalStepCount, int currStepNum, string stepName, int stepId,
                                          Action cancel)
            : base(totalStepCount, currStepNum, stepName, stepId)
        {
            Cancel = cancel ?? throw new ArgumentNullException($"{nameof(cancel)} action cannot be null");
            CanBeCanceled = true;
        }

        public SequenceStepStartEventArgs(int totalStepCount, int currStepNum, string stepName, int stepId)
            : base(totalStepCount, currStepNum, stepName, stepId)
        {
            CanBeCanceled = false;
            Cancel = null;
        }
    }

    public class SequenceStepCompletedEventArgs : SequenceEventArgs
    {
        public Exception Error { get; }

        public SequenceStepCompletedEventArgs(int totalStepCount, int currStepNum, string stepName, int stepId,
                                              Exception error)
            : base(totalStepCount, currStepNum, stepName, stepId)
        {
            Error = error;
        }
    }

    public class SequenceEndEventArgs : BasicSequenceEventArgs
    {
        public IReadOnlyList<Exception> Errors { get; }

        public SequenceEndEventArgs(int totalStepCount, int currStepNum, IReadOnlyList<Exception> errors)
            : base(totalStepCount, currStepNum)
        {
            Errors = errors;
        }
    }

    #endregion

}
