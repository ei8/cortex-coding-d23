using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using ei8.Cortex.Library.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstanceProcessor : IInstanceProcessor
    {
        private readonly IInstantiatesClassProcessor instantiatesClassProcessor;
        private readonly IPropertyAssociationProcessor propertyAssociationProcessor;
        private readonly Readers.Deductive.IInstanceProcessor readProcessor;

        public InstanceProcessor(
            IInstantiatesClassProcessor instantiatesClassProcessor, 
            IPropertyAssociationProcessor propertyAssociationProcessor,
            Readers.Deductive.IInstanceProcessor readProcessor            
            )
        {
            this.instantiatesClassProcessor = instantiatesClassProcessor;
            this.propertyAssociationProcessor = propertyAssociationProcessor;
            this.readProcessor = readProcessor;
        }

        public async Task<IInstance> BuildAsync(Ensemble ensemble, Id23neurULizerWriteOptions options, IInstanceParameterSet parameters) =>
            await new Instance().AggregateBuildAsync(
                CreateGreatGrannies(options, parameters),
                new IGreatGrannyProcessAsync<IInstance>[]
                {
                    new GreatGrannyProcessAsync<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new GreatGrannyProcessAsync<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            ProcessHelper.ObtainWithAggregateParamsAsync
                        )
                    )
                ),
                ensemble,
                options,
                () =>
                {
                    Neuron result = null;
                    if (options is Id23neurULizerWriteOptions writeOptions)
                        result = ensemble.Obtain(
                            Neuron.CreateTransient(
                                parameters.Id,
                                parameters.Tag,
                                parameters.ExternalReferenceUrl,
                                parameters.RegionId
                            ),
                            writeOptions.OperationOptions.Mode == WriteMode.Update
                        );
                    return result;
                },
                (r) => new[]
                    {
                        r.InstantiatesClass.Neuron
                    }.Concat(
                        // with PropertyAssociations in result
                        r.PropertyAssociations.Select(pa => pa.Neuron)
                    )
            );

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

        public IGrannyReadProcessor<IInstance, IInstanceParameterSet> ReadProcessor => this.readProcessor;
    }
}
