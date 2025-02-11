using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceReader : IInstanceReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IPropertyAssociationReader propertyAssociationReader;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IInstance>[] targets = new IGreatGrannyProcess<IInstance>[]
            {
                new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                    ProcessHelper.TryParse
                ),
                new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet, IInstance>(
                    ProcessHelper.TryParse
                )
            };
    public InstanceReader(
            IInstantiatesClassReader instantiatesClassReader, 
            IPropertyAssociationReader propertyAssociationReader,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(instantiatesClassReader, nameof(instantiatesClassReader));
            AssertionConcern.AssertArgumentNotNull(propertyAssociationReader, nameof(propertyAssociationReader));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.instantiatesClassReader = instantiatesClassReader;
            this.propertyAssociationReader = propertyAssociationReader;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IInstance> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IPropertyAssociationReader propertyAssociationReader,
            IInstanceParameterSet parameters,
            Network network
        ) =>
            ProcessHelper.CreateGreatGrannyCandidateSet(
                network,
                parameters.Granny,
                gc => new InductiveIndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                    gc,
                    instantiatesClassReader,
                    () => InstanceReader.CreateInstantiatesClassParameterSet(gc, parameters),
                    (g, r) => r.InstantiatesClass = g
                ),
                targets[0]
            ).AsSuperset().Concat(
                ProcessHelper.CreateGreatGrannyCandidateSets(
                    network,
                    parameters.Granny,
                    parameters.PropertyAssociationsParameters,
                    (gc, pa) => new InductiveIndependentGreatGrannyInfo<
                        IPropertyAssociation, 
                        IPropertyAssociationReader, 
                        IPropertyAssociationParameterSet, 
                        IInstance
                    >(
                        gc,
                        propertyAssociationReader,
                        () => PropertyAssociationParameterSet.CreateWithGranny(
                            gc,
                            pa.Property,
                            pa.Class
                        ),
                        (g, r) => r.PropertyAssociations.Add(g)
                    ),
                    targets[1]
                )
            );

        private static IInstantiatesClassParameterSet CreateInstantiatesClassParameterSet(Neuron grannyCandidate, IInstanceParameterSet parameters) =>
            new InstantiatesClassParameterSet(
                grannyCandidate,
                parameters.Class
            );

        public bool TryParse(Network network, IInstanceParameterSet parameters, out IInstance result) =>
            this.aggregateParser.TryParse<Instance, IInstance>(
                parameters.Granny,
                InstanceReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    this.propertyAssociationReader,
                    parameters,
                    network
                ),
                network,
                out result
            );
    }
}
