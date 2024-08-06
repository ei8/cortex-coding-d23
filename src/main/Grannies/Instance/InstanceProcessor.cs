﻿using ei8.Cortex.Coding.d23.neurULization;
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
                this.CreateGreatGrannies(options, parameters),
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
                () => {
                        Neuron result = null;
                        if (options is Id23neurULizerWriteOptions writeOptions)
                            ensemble.Obtain(
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

        private IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(Id23neurULizerOptions options, IInstanceParameterSet parameters) =>
            new IGreatGrannyInfo<IInstance>[]
            {
                new GreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                    this.instantiatesClassProcessor,
                    (g) => InstanceProcessor.CreateInstantiatesClassParameterSet(parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            }.Concat(
                parameters.PropertyAssociationsParameters.Select(
                    u => new GreatGrannyInfo<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
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
                this.CreateGreatGrannies(options, parameters),
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
