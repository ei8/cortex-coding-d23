using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public class InstantiatesClassWriter : IInstantiatesClassWriter
    {
        private readonly IExpressionWriter expressionWriter;
        private readonly Readers.Deductive.IInstantiatesClassReader reader;
        private readonly IMirrorSet mirrors;

        public InstantiatesClassWriter(
            IExpressionWriter expressionWriter,
            Readers.Deductive.IInstantiatesClassReader reader,
            IMirrorSet mirrors
            )
        {
            this.expressionWriter = expressionWriter;
            this.reader = reader;
            this.mirrors = mirrors;
        }

        public bool TryBuild(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.TryBuildAggregate(
                () => new InstantiatesClass(),
                parameters,
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParseBuild
                    )
                },
                network,
                this.mirrors,
                out result
            );

        public bool TryCreateGreatGrannies(
            IInstantiatesClassParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = new IGreatGrannyInfo<IInstantiatesClass>[]
                {
                    new IndependentGreatGrannyInfo<IExpression, IExpressionWriter, IExpressionParameterSet, IInstantiatesClass>(
                        expressionWriter,
                        () => InstantiatesClassWriter.CreateSubordinationParameterSet(mirrors, parameters),
                        (g, r) => {
                            if (coreBResult = g.TryGetValueUnitGrannyByTypeId(mirrors.DirectObject.Id, out IUnit vuResult))
                                r.Class = vuResult;
                        }
                    )
                };
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IMirrorSet mirrors, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        mirrors.Instantiates,
                        mirrors.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        mirrors.DirectObject
                    )
                }
            );
        }

        public IGrannyReader<IInstantiatesClass, IInstantiatesClassParameterSet> Reader => this.reader;
    }
}
