using ei8.Cortex.Coding.d23.Process;
using ei8.Cortex.Coding.d23.Process.Operation;
using ei8.Cortex.Coding.Mirrors;
using ei8.Cortex.Coding.Model.Reflection;
using ei8.Cortex.Coding.Spiker;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ei8.Cortex.Coding.d23
{
    using System.Linq;

    public static class ExtensionMethods
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static ReadOnlyNetwork ToNetwork(this IEnumerable<INetworkItem> items)
        {
            var result = new Network();
            result.AddReplaceItems(items);
            return result;
        }

        public static ReadOnlyNetwork Combine(this IEnumerable<ReadOnlyNetwork> networks)
        {
            var result = new Network();
            result.AddReplaceItems([.. networks]);
            return result;
        }

        public static ReadOnlyNetwork Combine(this IEnumerable<IneurUL> neurULs)
        {
            var result = new Network();
            result.AddReplaceItems([.. neurULs]);
            return result;
        }

        public static void AddReplaceItems(this Network original, params IneurUL[] neurULs) =>
            original.AddReplaceItems([.. neurULs.Select(no => no.Network)]);

        public static void AddReplaceItems(this Network original, params ReadOnlyNetwork[] networks)
        {
            foreach (var n in networks)
                original.AddReplaceItems(n);
        }

        public static Neuron GetInterneuron(this ReadOnlyNetwork value) => value.GetItems<Neuron>().Single();

        public static bool TryParseSensoryNeurons(
            this ISpikable spikable,
            IEnumerable<StimulusParser> parsers,
            [NotNullWhen(true)] out IEnumerable<ParseSensorResult>? result,
            params StimulusInfo[] stimuli
        )
        {
            var bResult = false;
            result = null;
            var tempList = new List<ParseSensorResult>();

            foreach (var stimulus in stimuli)
            {
                StimulusParser? parser = null;
                if (
                    (
                        parser = parsers
                            .Where(p => p.Type == stimulus.Type)
                            .SingleOrDefault(p => p.Evaluator(stimulus.Value))
                    ) != null
                )
                    tempList.Add(new(stimulus.Value, parser.NeuronConverter(stimulus.Value)));
            }

            if (tempList.Count == stimuli.Length)
            {
                bResult = true;
                result = tempList;
            }

            return bResult;
        }

        private static readonly object parseLock = new();

        /// <summary>
        /// This might be a temporary approach. 
        /// Ideally, the fired neurons for a method and its parameters
        /// should be retrieved via mirrors if necessary, deneurULized, cached and invoked accordingly. 
        /// eg. Rotate Method (granny), Clockwise Direction Parameter (granny), 22.5 Float Degrees Parameter (granny)
        /// Using Method (class), MethodParameter (class)
        /// </summary>
        /// <param name="spikable"></param>
        /// <param name="currentFire"></param>
        /// <param name="responseParsers"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParseMotorNeurons(
            this ISpikable spikable,
            FireInfo currentFire,
            IEnumerable<ResponseParser> responseParsers,
            [NotNullWhen(true)] out IEnumerable<ParseMotorResult>? result
        )
        {
            bool bResult = false;
            result = null;
            var tempResults = new List<ParseMotorResult>();

            lock (ExtensionMethods.parseLock)
            {
                var fireHistory = (ConcurrentDictionary<DateTime, FireInfo>)spikable.FireHistory;
                fireHistory.Clean(currentFire.Timestamp.Subtract(spikable.RelatedSpikesPeriod));
                fireHistory.TryAdd(currentFire.Timestamp, currentFire);

                // if last fired is one of the anticipated neurons
                // TODO: anticipated neurons can include instantiates grannies (eg. instantiates^methodParameter) to optimize recognition,
                // ie. no need to recognize all possible values
                foreach (var rp in responseParsers)
                    if (currentFire.Target.Id == rp.ActionNeuronId || rp.Evaluator(currentFire))
                    {
                        if (fireHistory.Count >= ExtensionMethods.GetExpectedResultCount(rp))
                        {
                            var matchedActionNeurons = fireHistory.Select(fh => fh.Value).Where(n => n.Target.Id == rp.ActionNeuronId);

                            if (matchedActionNeurons.Count() > 1)
                                ExtensionMethods.logger.Warn(
                                    new LogMessageGenerator(
                                        () =>
                                            $"{matchedActionNeurons.First().Target.ToReadableString()} fired more than once within related spikes period ({spikable.RelatedSpikesPeriod.TotalMilliseconds} ms): " +
                                            $"{string.Join(", ", matchedActionNeurons.Select(man => man.Timestamp.TimeOfDay.TotalMilliseconds))}"
                                    )
                                );

                            var actionFireInfo = matchedActionNeurons.FirstOrDefault();
                            // and specified method was fired within relative spikes period
                            if (actionFireInfo != null)
                            {
                                tempResults.Add(new(actionFireInfo.Target, actionFireInfo, actionFireInfo.Target.Tag));
                                foreach (var pc in rp.ParameterConverters)
                                {
                                    foreach (var fi in fireHistory.Values)
                                    {
                                        if (pc.Invoke(fi, out object? objectResult))
                                        {
                                            tempResults.Add(new(fi.Target, fi, objectResult));

                                            if (tempResults.Count == ExtensionMethods.GetExpectedResultCount(rp))
                                            {
                                                fireHistory.Clear();
                                                bResult = true;
                                                result = tempResults;
                                            }
                                            break;
                                        }
                                    }

                                    if (bResult)
                                        break;
                                }
                            }
                        }
                    }
            }

            return bResult;
        }

        private static int GetExpectedResultCount(ResponseParser responseParser) =>
            // if number of related fires equals eg. 2 parameters + 1 method
            responseParser.ParameterConverters.Count() + 1;
    }
}

