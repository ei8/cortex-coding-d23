using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceReader : IInstanceReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IPropertyValueAssociationReader propertyValueAssociationReader;
        private readonly IPropertyInstanceValueAssociationReader propertyInstanceValueAssociationReader;

        public InstanceReader(
            IInstantiatesClassReader instantiatesClassReader,
            IPropertyValueAssociationReader propertyValueAssociationReader,
            IPropertyInstanceValueAssociationReader propertyInstanceValueAssociationReader
        )
        {
            this.instantiatesClassReader = instantiatesClassReader;
            this.propertyValueAssociationReader = propertyValueAssociationReader;
            this.propertyInstanceValueAssociationReader=propertyInstanceValueAssociationReader;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IPropertyValueAssociationReader propertyValueAssociationReader,
            IPropertyInstanceValueAssociationReader propertyInstanceValueAssociationReader,
            IInstanceParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IInstance>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                    instantiatesClassReader,
                    () => InstanceReader.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            }.Concat(
                parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                    u => new IndependentGreatGrannyInfo<IPropertyValueAssociation, IPropertyValueAssociationReader, IPropertyValueAssociationParameterSet, IInstance>(
                    propertyValueAssociationReader,
                    () => u,
                    (g, r) => r.PropertyAssociations.Add(g)
                    )
                )
            ).Concat(
                parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                    u => new IndependentGreatGrannyInfo<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationReader, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                    propertyInstanceValueAssociationReader,
                    () => u,
                    (g, r) => r.PropertyAssociations.Add(g)
                    )
                )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IInstanceParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet>(
                    this.instantiatesClassReader,
                    (n) => CreateInstantiatesClassParameterSet(parameters)
                )
            }.Concat(
                // create GrannyQueryInner for each PropertyAssociation in parameters
                parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                    pa => new GreatGrannyQuery<IPropertyValueAssociation, IPropertyValueAssociationReader, IPropertyValueAssociationParameterSet>(
                        this.propertyValueAssociationReader,
                        (n) => pa
                    )
                )
            ).Concat(
                // create GrannyQueryInner for each PropertyAssociation in parameters
                parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                    pa => new GreatGrannyQuery<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationReader, IPropertyInstanceValueAssociationParameterSet>(
                        this.propertyInstanceValueAssociationReader,
                        (n) => pa
                    )
                )
            ).Concat(
                new IGrannyQuery[] {
                    // ... use Granny neurons of PropertyAssociations in PostsynapticIds along with id of InstantiatesClass
                    new GrannyQueryBuilder(
                        (n) => new NeuronQuery()
                        {
                            DirectionValues = DirectionValues.Outbound,
                            Depth = 1,
                            Postsynaptic = n.Select(ne => ne.Neuron.Id.ToString())
                        }
                    )
                }
            );

        public bool TryParse(Network network, IInstanceParameterSet parameters, out IInstance result) =>
            this.TryParseAggregate(
                () => new Instance(),
                parameters,
                InstanceReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    this.propertyValueAssociationReader,
                    this.propertyInstanceValueAssociationReader,
                    parameters
                ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyValueAssociation, IPropertyValueAssociationReader, IPropertyValueAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParse
                        )
                    )
                ).Concat(
                    parameters.PropertyAssociationsParameters.OfType<IPropertyInstanceValueAssociationParameterSet>().Select(
                        u => new GreatGrannyProcess<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationReader, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParse
                        )
                    )
                ),
                network,
                out result
            );
    }
}
