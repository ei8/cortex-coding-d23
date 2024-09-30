using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstanceReader : IInstanceReader
    {
        private readonly IInstantiatesClassReader instantiatesClassReader;
        private readonly IPropertyAssociationReader propertyAssociationReader;

        public InstanceReader(IInstantiatesClassReader instantiatesClassReader, IPropertyAssociationReader propertyAssociationReader)
        {
            this.instantiatesClassReader = instantiatesClassReader;
            this.propertyAssociationReader = propertyAssociationReader;
        }

        private static IEnumerable<IGreatGrannyInfo<IInstance>> CreateGreatGrannies(
            IInstantiatesClassReader instantiatesClassReader,
            IPropertyAssociationReader propertyAssociationReader,
            IInstanceParameterSet parameters,
            Ensemble ensemble
        ) =>
            ProcessHelper.CreateGreatGrannyCandidates(
                ensemble,
                parameters.Granny,
                gc => new IndependentGreatGrannyInfo<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                    instantiatesClassReader,
                    () => InstanceReader.CreateInstantiatesClassParameterSet(gc, parameters),
                    (g, r) => r.InstantiatesClass = g
                )
            ).Concat(
                ProcessHelper.CreateGreatGrannyCandidates(
                    ensemble,
                    parameters.Granny,
                    gc => parameters.PropertyAssociationsParameters.Select(
                        u => new IndependentGreatGrannyInfo<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet, IInstance>(
                            propertyAssociationReader,
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

        public bool TryParse(Ensemble ensemble, IInstanceParameterSet parameters, out IInstance result) =>
            new Instance().AggregateTryParse(
                parameters.Granny,
                InstanceReader.CreateGreatGrannies(
                    this.instantiatesClassReader,
                    this.propertyAssociationReader,
                    parameters,
                    ensemble
                    ),
                new IGreatGrannyProcess<IInstance>[]
                {
                    new GreatGrannyProcess<IInstantiatesClass, IInstantiatesClassReader, IInstantiatesClassParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    ),
                    new GreatGrannyProcess<IPropertyAssociation, IPropertyAssociationReader, IPropertyAssociationParameterSet, IInstance>(
                        ProcessHelper.TryParse
                    )
                },
                ensemble,
                1 + parameters.PropertyAssociationsParameters.Count(),
                out result
            );
    }
}
