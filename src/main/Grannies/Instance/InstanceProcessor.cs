using ei8.Cortex.Coding.d23.neurULization;
using ei8.Cortex.Coding.d23.Queries;
using ei8.Cortex.Library.Common;
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
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTargetAsync<IInstance>[]
                {
                    new InnerProcessTargetAsync<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new InnerProcessTargetAsync<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                    )
                ),
                ensemble,
                options,
                () => ensemble.Obtain(Neuron.CreateTransient(null, null, null)),
                (r) => new[]
                    {
                        r.InstantiatesClass.Neuron
                    }.Concat(
                        // with PropertyAssociations in result
                        r.PropertyAssociations.Select(pa => pa.Neuron)
                    )
            );

        private IEnumerable<IInnerProcess<IInstance>> CreateInnerProcesses(Id23neurULizerOptions options, IInstanceParameterSet parameters) =>
            new IInnerProcess<IInstance>[]
            {
                new InnerProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                    this.instantiatesClassProcessor,
                    (g) => InstanceProcessor.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            }.Concat(
                parameters.PropertyAssociationsParameters.Select(
                    u => new InnerProcess<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                    this.propertyAssociationProcessor,
                    (g) => u,
                    (g, r) => r.PropertyAssociations.Add(g)
                    )
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
                )
            }.Concat(
                // create GrannyQueryInner for each PropertyAssociation in parameters
                parameters.PropertyAssociationsParameters.Select(
                    pa => new GrannyQueryInner<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet>(
                        this.propertyAssociationProcessor,
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

        public bool TryParse(Ensemble ensemble, Id23neurULizerOptions options, IInstanceParameterSet parameters, out IInstance result) =>
            new Instance().AggregateTryParse(
                this.CreateInnerProcesses(options, parameters),
                new IInnerProcessTarget<IInstance>[]
                {
                    new InnerProcessTarget<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new InnerProcessTarget<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
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
