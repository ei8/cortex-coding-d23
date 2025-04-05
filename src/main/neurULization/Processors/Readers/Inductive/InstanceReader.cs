using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceReader : IInstanceReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IPropertyValueAssociationReader propertyValueAssociationReader;
        private readonly IPropertyInstanceValueAssociationReader propertyInstanceValueAssociationReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;

        private static readonly IGreatGrannyProcess<IInstance>[] targets = new IGreatGrannyProcess<IInstance>[]
            {
                new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                    ProcessHelper.TryParse
                ),
                new GreatGrannyProcess<IPropertyValueAssociation, IPropertyValueAssociationReader, IPropertyValueAssociationParameterSet, IInstance>(
                    ProcessHelper.TryParse
                ),
                new GreatGrannyProcess<IPropertyInstanceValueAssociation, IPropertyInstanceValueAssociationReader, IPropertyInstanceValueAssociationParameterSet, IInstance>(
                    ProcessHelper.TryParse
                )
            };
        public InstanceReader(
            IInstantiatesClassReader instantiatesClassReader, 
            IPropertyValueAssociationReader propertyValueAssociationReader,
            IPropertyInstanceValueAssociationReader propertyInstanceValueAssociationReader,
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(instantiatesClassReader, nameof(instantiatesClassReader));
            AssertionConcern.AssertArgumentNotNull(propertyValueAssociationReader, nameof(propertyValueAssociationReader));
            AssertionConcern.AssertArgumentNotNull(propertyInstanceValueAssociationReader, nameof(propertyInstanceValueAssociationReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.instantiatesClassReader = instantiatesClassReader;
            this.propertyValueAssociationReader = propertyValueAssociationReader;
            this.propertyInstanceValueAssociationReader = propertyInstanceValueAssociationReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        public bool TryCreateGreatGrannies(
            IInstanceParameterSet parameters, 
            Network network, 
            IExternalReferenceSet externalReferences, 
            out IGreatGrannyInfoSuperset<IInstance> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = ProcessHelper.CreateGreatGrannyCandidateSet(
                    network,
                    parameters.Granny,
                    gc => new InductiveIndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                        gc,
                        instantiatesClassReader,
                        () => InstanceReader.CreateInstantiatesClassParameterSet(gc, parameters),
                        (g, r) => r.InstantiatesClass = g
                    ),
                    targets[0]
                ).AsSuperset()
                .Concat(
                    ProcessHelper.CreateGreatGrannyCandidateSets(
                        network,
                        parameters.Granny,
                        parameters.PropertyAssociationParameters.OfType<IPropertyValueAssociationParameterSet>(),
                        (gc, pa) => new InductiveIndependentGreatGrannyInfo<
                            IPropertyValueAssociation, 
                            IPropertyValueAssociationReader, 
                            IPropertyValueAssociationParameterSet, 
                            IInstance
                        >(
                            gc,
                            propertyValueAssociationReader,
                            () => PropertyValueAssociationParameterSet.CreateWithGranny(
                                gc,
                                pa.Property
                            ),
                            (g, r) => r.PropertyAssociations.Add(g)
                        ),
                        targets[1]
                    )
                )
                .Concat(
                    ProcessHelper.CreateGreatGrannyCandidateSets(
                        network,
                        parameters.Granny,
                        parameters.PropertyAssociationParameters.OfType<IPropertyInstanceValueAssociationParameterSet>(),
                        (gc, pa) => new InductiveIndependentGreatGrannyInfo<
                            IPropertyInstanceValueAssociation,
                            IPropertyInstanceValueAssociationReader,
                            IPropertyInstanceValueAssociationParameterSet,
                            IInstance
                        >(
                            gc,
                            propertyInstanceValueAssociationReader,
                            () => PropertyInstanceValueAssociationParameterSet.CreateWithGranny(
                                gc,
                                pa.Property,
                                pa.Class
                            ),
                            (g, r) => r.PropertyAssociations.Add(g)
                        ),
                        targets[2]
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(Neuron grannyCandidate, IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Network network, IInstanceParameterSet parameters, out IInstance result) =>
            this.aggregateParser.TryParse<Instance, IInstance, IInstanceParameterSet>(
                parameters.Granny,
                this,
                parameters,
                network,
                this.externalReferences,
                out result
            );
    }
}
