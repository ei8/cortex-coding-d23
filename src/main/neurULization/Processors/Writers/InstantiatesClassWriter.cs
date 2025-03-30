using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstantiatesClassWriter : IInstantiatesClassWriter
    {
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IInstantiatesClassReader reader;
        private readonly IExternalReferenceSet externalReferences;

        public InstantiatesClassWriter(
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstantiatesClassReader reader,
            IExternalReferenceSet externalReferences
            )
        {
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.externalReferences = externalReferences;
        }

        public bool TryBuild(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.TryBuildAggregate(
                () => new InstantiatesClass(),
                parameters,
                InstantiatesClassWriter.CreateGreatGrannies(
                    this.expressionWriter, 
                    parameters, 
                    this.externalReferences
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParseBuild
                    )
                },
                network,
                out result
            );

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(
            IExpressionWriter expressionWriter, 
            IInstantiatesClassParameterSet parameters, 
            IExternalReferenceSet externalReferences
        ) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                    expressionWriter,
                    () => InstantiatesClassWriter.CreateSubordinationParameterSet(externalReferences, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(externalReferences.DirectObject.Id).Single()
                )
           };

        private static ExpressionParameterSet CreateSubordinationParameterSet(IExternalReferenceSet externalReferences, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        externalReferences.Instantiates,
                        externalReferences.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        externalReferences.DirectObject
                    )
                }
            );
        }

        public IGrannyReader<IInstantiatesClass, IInstantiatesClassParameterSet> Reader => this.reader;
    }
}
