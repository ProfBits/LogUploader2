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
        //TODO add more Prameter null checks

        private const int DEFAULT_FIRST_SEQUENCE_NUMBER = 1;
        private int NextStepId = 1;
        private List<SequenceStep> Steps = new List<SequenceStep>();
        public bool Running { get; private set; } = false;
        public bool Aborted { get; private set; } = false;

        public SequenceHandler()
        {

        }

        public event SequenceStartEventHandler SequenceStart;
        public delegate void SequenceStartEventHandler(object sender, SequenceStartEventArgs e);
        protected virtual void RaiseSequenceStartEvent(int totalStepCount, int currStepNum, Action cancel)
        {
            SequenceStart?.Invoke(this, new SequenceStartEventArgs(totalStepCount, currStepNum, cancel));
        }

        public event SequenceStepStartEventHandler SequenceStepStart;
        public delegate void SequenceStepStartEventHandler(object sender, SequenceStepStartEventArgs e);
        protected virtual void RaiseSequenceStepStartEvent(int totalStepCount, int currStepNum, string stepName,
                                                           int stepId, bool canBeCanceled, Action cancel = null)
        {
            SequenceStepStart?.Invoke(this, new SequenceStepStartEventArgs(totalStepCount, currStepNum, stepName, stepId, canBeCanceled, cancel));
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

        public int AddStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> action, string name,
                           int weight, bool vital = true, bool isIndividualCanceable = false)
        {
            int sequenceNumber = GetNextSequenceNumber();
            return AddStep(action, name, weight, sequenceNumber, vital, isIndividualCanceable);
        }

        public int AddStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> action, string name,
                           int weight, int sequenceNumber, bool vital = true, bool isIndividualCanceable = false)
        {
            if (action == null) throw new ArgumentNullException("action cannot be null");
            SequenceStep step = new AsynchronusStep(action, name, sequenceNumber, weight, vital, isIndividualCanceable);
            Steps.Add(step);

            return step.ID;
        }

        public int AddStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                           int weight, bool vital = true, bool isIndividualCanceable = false)
        {
            int sequenceNumber = GetNextSequenceNumber();
            return AddStep(action, name, weight, sequenceNumber, vital, isIndividualCanceable);
        }

        private int GetNextSequenceNumber()
        {
            return Steps.Any() ? Steps.Max(step => step.SequenceNumber) + 1 : DEFAULT_FIRST_SEQUENCE_NUMBER;
        }

        public int AddStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                           int weight, int sequenceNumber, bool vital = true, bool isIndividualCanceable = false)
        {
            if (action == null) throw new ArgumentNullException("action cannot be null");
            SequenceStep step = new SynchronusStep(action, name, sequenceNumber, weight, vital, isIndividualCanceable);
            Steps.Add(step);

            return step.ID;
        }

        public async void ExecuteSequence(CancellationTokenSource cts, IProgress<ProgressMessage> progress)
        {
            Running = true;
            Aborted = false;

            Queue<SequenceStep> orderdSequence = GetOrderedSequence(Steps);
            int totalWeight = GetTotalSequenceWeight(Steps);
            int numOfSteps = orderdSequence.Count;
            int runningWeight = 0;
            int currentStepNum = 0;
            RaiseSequenceStartEvent(numOfSteps, currentStepNum, () => { cts?.Cancel(); cts = null; });
            List<Exception> exceptions = new List<Exception>();

            while (orderdSequence.Any() && !Aborted)
            {
                Exception stepException = null;
                SequenceStep step = orderdSequence.Dequeue();

                Func<ProgressMessage, ProgressMessage> pMsgConverter = step.GetProgressMessageConverter(runningWeight, totalWeight);
                CancellationTokenSource stepCTS = step.IsIndividualCanceable ? CancellationTokenSource.CreateLinkedTokenSource(cts.Token) : cts;

                if (step.IsIndividualCanceable)
                    RaiseSequenceStepStartEvent(numOfSteps, currentStepNum, step.Name, step.ID,
                                                step.IsIndividualCanceable, () => stepCTS?.Cancel());
                else
                    RaiseSequenceStepStartEvent(numOfSteps, currentStepNum, step.Name, step.ID,
                                                step.IsIndividualCanceable);
                try
                {
                    await step.Execute(new Progress<ProgressMessage>((pMsg) => progress.Report(pMsgConverter(pMsg))), stepCTS.Token);
                }
                catch (Exception e)
                {
                    stepException = e;
                    LogStepExecutionException(step, e);
                    exceptions.Add(e);
                    Aborted = step.Vital;
                }
                finally
                {
                    RaiseSequenceStepCompletedEvent(numOfSteps, currentStepNum, step.Name, step.ID, stepException);
                }

                if (step.IsIndividualCanceable) stepCTS.Dispose();

                runningWeight += step.Weight;
            }

            RaiseSequenceEndEvent(numOfSteps, currentStepNum, exceptions);
            Running = false;
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
    }

    internal abstract class SequenceStep
    {
        private static int currentId = 0;

        public string Name { get; }
        public int SequenceNumber { get; }
        public int Weight { get; }
        public bool Vital { get; }
        public bool IsIndividualCanceable { get; }
        public int ID { get; } = ++currentId;

        public SequenceStep(string name, int sequenceNumber, int weight, bool vital, bool isIndividualCanceable)
        {
            if (Weight < 1) throw new ArgumentException("Weight cannot be negative or null");

            Name = name;
            SequenceNumber = sequenceNumber;
            Weight = weight;
            Vital = vital;
            IsIndividualCanceable = isIndividualCanceable;
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
        Func<IProgress<ProgressMessage>, CancellationToken, Task> Action;

        public AsynchronusStep(Func<IProgress<ProgressMessage>, CancellationToken, Task> action, string name, int sequenceNumber, int weight, bool vital, bool isIndividualCanceable)
            : base(name, sequenceNumber, weight, vital, isIndividualCanceable)
        {
            Action = action;
        }

        public override async Task Execute(IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            await Action(progress, ct);
        }
    }

    internal class SynchronusStep : SequenceStep
    {
        Action<IProgress<ProgressMessage>, CancellationToken> Action;

        public SynchronusStep(Action<IProgress<ProgressMessage>, CancellationToken> action, string name,
                              int sequenceNumber, int weight, bool vital, bool isIndividualCanceable)
            : base(name, sequenceNumber, weight, vital, isIndividualCanceable)
        {
            Action = action;
        }

        public override async Task Execute(IProgress<ProgressMessage> progress, CancellationToken ct)
        {
            await Task.Run(() => Action(progress, ct));
        }
    }

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
                                          bool canBeCanceled, Action cancel = null)
            : base(totalStepCount, currStepNum, stepName, stepId)
        {
            CanBeCanceled = canBeCanceled;
            Cancel = cancel ?? (() => { });
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

}
