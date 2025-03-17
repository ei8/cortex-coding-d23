using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
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

        public InstanceWriter(
            IInstantiatesClassWriter instantiatesClassWriter, 
            IPropertyValueAssociationWriter propertyValueAssociationWriter,
            IPropertyInstanceValueAssociationWriter propertyInstanceValueAssociationWriter,
            Readers.Deductive.IInstanceReader reader            
        )
        {
            this.instantiatesClassWriter = instantiatesClassWriter;
            this.propertyValueAssociationWriter = propertyValueAssociationWriter;
            this.propertyInstanceValueAssociationWriter = propertyInstanceValueAssociationWriter;
            this.reader = reader;
        }

        public IInstance Build(Network network, IInstanceParameterSet parameters) =>
            new Instance().AggregateBuild(
                InstanceWriter.CreateGreatGrannies(
                    this.instantiatesClassWriter,
                    this.propertyValueAssociationWriter,
                    this.propertyInstanceValueAssociationWriter,
                    parameters
                ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassWriter, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.ParseBuild
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyValueAssociation, IPropertyValueAssociationWriter, IPropertyValueAssociationParameterSet, IInstance>(
                            ProcessHelper.ParseBuild
                        )
                    )
                ).Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationWriter, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                            ProcessHelper.ParseBuild
                        )
                    )
                ),
                network,
                () =>
                {
                    Neuron result = network.AddOrGetIfExists(
                        Neuron.CreateTransient(
                            parameters.Id,
                            parameters.Tag,
                            parameters.ExternalReferenceUrl,
                            parameters.RegionId
                        )
                        // TODO: no longer necessary?
                        // writeOptions.OperationOptions.Mode == WriteMode.Update
                    );
                    return result;
                },
                (r) => new[]
                {
                    r.ToPostsynapticInfo(r.InstantiatesClass, g => g.InstantiatesClass)
                }.Concat(
                    // with PropertyAssociations in result
                    r.ToPostsynapticInfos(r.PropertyAssociations, g => g.PropertyAssociations)
                )
            );

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassWriter instantiatesClassWriter,
            IPropertyValueAssociationWriter propertyValueAssociationWriter, 
            IPropertyInstanceValueAssociationWriter propertyInstanceValueAssociationWriter,
            IInstanceParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IInstance>[]
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


        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IGrannyReader<IInstance, IInstanceParameterSet> Reader => this.reader;
    }
}
