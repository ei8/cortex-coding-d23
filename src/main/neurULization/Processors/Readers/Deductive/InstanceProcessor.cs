using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstanceProcessor : IInstanceProcessor
    {
        private readonly IInstantiatesClassProcessor instantiatesClassProcessor;
        private readonly IPropertyAssociationProcessor propertyAssociationProcessor;

        public InstanceProcessor(IInstantiatesClassProcessor instantiatesClassProcessor, IPropertyAssociationProcessor propertyAssociationProcessor)
        {
            this.instantiatesClassProcessor = instantiatesClassProcessor;
            this.propertyAssociationProcessor = propertyAssociationProcessor;
        }

        private IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(Id23neurULizerWriteOptions options, IInstanceParameterSet parameters) =>
            new IGreatGrannyInfo<IInstance>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                    instantiatesClassProcessor,
                    () => CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            }.Concat(
                parameters.PropertyAssociationsParameters.Select(
                    u => new IndependentGreatGrannyInfo<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                    propertyAssociationProcessor,
                    () => u,
                    (g, r) => r.PropertyAssociations.Add(g)
                    )
                )
            );


        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerWriteOptions options, IInstanceParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet>(
                    instantiatesClassProcessor,
                    (n) => CreateInstantiatesClassParameterSet(parameters)
                )
            }.Concat(
                // create GrannyQueryInner for each PropertyAssociation in parameters
                parameters.PropertyAssociationsParameters.Select(
                    pa => new GrannyQueryInner<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet>(
                        propertyAssociationProcessor,
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerWriteOptions options, IInstanceParameterSet parameters, out IInstance result) =>
            new Instance().AggregateTryParse(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            ProcessHelper.TryParse
                        )
                    )
                ),
                ensemble,
                options,
                out result
            );
    }
}
