using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Library.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.Grannies
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

        public async Task<IInstance> BuildAsync(Ensemble ensemble, Id23neurULizerOptions options, IInstanceParameterSet parameters) =>
            await new Instance().AggregateBuildAsync(
                new IInnerProcess<IInstance>[]
                {
                    new InnerProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        this.instantiatesClassProcessor,
                        (g) => InstanceProcessor.CreateInstantiatesClassParameterSet(parameters),
                        (g, r) => r.InstantiatesClass = g,
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new InnerProcess<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            this.propertyAssociationProcessor,
                            (g) => u,
                            (g, r) => r.PropertyAssociations.Add(g),
                            ProcessHelper.ObtainWithAggregateParamsAsync
                            )
                    )
                ),
                ensemble,
                options,
                (n, r) => r.Neuron = ensemble.Obtain(Neuron.CreateTransient(null, null, null)),
                (r) => new[]
                    {
                        r.InstantiatesClass.Neuron 
                    }.Concat(
                        // with PropertyAssociations in result
                        r.PropertyAssociations.Select(pa => pa.Neuron)
                    )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                parameters.Class
            );

        public IEnumerable<IGrannyQuery> GetQueries(Id23neurULizerOptions options, IInstanceParameterSet parameters) =>
            new IGrannyQuery[] {
                new GrannyQueryInner<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet>(
                    this.instantiatesClassProcessor,
                    (n) => InstanceProcessor.CreateInstantiatesClassParameterSet(parameters)
                ) ,
                // TODO: create GrannyQueryInner for each PropertyAssociation in parameters
                // ... use Granny neurons of PropertyAssociations in PostsynapticIds along with id of InstantiatesClass
                new GrannyQueryBuilder(
                    (n) => new NeuronQuery()
                    {
                        DirectionValues = DirectionValues.Outbound,
                        Depth = 1,
                        Postsynaptic = n.Select(ne => ne.Neuron.Id.ToString())
                    }
                )
            };

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IInstanceParameterSet parameters, out IInstance result)
        {
            throw new NotImplementedException();
        }
    }
}
