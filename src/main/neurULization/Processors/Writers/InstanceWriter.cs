using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceWriter : IInstanceWriter
    {
        private readonly IInstantiatesClassWriter instantiatesClassWriter;
        private readonly IPropertyValueAssociationWriter propertyValueAssociationWriter;
        private readonly IPropertyInstanceValueAssociationWriter propertyInstanceValueAssociationWriter;
        private readonly Readers.Deductive.IInstanceReader reader;
        private readonly IMirrorSet mirrors;

        public InstanceWriter(
            IInstantiatesClassWriter instantiatesClassWriter, 
            IPropertyValueAssociationWriter propertyValueAssociationWriter,
            IPropertyInstanceValueAssociationWriter propertyInstanceValueAssociationWriter,
            Readers.Deductive.IInstanceReader reader,
            IMirrorSet mirrors
        )
        {
            AssertionConcern.AssertArgumentNotNull(instantiatesClassWriter, nameof(instantiatesClassWriter));
            AssertionConcern.AssertArgumentNotNull(propertyValueAssociationWriter, nameof(propertyValueAssociationWriter));
            AssertionConcern.AssertArgumentNotNull(propertyInstanceValueAssociationWriter, nameof(propertyInstanceValueAssociationWriter));
            AssertionConcern.AssertArgumentNotNull(reader, nameof(reader));
            AssertionConcern.AssertArgumentNotNull(mirrors, nameof(mirrors));

            this.instantiatesClassWriter = instantiatesClassWriter;
            this.propertyValueAssociationWriter = propertyValueAssociationWriter;
            this.propertyInstanceValueAssociationWriter = propertyInstanceValueAssociationWriter;
            this.reader = reader;
            this.mirrors = mirrors;
        }

        public bool TryBuild(Network network, IInstanceParameterSet parameters, out IInstance result) =>
            this.TryBuildAggregate(
                () => new Instance(),
                parameters,
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParseBuild
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyValueAssociation, IPropertyValueAssociationWriter, IPropertyValueAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParseBuild
                        )
                    )
                ).Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationWriter, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParseBuild
                        )
                    )
                ),
                network,
                this.mirrors,
                out result,
                () =>
                {
                    Neuron netResult = network.AddOrGetIfExists(
                        Neuron.CreateTransient(
                            parameters.Id,
                            parameters.Tag,
                            parameters.MirrorUrl,
                            parameters.RegionId
                        )
                        // TODO: no longer necessary?
                        // writeOptions.OperationOptions.Mode == WriteMode.Update
                    );
                    return netResult;
                },
                (r) => new[]
                {
                    r.ToPostsynapticInfo(r.InstantiatesClass, g => g.InstantiatesClass)
                }.Concat(
                    // with PropertyAssociations in result
                    r.ToPostsynapticInfos(r.PropertyAssociations, g => g.PropertyAssociations)
                )
            );

        public bool TryCreateGreatGrannies(
            IInstanceParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<IInstance>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = new IGreatGrannyInfo<IInstance>[]
                {
                    new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstance>(
                        instantiatesClassWriter,
                        () => InstanceWriter.CreateInstantiatesClassParameterSet(parameters),
                        (g, r) => r.InstantiatesClass = g
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                        u => new IndependentGreatGrannyInfo<IPropertyValueAssociation, IPropertyValueAssociationWriter, IPropertyValueAssociationParameterSet, IInstance>(
                        propertyValueAssociationWriter,
                        () => u,
                        (g, r) => r.PropertyAssociations.Add(g)
                        )
                    )
                ).Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                        u => new IndependentGreatGrannyInfo<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationWriter, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                        propertyInstanceValueAssociationWriter,
                        () => u,
                        (g, r) => r.PropertyAssociations.Add(g)
                        )
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IGrannyReader<IInstance, IInstanceParameterSet> Reader => this.reader;
    }
}
