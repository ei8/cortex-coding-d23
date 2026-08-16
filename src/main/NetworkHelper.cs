using ei8.Cortex.Coding.Mirrors;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ei8.Cortex.Coding.d23
{
    // TODO: Use C# 14+ Extension Members and attach to Neuron and Terminal classes
    public static class NetworkHelper
    {
        public enum AdditionalInputNeuronType
        {
            Or,
            And
        }

        public static bool TryCreateNeuron(
            [NotNullWhen(true)] out Neuron? result,
            [CallerArgumentExpression(nameof(result))] string parameterExpression = ""
        )
        {
            bool bResult = false;
            result = null;
            if (VariableInfo.TryParse(parameterExpression, out var variable))
            {
                result = NetworkHelper.CreateNeuron(variable.Inputs.Single());
                bResult = true;
            }

            return bResult;
        }


        public static Neuron CreateNeuron(string? tag = null) =>
            Neuron.CreateTransient(Guid.NewGuid(), tag, null, null);

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons(params Neuron[] postsynapticNeurons) =>
            NetworkHelper.CreateInterneuronNetworkByOutputNeurons(null, postsynapticNeurons);

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons(string? interneuronTag = null, params Neuron[] postsynapticNeurons)
        {
            var network = new Network();
            Neuron neuron = NetworkHelper.CreateNeuron(interneuronTag);
            network.AddReplace(neuron);

            foreach (var post in postsynapticNeurons)
                network.AddReplace(NetworkHelper.CreateTerminal(neuron, post));

            return network;
        }

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworksByOutputNeurons(
            IEnumerable<Neuron> outputs,
            IEnumerable<string> outputInterneuronTags
        ) =>
        [
            ..outputs.Select(o => {
                var index = outputs.ToList().IndexOf(o);
                return NetworkHelper.CreateInterneuronNetworkByOutputNeurons(outputInterneuronTags.ElementAt(index), outputs.ElementAt(index));
            })
        ];

        public static ReadOnlyNetwork LinkInputNeuronsToInterneuron
        (
            Neuron interneuron,
            IEnumerable<NeuronInfo> inputNeuronInfos,
            AdditionalInputNeuronType additionalInputNeuronType = AdditionalInputNeuronType.And,
            params NeuronInfo[] additionalInputNeuronInfos
        )
        {
            var network = new Network();
            NetworkHelper.LinkInputNeuronsToInterneuronByEffect
            (
                interneuron,
                inputNeuronInfos,
                network,
                NeurotransmitterEffect.Excite,
                additionalInputNeuronType,
                additionalInputNeuronInfos
            );
            NetworkHelper.LinkInputNeuronsToInterneuronByEffect
            (
                interneuron,
                inputNeuronInfos,
                network,
                NeurotransmitterEffect.Inhibit,
                additionalInputNeuronType,
                additionalInputNeuronInfos
            );
            return network;
        }

        private static void LinkInputNeuronsToInterneuronByEffect
        (
            Neuron interneuron,
            IEnumerable<NeuronInfo> inputNeuronInfos,
            Network network,
            NeurotransmitterEffect effect,
            AdditionalInputNeuronType additionalInputNeuronType,
            NeuronInfo[] additionalInputNeuronInfos
        )
        {
            var inputNeurons = inputNeuronInfos
                .Concat(additionalInputNeuronInfos)
                .Where(i => i.Effect == effect);

            var strengthDivisor = additionalInputNeuronType == AdditionalInputNeuronType.And ?
                inputNeurons.Count() :
                inputNeuronInfos.Count() + (additionalInputNeuronInfos.Length > 0 ? 1 : 0);

            foreach (var input in inputNeurons)
                network.AddReplace(NetworkHelper.CreateTerminal
                    (
                        input.Neuron, 
                        interneuron, 
                        input.Effect, 
                        input.Strength / strengthDivisor
                    )
                );
        }

        public static ReadOnlyNetwork CreateInputNeuronNetwork(MirrorConfig mirrorConfig, float strengthToInterneurons, params ReadOnlyNetwork[] interneurons) =>
            NetworkHelper.CreateInputNeuronNetwork(mirrorConfig, strengthToInterneurons, [.. interneurons.Select(i => i.GetInterneuron())]);

        public static ReadOnlyNetwork CreateInputNeuronNetwork(MirrorConfig mirrorConfig, float strengthToInterneurons, params Neuron[] interneurons)
        {
            AssertionConcern.AssertArgumentNotNull(mirrorConfig, nameof(mirrorConfig));

            var result = new Network();
            var inputNeuron = NetworkHelper.CreateNeuron(mirrorConfig);
            result.AddReplace(inputNeuron);

            foreach (var interneuron in interneurons)
                result.AddReplace(NetworkHelper.CreateTerminal(inputNeuron, interneuron, NeurotransmitterEffect.Excite, strengthToInterneurons));

            return result;
        }

        public static Neuron CreateNeuron(
            MirrorConfig mirrorConfig
        )
        {
            AssertionConcern.AssertArgumentNotNull(mirrorConfig, nameof(mirrorConfig));

            return Neuron.CreateTransient(Guid.NewGuid(), string.Join(',', mirrorConfig.Keys), mirrorConfig.Url, null);
        }

        public static Neuron CreateNeuron() =>
            Neuron.CreateTransient(Guid.NewGuid(), null, null, null);

        public static Terminal CreateTerminal(
            Neuron presynapticNeuron,
            Neuron postsynapticNeuron
        ) => NetworkHelper.CreateTerminal(presynapticNeuron, postsynapticNeuron, NeurotransmitterEffect.Excite, 1f);

        public static Terminal CreateTerminal(
            Neuron presynapticNeuron,
            Neuron postsynapticNeuron,
            NeurotransmitterEffect effect,
            float strength
        ) => new(Guid.NewGuid(), presynapticNeuron.Id, postsynapticNeuron.Id, effect, strength);
    }
}