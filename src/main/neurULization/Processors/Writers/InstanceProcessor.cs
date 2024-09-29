using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;
using System.Linq;

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

        public IInstance Build(Ensemble ensemble, IInstanceParameterSet parameters) =>
            new Instance().AggregateBuild(
                InstanceProcessor.CreateGreatGrannies(
                    this.instantiatesClassProcessor,
                    this.propertyAssociationProcessor,
                    parameters
                ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.ParseBuild
                    )
                }.Concat(
                    parameters.PropertyAssociationsParameters.Select(
                        u => new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationProcessor, IPropertyAssociationParameterSet, IInstance>(
                            ProcessHelper.ParseBuild
                        )
                    )
                ),
                ensemble,
                () =>
                {
                    Neuron result = ensemble.AddOrGetIfExists(
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
                        r.InstantiatesClass.Neuron
                    }.Concat(
                        // with PropertyAssociations in result
                        r.PropertyAssociations.Select(pa => pa.Neuron)
                    )
            );

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassProcessor instantiatesClassProcessor,
            IPropertyAssociationProcessor propertyAssociationProcessor, 
            IInstanceParameterSet parameters
        ) =>
            new IGreatGrannyInfo<IInstance>[]
            {
                new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassProcessor, IInstantiatesClassParameterSet, IInstance>(
                    instantiatesClassProcessor,
                    () => InstanceProcessor.CreateInstantiatesClassParameterSet(parameters),
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
