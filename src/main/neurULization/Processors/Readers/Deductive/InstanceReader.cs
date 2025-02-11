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
        private readonly IPropertyAssociationReader propertyAssociationReader;

        public InstanceReader(
            IInstantiatesClassReader instantiatesClassReader, 
            IPropertyAssociationReader propertyAssociationReader
        )
        {
            this.instantiatesClassReader = instantiatesClassReader;
            this.propertyAssociationReader = propertyAssociationReader;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IPropertyAssociationReader propertyAssociationReader,
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
                parameters.PropertyAssociationsParameters.Select(
                    u => new IndependentGreatGrannyInfo<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet, IInstance>(
                    propertyAssociationReader,
                    () => u,
                    (g, r) => r.PropertyAssociations.Add(g)
                    )
                )
            );


        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IEnumerable<IGrannyQuery> GetQueries(IInstanceParameterSet parameters) =>
            new IGrannyQuery[] {
                new GreatGrannyQuery<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet>(
                    this.instantiatesClassReader,
                    (n) => CreateInstantiatesClassParameterSet(parameters)
                )
            }.Concat(
                // create GrannyQueryInner for each PropertyAssociation in parameters
                parameters.PropertyAssociationsParameters.Select(
                    pa => new GreatGrannyQuery<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet>(
                        this.propertyAssociationReader,
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
            new Instance().AggregateTryParse(
                InstanceReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    this.propertyAssociationReader,
                    parameters
                ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParse
                        )
                    )
                ),
                network,
                out result
            );
    }
}