namespace ei8.Cortex.Coding.d23
{
    public static class PerfExtensionMethods
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static string LogFire(Neuron n) => $"Fired: {n.ToReadableString()}";

        public static void Log(this IProcess process, Logger logger, LogLevel level, LogMessageGenerator logMessageGenerator) =>
            logger.Log(level, logMessageGenerator);

        public static T[] WhereNotNull<T>(this T?[] array) where T : class
        {
            var result = new List<T>();
            foreach (var item in array)
                if (item != null)
                    result.Add(item);

            return [.. result];
        }

        public static string ToReadableString(this Neuron neuron)
        {
            return $"{neuron.Id}:'{neuron.Tag}'";
        }

        public static bool TryGetFiredParameter<T>(this FireInfo fireInfo, IDictionary<Guid, T> paramValueMap, out object? parameter)
        {
            parameter = null;

            if (paramValueMap.TryGetValue(fireInfo.Target.Id, out T? value))
                parameter = value;

            return parameter != null;
        }

        // TODO: Use Adder to:
        // 1. Store current digit info
        // 2. Retrieve next digit value by looping from first digit to current digit
        // 3. Use Equal (Operation-TBD) as condition in DoUntil  to determine if current Digit Index is equal to current digit info
        /// <summary>
        /// Gets adjacent item in an array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemArray"></param>
        /// <param name="currentItem"></param>
        /// <param name="ascending">A value indicating whether retrieval direction is ascending (lower to higher index) or otherwise.</param>
        /// <returns></returns>
        internal static T? GetAdjacentOrDefault<T>(this T[] itemArray, T? currentItem, bool ascending = true)
            where T : class
        {
            T? adjacentItem = default;

            if (currentItem != null)
            {
                if (itemArray.TryGetIndexOf(currentItem, out var currentIndex))
                {
                    if (ascending)
                    {
                        if (currentIndex < itemArray.Length - 1)
                        {
                            adjacentItem = itemArray[currentIndex.Value + 1];
                        }
                    }
                    else
                    {
                        if (currentIndex > 0)
                        {
                            adjacentItem = itemArray[currentIndex.Value - 1];
                        }
                    }
                }
                else
                    throw new ArgumentOutOfRangeException(nameof(currentItem));
            }

            return adjacentItem;
        }

        public static IDictionary<Guid, T> ConvertToNeuronValueMap<T>(this IEnumerable<T> values, IEnumerable<MirrorConfig> mirrorConfigs, Network network) where T : Enum
        {
            var result = new Dictionary<Guid, T>();

            foreach (var value in values)
            {
                if (
                    !mirrorConfigs.TryGetMirrorNeuron(
                        value.ToKeyString(),
                        network,
                        out Neuron? neuron
                    )
                )
                    throw new InvalidOperationException($"Failed retrieving NeuronValueMap for {value.ToKeyString()}");

                result.Add(neuron.Id, value);
            }

            return result;
        }

        public static bool CanHandle
        (
            this IResultChunk resultInfo,
            Neuron target
        )
        {
            foreach (var neuronChunk in resultInfo.DigitValues.Items)
                if (neuronChunk.Neuron == target)
                    return true;
            return false;
        }

        public static void HandleFire
        (
            this IResultChunk resultInfo,
            Neuron target
        )
        {
            resultInfo.Value.Add(new NeuronChunk(target));
            if (resultInfo is INamedChunk namedChunk)
            {
                PerfExtensionMethods.logger.Info(new LogMessageGenerator(() => $"Added to {namedChunk.Name}: {target.Tag}"));
            }
        }

        public static bool TryGetDigitValueByValueIndex
        (
            this IDigitValueArray digitValueArray,
            int index,
            [NotNullWhen(true)]
            out INeuronChunk? result
        )
        {
            ArgumentNullException.ThrowIfNull(digitValueArray.ValueArray);

            result = null;

            return digitValueArray.DigitValuesArray.TryGetDigitValueByValueIndex
            (
                digitValueArray.ValueArray.Length,
                index,
                (digitNeuron, valueNeuron) =>
                    digitNeuron.Tag.EndsWith(valueNeuron.Tag[^1]),
                digitNeuron => digitNeuron.Tag.EndsWith('0'),
                i => digitValueArray.ValueArray[i],
                out result
            );
        }

