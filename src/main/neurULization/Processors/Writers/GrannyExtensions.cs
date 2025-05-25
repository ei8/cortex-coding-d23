using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public static class GrannyExtensions
    {
        /// <summary>
        /// Retrieves granny from network if present, otherwise, builds and adds it to the network.
        /// </summary>
        /// <typeparam name="TGranny"></typeparam>
        /// <typeparam name="TParameterSet"></typeparam>
        /// <param name="grannyWriter"></param>
        /// <param name="network"></param>
        /// <param name="options"></param>
        /// <param name="writeParameters"></param>
        /// <returns></returns>
        public static bool TryParseBuild<TGranny, TGrannyWriter, TParameterSet>(
            this TGrannyWriter grannyWriter,
            Network network,
            TParameterSet writeParameters,
            out TGranny result
        )
            where TGranny : IGranny
            where TGrannyWriter : IGrannyWriter<TGranny, TParameterSet>
            where TParameterSet : IDeductiveParameterSet
        {
            AssertionConcern.AssertArgumentNotNull(network, nameof(network));
            AssertionConcern.AssertArgumentNotNull(writeParameters, nameof(writeParameters));

            result = default;
            var bResult = grannyWriter.Reader.TryParse(
                network,
                writeParameters,
                out result
            );
            // if target is not in specified network
            if (!bResult)
                // build in network
                bResult = grannyWriter.TryBuild(
                    network,
                    writeParameters,
                    out result
                );

            return bResult;
        }

        internal static bool TryBuildAggregate<TWriter, TParameterSet, TResult>(
            this TWriter grannyWriter,
            Func<TResult> resultConstructor,
            TParameterSet parameters,
            IEnumerable<IGreatGrannyProcess<TResult>> targets,
            Network network,
            IMirrorSet mirrors,
            out TResult result,
            Func<Neuron> grannyNeuronCreator = null,
            Func<TResult, IEnumerable<PostsynapticInfo>> postsynapticsRetriever = null
        )
            where TResult : IGranny
            where TParameterSet : IDeductiveParameterSet
            where TWriter : ILesserGrannyWriter<TResult, TParameterSet>
        {
            var bResult = false;
            result = default;

            try
            {
                GrannyExtensions.Log($"Building aggregate '{typeof(TResult).Name}'...");

                var tempResult = resultConstructor();
                IGranny precedingGranny = null;
                var ts = targets.ToArray();

                if (grannyWriter.TryCreateGreatGrannies(
                    parameters,
                    network,
                    mirrors, 
                    out IEnumerable<IGreatGrannyInfo<TResult>> processes
                ))
                {
                    for (int i = 0; i < ts.Length; i++)
                    {
                        var candidate = processes.ElementAt(i);

                        if (
                            ts[i].TryGetParameters(
                                precedingGranny,
                                candidate,
                                out IParameterSet executionParameters
                            ) &&
                            ts[i].TryExecute(
                                candidate,
                                network,
                                tempResult,
                                executionParameters,
                                out precedingGranny
                            )
                        )
                            GrannyExtensions.Log(
                                $"Target {(i + 1)}/{ts.Length} execution successful."
                            );
                    }

                    if (precedingGranny != null)
                    {
                        tempResult.Neuron = grannyNeuronCreator != null ?
                            grannyNeuronCreator() :
                            precedingGranny.Neuron;

                        IEnumerable<PostsynapticInfo> postsynaptics = null;
                        if (
                            postsynapticsRetriever != null &&
                            (postsynaptics = postsynapticsRetriever(tempResult)) != null &&
                            postsynaptics.Any()
                            )
                        {
                            postsynaptics.ToList().ForEach(ps =>
                            {
                                GrannyExtensions.Log(
                                    $"Linking postsynaptic: {ps.Neuron.Id} - '{ps.Neuron.Tag}'" +
                                    $"{(!string.IsNullOrEmpty(ps.Name) ? $" ({ps.Name})" : string.Empty)}"
                                );

                                network.AddReplace(
                                    Terminal.CreateTransient(
                                        tempResult.Neuron.Id, ps.Neuron.Id
                                    )
                                );
                            }
                            );
                        }

                        GrannyExtensions.Log($"DONE... Id: {tempResult.Neuron.Id}");

                        result = tempResult;
                        bResult = true;
                    }
                }
            }
            catch(Exception ex)
            {
                GrannyExtensions.Log($"Error: {ex}");
            }

            return bResult;
        }

        [Conditional("BUILDLOG")]
        internal static void Log(string message, int indent = 0, bool writeLine = true)
        {
            var logMessage = $"{AggregateParser.GetPrefix(GrannyExtensions.Counter, indent)}{message}";

            if (writeLine)
                Debug.WriteLine(logMessage);
            else
                Debug.Write(logMessage);
        }

        private static int Counter(int indent)
        {
            var st = new StackTrace().ToString();
            var rootIndentation = 1;
            var patternCount = Regex.Matches(st, nameof(TryBuildAggregate)).Count;
            return patternCount + indent - rootIndentation;
        }

        internal static PostsynapticInfo ToPostsynapticInfo<T, TProp>(
            this T granny, 
            IGranny value,
            Expression<Func<T, TProp>> propertySelector
        )
            where T : IGranny
        {
            var result = new PostsynapticInfo()
            {
                Name = string.Empty,
                Neuron = value.Neuron
            };

            GrannyExtensions.SetPropertyName(result, propertySelector);

            return result;
        }

        internal static IEnumerable<PostsynapticInfo> ToPostsynapticInfos<T, TProp, TItem>(
            this T granny,
            IList<TItem> values,
            Expression<Func<T, TProp>> propertySelector
        )
            where T : IGranny
            where TProp : IList<TItem>
            where TItem : IGranny
        {
            var result = values.Select(g => new PostsynapticInfo()
            {
                Name = string.Empty,
                Neuron = g.Neuron
            }
            ).ToList();

            GrannyExtensions.SetPropertyName(result, propertySelector);

            return result;
        }

        [Conditional("BUILDLOG")]
        private static void SetPropertyName<T, TProp>(
            PostsynapticInfo pi, 
            Expression<Func<T, TProp>> propertySelector
        )
        {
            MemberExpression body = (MemberExpression)propertySelector.Body;
            pi.Name = body.Member.Name;
        }

        [Conditional("BUILDLOG")]
        private static void SetPropertyName<T, TProp>(
            IList<PostsynapticInfo> pis,
            Expression<Func<T, TProp>> propertySelector
        )
        {
            MemberExpression body = (MemberExpression)propertySelector.Body;
            foreach (var pi in pis)
                pi.Name = body.Member.Name;
        }
    }
}
