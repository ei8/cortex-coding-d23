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
        public enum InputNeuronStrengthMode
        {
            Or,
            And,
            Manual
        }

        public static IEnumerable<ReadOnlyNetwork> ConvertToNetworks(params IneurUL?[] neurULs) =>
            neurULs.WhereNotNull().Select(n => n.Network);

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
            NetworkHelper.CreateInterneuronNetworkByOutputNeurons
            (
                NeurotransmitterEffect.Excite,
                1f,
                postsynapticNeurons
            );

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons
        (
            NeurotransmitterEffect effect,
            float strength, 
            params Neuron[] postsynapticNeurons
        ) =>
            NetworkHelper.CreateInterneuronNetworkByOutputNeurons(null, effect, strength, postsynapticNeurons);

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons
        (
            string? interneuronTag,
            params Neuron[] postsynapticNeurons
        ) =>
            NetworkHelper.CreateInterneuronNetworkByOutputNeurons(interneuronTag, NeurotransmitterEffect.Excite, 1f, postsynapticNeurons);

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons
        (
            string? interneuronTag,
            float strength,
            params Neuron[] postsynapticNeurons
        )
            =>
            NetworkHelper.CreateInterneuronNetworkByOutputNeurons(interneuronTag, NeurotransmitterEffect.Excite, strength, postsynapticNeurons);

        public static ReadOnlyNetwork CreateInterneuronNetworkByOutputNeurons
        (
            string? interneuronTag, 
            NeurotransmitterEffect effect, 
            float strength, 
            params Neuron[] postsynapticNeurons
        )
        {
            var network = new Network();
            Neuron neuron = NetworkHelper.CreateNeuron(interneuronTag);
            network.AddReplace(neuron);

            foreach (var post in postsynapticNeurons)
                network.AddReplace(NetworkHelper.CreateTerminal(neuron, post, effect, strength));

            return network;
        }

        public static IEnumerable<ReadOnlyNetwork> CreateInterneuronNetworksByOutputNeurons(
            IEnumerable<Neuron> outputs,
            IEnumerable<string> outputInterneuronTags,
            NeurotransmitterEffect effect = NeurotransmitterEffect.Excite,
            float strength = 1f
        ) =>
        [
            ..outputs.Select(o => {
                var index = outputs.ToList().IndexOf(o);
                return NetworkHelper.CreateInterneuronNetworkByOutputNeurons
                (
                    outputInterneuronTags.ElementAt(index),
                    effect,
                    strength,
                    outputs.ElementAt(index)
                );
            })
        ];

        public static ReadOnlyNetwork LinkInputNeuronsToInterneuron
        (
            Neuron interneuron,
            IEnumerable<NeuronInfo> inputNeuronInfos,
            InputNeuronStrengthMode additionalInputNeuronStrengthMode = InputNeuronStrengthMode.And,
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
                additionalInputNeuronStrengthMode,
                additionalInputNeuronInfos
            );
            NetworkHelper.LinkInputNeuronsToInterneuronByEffect
            (
                interneuron,
                inputNeuronInfos,
                network,
                NeurotransmitterEffect.Inhibit,
                additionalInputNeuronStrengthMode,
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
            InputNeuronStrengthMode additionalInputNeuronStrengthMode,
            NeuronInfo[] additionalInputNeuronInfos
        )
        {
            var inputNeurons = inputNeuronInfos
                .Concat(additionalInputNeuronInfos)
                .Where(i => i.Effect == effect);

            foreach (var input in inputNeurons)
            {
                float strength = input.Strength;

                if (additionalInputNeuronStrengthMode != InputNeuronStrengthMode.Manual)
                {
                    var strengthDivisor = additionalInputNeuronStrengthMode == InputNeuronStrengthMode.And ?
                    inputNeurons.Count() :
                    inputNeuronInfos.Count() + (additionalInputNeuronInfos.Length > 0 ? 1 : 0);
                    strength =  input.Strength / strengthDivisor;
                }

                network.AddReplace(
                    NetworkHelper.CreateTerminal
                    (
                        input.Neuron,
                        interneuron,
                        input.Effect,
                        strength
                    )
                );
            }
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

        public static Terminal CreateTerminal
        (
            Neuron presynapticNeuron,
            Neuron postsynapticNeuron,
            NeurotransmitterEffect effect,
            float strength
        ) => 
            new
            (
                Guid.NewGuid(), 
                presynapticNeuron.Id, 
                postsynapticNeuron.Id, 
                effect, strength
            );

        public static IEnumerable<Terminal> CreateTerminals
        (
            IEnumerable<Neuron> presynapticNeurons, 
            IEnumerable<Neuron> postsynapticNeurons, 
            NeurotransmitterEffect effect, 
            float strength
        )
        {
            var result = new List<Terminal>();
            foreach (var presynapticNeuron in presynapticNeurons)
                foreach (var postsynapticNeuron in postsynapticNeurons)
                    result.Add(NetworkHelper.CreateTerminal(presynapticNeuron, postsynapticNeuron, effect, strength));

            return result;
        }
    }
}