using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Readers
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

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassProcessor instantiatesClassProcessor, 
            IPropertyAssociationProcessor propertyAssociationProcessor,
            Id23neurULizerReadOptions options, 
            IInstanceParameterSet parameters,
            Ensemble ensemble
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                    instantiatesClassProcessor,
                    () => InstanceProcessor.CreateInstantiatesClassParameterSet(gc, parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            ).Concat(
                ProcessHelper.CreateGreatGrannyCandidates(
                    ensemble,
                    parameters.Granny,
                    gc => parameters.PropertyAssociationsParameters.Select(
                        u => new IndependentGreatGrannyInfo<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            propertyAssociationProcessor,
                            () => PropertyAssociationParameterSet.CreateWithGranny(
                                gc,
                                u.Property,
                                u.Class
                            ),
                            (g, r) => r.PropertyAssociations.Add(g)
                        )
                    )
                )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(Neuron grannyCandidate, IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Ensemble ensemble, Id23neurULizerReadOptions options, IInstanceParameterSet parameters, out IInstance result) =>
            new Instance().AggregateTryParse(
                parameters.Granny,
                InstanceProcessor.CreateGreatGrannies(
                    this.instantiatesClassProcessor,
                    this.propertyAssociationProcessor,
                    options, 
                    parameters,
                    ensemble
                    ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    ),
                    new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                options,
                1 + parameters.PropertyAssociationsParameters.Count(),
                out result
            );
    }
}