        public static bool TryGetDigitValueByValueIndex
        (
            this INeuronChunk[] digitValues,
            int valueLength,
            int index,
            Func<Neuron, Neuron, bool> matchComparer,
            Predicate<Neuron> noMatchComparer,
            Func<int, INeuronChunk> valueRetriever,
            [NotNullWhen(true)]
            out INeuronChunk? result
        )
        {
            result = null;

            if (index < valueLength)
                digitValues.TryGetValueByComparer
                (
                    digitNeuron =>
                        matchComparer
                        (
                            digitNeuron,
                            valueRetriever(index).Neuron
                        ),
                    out result
                );
            else
                digitValues.TryGetValueByComparer
                (
                    neuron => noMatchComparer(neuron),
                    out result
                );

            return result != null;
        }

        public static bool TryGetDigitValueByGroupedValueIndex
        (
            this INeuronChunk[] digitValues,
            IConcurrentListChunk<INeuronChunk> value,
            int index,
            [NotNullWhen(true)]
            out INeuronChunk? result
        )
        {
            result = null;

            return 
                (
                    index == 0 &&
                    digitValues.TryGetValueByComparer
                    (
                        n => n.Tag.EndsWith('0'),
                        out result
                    )
                ) ||
                (
                    index - 1 < value.Count &&
                    digitValues.TryGetDigitValueByValueIndex
                    (
                        value,
                        index - 1,
                        out result
                    )
                );
        }

        public static bool TryGetDigitValueByValueIndex
        (
            this INeuronChunk[] digitValues,
            IConcurrentListChunk<INeuronChunk> value,
            int index,
            [NotNullWhen(true)]
            out INeuronChunk? result
        )
        {
            return digitValues.TryGetDigitValueByValueIndex
                (
                    value.Count,
                    index,
                    (digitNeuron, valueNeuron) =>
                        digitNeuron.Tag.EndsWith(valueNeuron.Tag[^1]),
                    n => n.Tag.EndsWith('0'),
                    value.ItemAt,
                    out result
                );
        }

        public static bool TryGetValueByComparer
        (
            this INeuronChunk[] valueChunks,
            Predicate<Neuron> comparer,
            [NotNullWhen(true)]
            out INeuronChunk? result
        )
        {
            result = null;
            foreach (var chunk in valueChunks)
                if (comparer(chunk.Neuron))
                {
                    result = chunk;
                    break;
                }

            return result != null;
        }

        public static bool TryHandleAllResults
        (
            this ISyncOperationWorkingMemory workingMemory,
            Neuron target,
            TryFunc<int?> expectedHandledCountRetriever,
            out bool targetHandled
        )
        {
            var result = false;
            targetHandled = false;

            if (expectedHandledCountRetriever(out var expectedHandledCount))
            {
                var expectedResults = workingMemory.GetResults();
                IResultChunk? expectedResultTargetHandler = null;
                foreach (var expectedResult in expectedResults)
                    if (expectedResult.CanHandle(target))
                    {
                        expectedResultTargetHandler = expectedResult;
                        break;
                    }
                if
                (
                    expectedResultTargetHandler != null &&
                    expectedResultTargetHandler.Value.Count < expectedHandledCount
                )
                {
                    expectedResultTargetHandler.HandleFire(target);
                    targetHandled = true;
                }

                var handledResultsCount = 0;
                foreach (var expectedResult in expectedResults)
                    if (expectedResult.Value.Count == expectedHandledCount)
                        handledResultsCount++;

                result = handledResultsCount == expectedResults.Length;
            }

            return result;
        }

        public static bool TryGetCurrentDigitIndex
        (
            this IOperandChunk[] operands,
            [NotNullWhen(true)]
            out int? result
        )
        {
            result = null;

            foreach (var operand in operands)
            {
                if
                (
                    operand.TryGetCurrentDigitIndex(out var index) &&
                    (result == null || index > result)
                )
                    result = index;
            }

            return result.HasValue;
        }

        public static int GetExpectedHandledDigitsCount
        (
            this IOperandChunk[] operands,
            int operandsCurrentIndex,
            bool incremental
        )
        {
            int result = 0;

            if (incremental)
                result = operandsCurrentIndex + 1;
            else
            {
                var maxDigitCount = int.MinValue;

                foreach (var operand in operands)
                    if (operand.ValueArray != null && operand.ValueArray.Length > maxDigitCount)
                        maxDigitCount = operand.ValueArray.Length;

                result = maxDigitCount - operandsCurrentIndex;
            }

            return result;
        }

        public static bool TryGetIndexOf<T>
        (
            this T[]? array,
            T? value,
            [NotNullWhen(true)]
            out int? index
        )
        {
            index = null;
            if (array != null)
                index =
                    Array.IndexOf
                    (
                        array,
                        value
                    );

            return index != null;
        }

        public static TResult[] Select<T, TResult>
        (
            this T[] array,
            Func<T, TResult> selector
        )
        {
            var result = new List<TResult>();

            foreach (var item in array)
                result.Add(selector(item));

            return [.. result];
        }
    }
}