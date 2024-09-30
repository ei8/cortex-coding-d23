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
        private readonly IPrimitiveSet primitives;

        public InstantiatesClassWriter(
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstantiatesClassReader reader,
            IPrimitiveSet primitives
            )
        {
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.primitives = primitives;
        }

        public IInstantiatesClass Build(Ensemble ensemble, IInstantiatesClassParameterSet parameters) =>
            new InstantiatesClass().AggregateBuild(
                InstantiatesClassWriter.CreateGreatGrannies(
                    this.expressionWriter, 
                    parameters, 
                    this.primitives
                ),
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.ParseBuild
                    )
                },
                ensemble
            );

        private static IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> CreateGreatGrannies(
            IExpressionWriter expressionWriter, 
            IInstantiatesClassParameterSet parameters, 
            IPrimitiveSet primitives
        ) =>
           new IGreatGrannyInfo<IInstantiatesClass>[]
           {
                new IndependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                    expressionWriter,
                    () => InstantiatesClassWriter.CreateSubordinationParameterSet(primitives, parameters),
                    (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(primitives.DirectObject.Id).Single()
                )
           };

        private static ExpressionParameterSet CreateSubordinationParameterSet(IPrimitiveSet primitives, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        primitives.Instantiates,
                        primitives.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        primitives.DirectObject
                    )
                }
            );
        }

        public IGrannyReader<IInstantiatesClass, IInstantiatesClassParameterSet> Reader => this.reader;
    }
}
